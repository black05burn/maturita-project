using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

	public static event System.Action PlayerHasEnteredFinish;

	#region Variables
	[Header("Developer mode")]
	public bool devMode;
	public static bool dev;

	[Header("Important values")]
	public float moveSpeed = 5f;
	public float blinkCooldown = 5f;
	public float invisibilityCooldown = 5f;

	//auxiliary values
	public static bool isInvisible = false;
	bool blink = true;
	bool invisibility = false;
	bool playerHasFoundAllKeys = false;

	//input
	Camera viewCam;
	PlayerController controller;
	Vector3 point;

	[Space]
	public List<GameObject> keysOnLevel;
	#endregion

	#region Unity Methods

	private void Awake()
	{
		viewCam = Camera.main;
		controller = GetComponent<PlayerController>();
	}

	private void Start()
	{
		//DEV MODE
		dev = devMode;
		isInvisible = false;
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
	}

	private void Update()
	{
		//movement input
		Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		controller.Move(moveInput * moveSpeed);

		//mouse input
		Ray ray = viewCam.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

		float rayDistance;
		if (groundPlane.Raycast(ray, out rayDistance))
		{
			//look at mouse point
			point = ray.GetPoint(rayDistance);
			controller.LookAt(point);
		}


		# region POWERS

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
			StartCoroutine(controller.Blink(point, blinkCooldown));
		}
		if (invisibility)
		{
			Game.instance.blinkActive.enabled = false;
			Game.instance.invisibilityActive.enabled = true;
			StartCoroutine(controller.Invisibility(invisibilityCooldown));
		}
		#endregion
	}

	//FINISH and KEY
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
					print("You need to find a KEY");
					//GAME UI needed here!!!
				}
			}
		}

		if (collision.collider.CompareTag("Key"))
		{
			collision.gameObject.SetActive(false);

			if (keysOnLevel.Count > 0)
			{
				keysOnLevel.RemoveAt(keysOnLevel.Count - 1);
				if (keysOnLevel.Count == 0)
				{
					playerHasFoundAllKeys = true;
					GameObject.FindGameObjectWithTag("Finish").GetComponent<Renderer>().material.color = new Color(0, .5f, 0);
				}
			}


			print("You have found the key");
			//GAME UI needed here!!!
		}
	}

	#endregion



}
