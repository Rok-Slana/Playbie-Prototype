using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float projectileThrust = 2.5f;
    public float maxSwipeDistance = 100.0f;
    public Projectile projectile;

    public GameObject laserPointer;
    private LaserPointerV3 laser;

    private Vector3 mousePositionOnBtnUp = new Vector3();
    private Vector3 mousePositionOnBtnUpWorld = new Vector3();
    private Vector3 currentMousePosition = new Vector3();
    private Vector3 currentMousePositionWorld = new Vector3();
    private Vector3 swipeRendererDepth = new Vector3(0, 0, 5);
    //private Vector3 defaultDepth = new Vector3(0, 0, 0);

    private LineRenderer swipeRenderer;
    private float distance;
    private Camera mainCamera;
    private AudioManager aM;

    private bool swipeMotion = false;

    void Start()
    {
        //projectile = FindObjectOfType<Projectile>();
        mainCamera = Camera.main;
        swipeRenderer = GetComponent<LineRenderer>();//FindObjectOfType<LineRenderer>();
        //swipeRenderer.transform.position = gameObject.transform.position;
        swipeRenderer.SetPosition(0, this.transform.position);
        swipeRenderer.SetPosition(1, this.transform.position);
        aM = FindObjectOfType<AudioManager>();
        Debug.Log("Audio Manager : "+aM);
        laser = laserPointer.GetComponent<LaserPointerV3>();
    }

    void Update()
    {
        if (swipeMotion)
        {
            //Debug.Log("Swipe motion...Update");
            ShowSwipeMotion();
        }/*
        else
        {
            laser.AimLaser(Vector3.up*50.0f);

        }*/
    }

    //When player clicks/taps on Player GO, start rendering swipe motion
    private void OnMouseDown()
    {
        //Debug.Log("Swipe motion...Call");        
        swipeMotion = true;
    }

    private void OnMouseUp()
    {
        /////////
        ///
        //Fire();
        /////////
        ///
       
        //Code proceeds only if swipe motion is less that maxSwipeDistance long ( calculated and set in ShowSwipeMotion() )
        if (swipeMotion)
        {
            //Reset mouse renderer when mouse button is released (instead of disabling it or whatever)
            //swipeRenderer.SetPosition(1, this.transform.position + swipeRendererDepth);
            Fire();
            swipeMotion = false;
        }
    }

    //Render line while player swipes
    private void ShowSwipeMotion()
    {
        //get position of mouse while mouse button is pressed
        currentMousePosition = Input.mousePosition;
        currentMousePosition.z = 400.0f;

        //convert mouse position to world position
        //currentMousePostionWorld.y += this.transform.position.y;
        currentMousePositionWorld = mainCamera.ScreenToWorldPoint(currentMousePosition);//new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));//currentMousePostion);

        //send world position of mouse to Swipe Renderer and render line between Player GO and mouse position
        swipeRenderer.SetPosition(1, currentMousePositionWorld + swipeRendererDepth);
      

        //Fire projectile and reset swipeMotion if swipe motion exceeds maxSwipeDistance
        distance = Vector3.Distance(this.transform.position, currentMousePositionWorld);

        if (distance >= maxSwipeDistance)
        {
            swipeMotion = false;
            swipeRenderer.SetPosition(1, this.transform.position + swipeRendererDepth);
            Fire();
        }

        AimAndRenderLaserPointer();
    }

    private void Fire()
    {
        //Get mouse position on mouse release
        mousePositionOnBtnUp = Input.mousePosition;
        mousePositionOnBtnUp.z = 400.0f;// mainCamera.nearClipPlane;
        //Convert mouse position to world coords and set depth to 0.0
        mousePositionOnBtnUpWorld = mainCamera.ScreenToWorldPoint(mousePositionOnBtnUp);// + defaultDepth);
        //mousePositionOnBtnUpWorld.z = 0.0f;
        mousePositionOnBtnUpWorld.y -= this.transform.position.y;

        //Fire projectile using converted coordinates as a direction vector 
        Projectile newProjectile = Instantiate(projectile);
        newProjectile.SetUpProjectile();
        newProjectile.SetSelfDestructTime(10.0f);
        newProjectile.FireProjectile(projectileThrust, mousePositionOnBtnUpWorld);
        aM.Play("projectile");

        //switch off laser
        laser.laserState = false;

    }

    private void AimAndRenderLaserPointer()
    {
        //Aim and Render LaserPointer GO 
        // Y position value has to be adjusted for laser pointer

        //switch on laser;
        laser.laserState = true;
        currentMousePositionWorld.y -= this.transform.position.y;
        laser.AimLaser(currentMousePositionWorld);
    }
}
