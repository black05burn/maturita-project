using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public static event System.Action PlayerHasEnteredFinish;

    // DEV MOD
    public bool devMod;
    public static bool dev;


    public float moveSpeed = 5f;
    public float blinkCooldown = 5f;
    public float invisibilityCooldown = 5f;

    public static bool isInvisible = false;
    bool blink = true;
    bool invisibility = false;

    Camera viewCam;
    PlayerController controller;
    Vector3 point;

    private void Awake()
    {
        dev = devMod; // DEV MOD
        viewCam = Camera.main;
        controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        Guard.OnGuardHasSpottedPlayer += Die;
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
            point = ray.GetPoint(rayDistance);
            //Debug.DrawRay(transform.position, point);
            controller.LookAt(point);
        }

        //POWERS
        if (!dev)
        {
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
        }

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

    void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Guard.OnGuardHasSpottedPlayer -= Die;
    }

}
