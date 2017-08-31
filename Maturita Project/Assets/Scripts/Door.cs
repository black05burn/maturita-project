using UnityEngine;

public class Door : MonoBehaviour {

	#region Variables
	bool doorOpen;
	public GameObject door;
	float offset;

	#endregion

	#region Unity Methods
	private void Start()
	{
		doorOpen = false;
		offset = door.transform.localScale.y;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Guard"))
		{
			if (!doorOpen)
			{
				door.transform.position -= new Vector3(0f, offset, 0f);
				doorOpen = true;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (doorOpen)
		{
			door.transform.position += new Vector3(0f, offset, 0f);
			doorOpen = false;
		}
	}
	#endregion
}
