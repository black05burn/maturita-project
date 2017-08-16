using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Transform deahtEffect;

    private Transform target;
    private float speed = 70f;


    public void Seek(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        //target is not alive
        if (target == null)
        {
            //destroy bullet
            Destroy(gameObject);
            return;
        }

        //direction to target
        Vector3 dir = target.position - transform.position;

        float distanceThisFrame = speed * Time.deltaTime;

        //we can overshoot (travel further than target)
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }

        //bullet movement
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    private void HitTarget()
    {
        //destroy target and bullet & show particles
        Destroy(gameObject);
        Destroy(target.gameObject);
        Instantiate(deahtEffect, transform.position, transform.rotation);
    }
}
