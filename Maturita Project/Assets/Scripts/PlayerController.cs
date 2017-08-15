using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour {

    public LayerMask blinkObstacleMask;
    public GameObject blinkCube;

    public float blinkRange = 5f;


    Rigidbody myRigidbody;
    Vector3 velocity;

    LineRenderer lr;
    Color colorOfBlinkCube = new Color(0, 255, 255, .25f);
    Color playerColor;

    bool invisible = false;
    bool canBlink = true;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCube;
        playerColor = GetComponent<Renderer>().material.color;
        //blinkCube = GameObject.FindGameObjectWithTag("BlinkCube");
    }

    public void LookAt(Vector3 point)
    {
        transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
    }

    public void Move(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    //rigidbody uses fixedupdate
    private void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }

    public void Blink(Vector3 point, float cooldown)
    {
        StartCoroutine(BlinkC(point, cooldown));
    }

    IEnumerator BlinkC(Vector3 point, float cooldown)
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

            //max distance of blink
            if (Player.dev) // DEV MOD
            {
                blinkCube.transform.position = lr.GetPosition(1);
                if (Input.GetMouseButtonDown(0))
                {
                    transform.position = blinkCube.transform.position;
                }
            }
            else
            {
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
                //blink to position
                if (Input.GetMouseButtonDown(0) && canBlink)
                {
                    transform.position = blinkCube.transform.position;
                    GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
                    //cooldown + change of color
                    canBlink = false;
                    blinkCube.GetComponent<Renderer>().sharedMaterial.color = new Color(80, 80, 80, .25f);

                    for (int i = 0; i < cooldown; i++)
                    {
                        Game.instance.cooldownBlinkText.text = cooldown - i + "";
                        yield return new WaitForSeconds(1);
                    }

                    Game.instance.cooldownBlinkText.text = "";
                    canBlink = true;
                    blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCube;
                }
            }
        }
    }

    public void Invisibility(float cooldown)
    { 
        StartCoroutine(InvisibilityC(cooldown));
    }

    IEnumerator InvisibilityC(float cooldown)
    {
        blinkCube.SetActive(false);
        lr.startWidth = 0f;
        if (Player.dev) // DEV MOD
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
        else if (Input.GetMouseButtonDown(1) && !invisible)
        {
            invisible = true;
            GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1 / 3f);
            Player.isInvisible = true;

            for (int i = 0; i < cooldown; i++)
            {
                if (GetComponent<Renderer>().material.color == playerColor)
                {
                    Game.instance.cooldownInvisibilityText.text = cooldown - i + "";
                }
                else
                {
                    if (i < Mathf.FloorToInt(cooldown/2))
                    {
                        Game.instance.cooldownInvisibilityText.text = cooldown/2 - i + "";
                    }
                    else if (i == Mathf.FloorToInt(cooldown/2))
                    {
                        Player.isInvisible = false;
                        GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
                        Game.instance.cooldownInvisibilityText.text = cooldown / 2 + "";
                    }
                }
                yield return new WaitForSeconds(1);
            }

            Game.instance.cooldownInvisibilityText.text = "";
            invisible = false;

        }
    }
}
