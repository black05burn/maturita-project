using UnityEngine;

public class Door : MonoBehaviour {

	#region Variables
	Animator animator;
	bool doorOpen;
	public GameObject door;
	#endregion

	#region Unity Methods
	private void Start()
	{
		doorOpen = false;
		//animator = GetComponent<Animator>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") || other.CompareTag("Guard"))
		{
			if (!doorOpen)
			{
				doorOpen = true;
				door.transform.Translate(0f, 0f, 3f);
			}
			//DoorController("Open");
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (doorOpen)
		{
			doorOpen = false;
			door.transform.Translate(0f, 0f, -3f);
			//DoorController("Close");
		}
	}
	#endregion

	void DoorController(string direction)
	{
		animator.SetTrigger(direction);
	}
}
