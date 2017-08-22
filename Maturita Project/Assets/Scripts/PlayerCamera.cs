using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	#region Variables
	Transform player;

	public float smoothSpeed = 10f;
	public Vector3 offset = new Vector3(0f, 30f, 0f);
	#endregion

	#region Unity Methods
	private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

	private void LateUpdate()
    {
		if (player != null)
		{
			Vector3 desiredPos = player.position + offset;
			Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
			transform.position = smoothedPos;
		}
    }
	#endregion

}
