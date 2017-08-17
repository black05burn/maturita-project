using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour {

    [Header("Blink")]
    public LayerMask blinkObstacleMask;
    public GameObject blinkCube;
    public float blinkRange = 5f;
    public AudioClip blinkSound;

    LineRenderer lr;
    Color colorOfBlinkCube = new Color(0, 255, 255, .25f);
    bool canBlink = true;

    //invisibility
    Color playerColor;
    bool invisible = false;

    //movement
    Rigidbody myRigidbody;
    Vector3 velocity;

    public GameObject playerHolder;
    Animator anim;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        blinkCube.GetComponent<Renderer>().sharedMaterial.color = colorOfBlinkCube;
        playerColor = GetComponent<Renderer>().material.color;
        anim = playerHolder.GetComponent<Animator>();
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


    // --== BLINK ==--

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

            // DEV MODE
            if (Player.dev)
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
                    Player.isInvisible = false;
                    //play audio and animation
                    Audio.instance.PlaySound(blinkSound, transform.position);
                    anim.Play("BlinkAnimation");
                    //change of color on cooldown
                    blinkCube.GetComponent<Renderer>().sharedMaterial.color = new Color(80, 80, 80, .25f);
                    canBlink = false;

                    for (int i = 0; i < cooldown; i++)
                    {
                        //cooldown
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

    // --== INVISIBILITY ==--

    public void Invisibility(float cooldown)
    { 
        StartCoroutine(InvisibilityC(cooldown));
    }

    IEnumerator InvisibilityC(float cooldown)
    {
        //initial setup
        blinkCube.SetActive(false);
        lr.startWidth = 0f;

        // DEV MODE
        if (Player.dev)
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
            GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1 / 3f);
            GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            Player.isInvisible = true;

            //COOLDOWN
            for (int i = 0; i < cooldown; i++)
            {
                //if visible show actual cooldown
                if (GetComponent<Renderer>().material.color == playerColor)
                {
                    Game.instance.cooldownInvisibilityText.text = cooldown - i + "";
                }
                else
                {
                    //show invisible time left
                    if (i < Mathf.FloorToInt(cooldown/2))
                    {
                        Game.instance.cooldownInvisibilityText.text = cooldown/2 - i + "";
                    }
                    //invisible to visible
                    else if (i == Mathf.FloorToInt(cooldown/2))
                    {
                        Player.isInvisible = false;
                        GetComponent<Renderer>().material.color = new Color(playerColor.r, playerColor.g, playerColor.b, 1f);
                        GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
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
