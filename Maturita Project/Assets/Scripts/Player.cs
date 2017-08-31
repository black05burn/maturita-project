using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

	public static event System.Action PlayerHasEnteredFinish;

	#region Variables
	[Header("Important values")]
	public float moveSpeed = 5f;
	public float blinkCooldown = 5f;
	public float invisibilityCooldown = 5f;
	public float invisibilityDuration = 3f;

	//auxiliary values
	public static bool isInvisible;
	bool blink = true;
	bool invisibility = false;

	//input
	Camera viewCam;
	PlayerController controller;
	Vector3 velocity;

	[Space]
	public Transform keysHolder;
	bool playerHasFoundAllKeys = false;
	#endregion

	#region Unity Methods

	private void Awake()
	{ 
		controller = GetComponent<PlayerController>();
	}

	private void Start()
	{
		viewCam = Camera.main;
		isInvisible = false;
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		//text to show how many keys left
		Game.instance.keys.text = (keysHolder.childCount - 1 > 1) ? string.Format("Find {0} keys", keysHolder.childCount) : "Find 1 key";
	}

	private void Update()
	{
		//input
		Vector3 mousePos = viewCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCam.transform.position.y));
		transform.LookAt(mousePos + Vector3.up * transform.position.y);
		velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
		controller.Move(velocity);

		#region POWERS

		//changing powers
		if (Input.GetKeyDown("1"))
		{
			blink = true;
			invisibility = false;
		}
		else if (Input.GetKeyDown("2"))
		{
			invisibility = true;
			blink = false;
		}

		//starting powers
		if (blink)
		{
			Game.instance.blinkActive.enabled = true;
			Game.instance.invisibilityActive.enabled = false;
			StartCoroutine(controller.Blink(mousePos, blinkCooldown));
		}
		if (invisibility)
		{
			Game.instance.blinkActive.enabled = false;
			Game.instance.invisibilityActive.enabled = true;
			StartCoroutine(controller.Invisibility(invisibilityCooldown, invisibilityDuration));
		}

		#endregion
	}

	#region FINISH
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Finish"))
		{
			if (PlayerHasEnteredFinish != null)
			{
				if (playerHasFoundAllKeys)
				{
					PlayerHasEnteredFinish();
				}
				else
				{
					if (gameObject != null)
					{
						StartCoroutine(Game.instance.ShowHideFinishText());
					}
					//GAME UI needed here!!!
				}
			}
		}
		//collect key (count number of keys on the map)
		if (collision.collider.CompareTag("Key"))
		{
			//text to show how many keys left
			Game.instance.keys.text = (keysHolder.childCount - 1 > 1) ? string.Format("Find {0} keys", keysHolder.childCount - 1) : "Find 1 key";
			Destroy(collision.gameObject);

			if (keysHolder.childCount - 1 == 0)
			{
				playerHasFoundAllKeys = true;
				GameObject.FindGameObjectWithTag("Finish").GetComponent<Renderer>().material.color = new Color(0, .5f, 0);
				Game.instance.keys.text = "Go to FINISH";
			}
			//GAME UI needed here!!!
		}
	}
	#endregion

	#endregion
}
