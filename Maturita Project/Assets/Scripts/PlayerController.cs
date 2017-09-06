using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{

    #region Variables
    [Header("Blink")]
    public LayerMask blinkObstacleMask;
    public GameObject blinkCube;
    public float blinkRange = 5f;
    public AudioClip blinkSound;

    LineRenderer lr;
	#region Colors of BlinkCube
    public Color colorOfBlinkCube;
	public Color colorOfBlinkCubeOnCooldown;
	#endregion
	bool canBlink = true;

    //invisibility
    Color playerColor;
    bool invisible = false;

    //movement
    Rigidbody myRigidbody;
    Vector3 velocity;

    public GameObject playerHolder;
    Animator anim;
	#endregion

	#region Unity Methods
	private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCube;
        playerColor = GetComponent<Renderer>().material.color;
        anim = playerHolder.GetComponent<Animator>();
    }

	//rigidbody uses fixedupdate
	private void FixedUpdate()
	{
		myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
	}

	#endregion

	public void LookAt(Vector3 point)
    {
        transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    #region POWERS
	public IEnumerator Blink(Vector3 point, float cooldown)
    {
        //hide line and blink cube
        blinkCube.SetActive(false);
        lr.startWidth = 0f;

        if (Input.GetMouseButton(1))
        {
            //change height of point to match players height
            point = new Vector3(point.x, transform.position.y, point.z);
            //show line and blink cube
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, point);
            lr.startWidth = .1f;
            blinkCube.SetActive(true);
            //set blink cubes position to the end of the line
            blinkCube.transform.position = lr.GetPosition(1);
            blinkCube.transform.rotation = transform.rotation;

            // DEV MODE
            if (Game.dev)
            {
                blinkCube.transform.position = lr.GetPosition(1);
                if (Input.GetMouseButtonDown(0))
                {
                    transform.position = blinkCube.transform.position;
                }
            } 
            // NORMAL MODE
            else
            {
                //blink range
                if (Vector3.Distance(blinkCube.transform.position, transform.position) > blinkRange)
                {
                    lr.SetPosition(1, (blinkCube.transform.position - transform.position).normalized * blinkRange + transform.position);
                    blinkCube.transform.position = lr.GetPosition(1);
                }

                //collision
                RaycastHit hit;
                float maxDistOfColDetection = Vector3.Distance(transform.position, blinkCube.transform.position);
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxDistOfColDetection, blinkObstacleMask))
                {
                    lr.SetPosition(1, hit.point);
                    blinkCube.transform.position = hit.point;
                }

                if (Input.GetMouseButtonDown(0) && canBlink)
                {
                    //blink to position
                    transform.position = blinkCube.transform.position;

                    //set player to visible
                    GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
					Game.instance.durationInvisibilityImage.fillAmount = 0f;
					Player.isInvisible = false;

                    //play audio and animation
                    Audio.instance.PlaySound(blinkSound, transform.position);
                    anim.Play("BlinkAnimation");

                    //change of color on cooldown
                    blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCubeOnCooldown;
                    canBlink = false;

					float c = cooldown;
					while (c > 0f)
					{
						c -= Time.deltaTime;
						Game.instance.cooldownBlinkText.text = (c > 0) ? Mathf.CeilToInt(c).ToString() : null;
						Game.instance.cooldownBlinkImage.fillAmount = c / cooldown;
						yield return null;
					}
                    canBlink = true;
                    blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCube;
                }
            }
        }
    }

    public IEnumerator Invisibility(float cooldown, float duration)
    {
        //initial setup
        blinkCube.SetActive(false);
        lr.startWidth = 0f;

        // DEV MODE
        if (Game.dev)
        {
			if (Input.GetMouseButtonDown(1)) invisible = !invisible;

            if (invisible)
            {
                GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1 / 3f);
            }
            else
            {
                GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
            }
            Player.isInvisible = invisible;
            yield return null;
        }
        //NORMAL MODE
        else if (Input.GetMouseButtonDown(1) && !invisible)
        {
            //change color to "invisible"
            invisible = true;
            Player.isInvisible = true;
            GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1 / 3f);
            GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

			//cooldown
			float c = cooldown;
			while (c > 0f)
			{
				c -= Time.deltaTime;
				//duration of invisibility
				float d = duration;
				while (d > 0f && Player.isInvisible)
				{
					d -= Time.deltaTime;
					Game.instance.durationInvisibilityImage.fillAmount = d / duration;
					yield return null;
				}
				//making player visible
				Player.isInvisible = false;
				GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
				GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;

				Game.instance.cooldownInvisibilityText.color = Color.white;
				Game.instance.cooldownInvisibilityText.text = (c > 0) ? Mathf.CeilToInt(c).ToString() : null;
				Game.instance.cooldownInvisibilityImage.fillAmount = (c > 0f) ? c / cooldown : 0f;
				yield return null;
			}

			invisible = false;
        }
    }
    #endregion
}
