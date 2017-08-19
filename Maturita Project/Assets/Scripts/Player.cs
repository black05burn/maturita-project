using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public static event System.Action PlayerHasEnteredFinish;

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

    //input
    Camera viewCam;
    PlayerController controller;
    Vector3 point;

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

        //POWERS

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
            controller.Blink(point, blinkCooldown);
        }
        if (invisibility)
        {
            Game.instance.blinkActive.enabled = false;
            Game.instance.invisibilityActive.enabled = true;
            controller.Invisibility(invisibilityCooldown);
        }
    }

    //FINISH
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Finish"))
        {
            if (PlayerHasEnteredFinish != null)
            {
                PlayerHasEnteredFinish();
            }
        }
    }
}
