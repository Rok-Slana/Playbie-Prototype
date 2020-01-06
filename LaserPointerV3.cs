using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Game Object needs a Liner Rendere attached to it for this class to work
//              !! SIZE OF LINE RENDERER NEEDS TO BE SET TO 3 !!

public class LaserPointerV3 : MonoBehaviour
{
    private LineRenderer lineRend;
    private Vector3 pI = Vector3.positiveInfinity;
    public bool laserState { get; set; }
    //**
    private int maxRayIndex = 7;
    private int laserRayIndex = 0;
    private bool maximumReflections = false;
    private Vector3 aim = Vector3.up;
    //**
    void Start()
    {
        laserState = true;
        lineRend = GetComponent<LineRenderer>();
        lineRend.positionCount = maxRayIndex;
        lineRend.SetPosition(0, gameObject.transform.position);

        lineRend.SetPosition(1, Vector3.up * 50);
        lineRend.SetPosition(2, Vector3.up * 50);
        lineRend.SetPosition(3, Vector3.up * 50);
        lineRend.SetPosition(4, Vector3.up * 50);
        lineRend.SetPosition(5, Vector3.up * 50);
        lineRend.SetPosition(6, Vector3.up * 50);
    }

    void FixedUpdate()
    {
        //Cast and render LaserRay path
        CastLaserRay(gameObject.transform.position, aim);
        //Reset LasesRay's LineRenderer points index
        laserRayIndex = 0;
        //Reset Maximum reflections
        maximumReflections = false;
    }

    private void CastLaserRay(Vector3 position, Vector3 target)
    {
        Vector3 aimingDirection = gameObject.transform.TransformDirection(target);
        Debug.DrawRay(position, aimingDirection, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(position, aimingDirection, out hit))
        {
            //If obstacle is 'bouncy' reflect laserRay
            if (hit.collider.tag == "Bouncer")
            {
                RenderLaserRay(hit.point);
                if (!maximumReflections) CastLaserRay(hit.point, Vector3.Reflect(hit.point - position, hit.normal));
            }
            //if obstacle is not 'bouncy', stop laserRay at hit.point
            else
            {
                RenderLaserRay(hit.point);
            }
        }
        else
        {
            //Handle the last reflections 'outward' line, if there are reflections
            if (laserRayIndex > 1)
            {
                Vector3 incomingVector = position + aimingDirection;
                lineRend.SetPosition(laserRayIndex+1, hit.point + Vector3.Reflect(incomingVector, hit.normal));
            }

            //If ray isn't hiting anything, just render a line in the direction where the GO is aiming
            else
            {
                RenderLaserRay(aimingDirection * 50.0f);
            }
        }

     
    }

    private void RenderLaserRay(Vector3 directionPoint)
    {
        laserRayIndex++;

        for (int i = laserRayIndex; i < maxRayIndex; i+=1)
        {
            lineRend.SetPosition(i, directionPoint);
        }

        if (laserRayIndex >= 6)
        {
            maximumReflections = true;
        }
    }

    public void AimLaser(Vector3 target)
    {
       aim = target;
    }
}
