using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game Object needs a Liner Rendere attached to it for this class to work
//              !! SIZE OF LINE RENDERER NEEDS TO BE SET TO 3 !!

public class LaserPointer : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector3 pI = Vector3.positiveInfinity;
    private Vector3 fwd = Vector3.zero;
    public bool laserState { get; set; }

    void Start()
    {
        laserState = false;
        lineRend = GetComponent<LineRenderer>();
        lineRend.SetPosition(0, gameObject.transform.position);
        lineRend.SetPosition(1, gameObject.transform.position);
        lineRend.SetPosition(2, gameObject.transform.position);
    }

    void FixedUpdate()
    {
        if (laserState)
        {
            //fwd = gameObject.transform.TransformDirection(Vector3.forward);
            //Debug.DrawRay(gameObject.transform.position, fwd, Color.yellow);
            lineRend.SetPosition(0, gameObject.transform.position);
            lineRend.SetPosition(1, fwd * 500);
            lineRend.SetPosition(2, fwd * 500);

            RaycastHit hit;
            if (Physics.Raycast(gameObject.transform.position, fwd * 500, out hit, 5000))
            {
                if (hit.collider.tag == "Bouncer")
                {
                    Vector3 incomingVector = hit.point - gameObject.transform.position;
                    Vector3 reflectVector = Vector3.Reflect(incomingVector, hit.normal);

                    //Render the line emitting from the player
                    lineRend.SetPosition(0, gameObject.transform.position);
                    lineRend.SetPosition(1, hit.point);

                    //Render the reflected line
                    //Vector3.Reflect has to be used again, otherwise the line renders in the direction of the surface normal !!
                    lineRend.SetPosition(2, hit.point + Vector3.Reflect(incomingVector, hit.normal));
                }
                else
                {
                    lineRend.SetPosition(1, hit.point);
                    lineRend.SetPosition(2, hit.point);
                }
            }
        }
        else
        {
            lineRend.SetPosition(1,gameObject.transform.position);
            lineRend.SetPosition(2,gameObject.transform.position);
        }

    }

    public void RotateLaser(Vector3 target)
    {
        fwd = target;
    }
}
