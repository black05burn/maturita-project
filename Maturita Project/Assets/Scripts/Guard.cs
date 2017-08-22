using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    //event for other classes
    public static event System.Action OnGuardHasSpottedPlayer;

    #region Variables
    [Header("Movement")]
    public float speed = 5f;
    public float waitTime = 0.3f;
    public float turnSpeed = 90f;
    public float timeToSpotPlayer = .5f;
    public Transform pathHolder;

    [Header("Field of view")]
    public Light spotLight;
    public float viewDistance;
    public LayerMask viewMask;
    private float viewAngle;
    private float playerVisibleTimer;
    private Color originalSpotlightColor;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioClip shootSound;
    bool shot = false;

    Transform player;

	#endregion

	#region Unity Methods
	private void Awake()
    {
        originalSpotlightColor = spotLight.color;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotLight.spotAngle;
    }

    private void Start()
    {
        //searching for waypoints and making array of waypoints
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        //follow waypoints
        StartCoroutine(FollowPath(waypoints));
    }

    private void Update()
    {
        //change of color when player is seen
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
            spotLight.color = Color.red;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
            spotLight.color = originalSpotlightColor;
        }


        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotLight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);

        //can see player for too long
        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            //guard aims on player
            Vector3 dir = player.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = lookRotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            //shoot once
            if (!shot)
            {
                Shoot();
                shot = true;
            }

            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }

	//Gizmos in inspector (debuging tools)
	private void OnDrawGizmos()
	{
		Vector3 startPos = pathHolder.GetChild(0).position;
		Vector3 prevPos = startPos;
		foreach (Transform waypoint in pathHolder)
		{
			Gizmos.DrawSphere(waypoint.position, .3f);
			Gizmos.DrawLine(prevPos, waypoint.position);
			prevPos = waypoint.position;
		}
		Gizmos.DrawLine(prevPos, startPos);
		Gizmos.color = Color.red;
		Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
	}

	#endregion

	bool CanSeePlayer()
    {
        //player is alive and visible
        if (player != null && !Player.isInvisible)
        {
            //player is in view distance
            if (Vector3.Distance(transform.position, player.position) < viewDistance)
            {
                Vector3 dirToPlayer = (player.position - transform.position).normalized;
                float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

                //player is in view angle
                if (angleBetweenGuardAndPlayer < viewAngle / 2f)
                {
                    //nothing stands between player and guard
                    if (!Physics.Linecast(transform.position, player.position, viewMask))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        //initial setup
        transform.position = waypoints[0];
        int targetWaypointIdx = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIdx];
        transform.LookAt(targetWaypoint);

        //player is alive
        while (player != null)
        {
            //move towards waypoint
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

            //move towards next waypoint
            if (transform.position == targetWaypoint)
            {
                targetWaypointIdx = (targetWaypointIdx + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIdx];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 target)
    {
        //direction and angle to next waypoint
        Vector3 directionToTarget = (target - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(directionToTarget.z, directionToTarget.x) * Mathf.Rad2Deg;

        //while not looking at waypoint
        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            //look at waypoint
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    void Shoot()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            Audio.instance.PlaySound(shootSound, firePoint.position);
            bullet.Seek(player);
        }
    }

    
}
