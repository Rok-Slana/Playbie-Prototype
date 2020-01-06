using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedTarget;

    public bool isIndestuctible = false;
    public bool isNotBouncy = false;
    public int hits = 1;
    public bool isMovingTarget = false;
    public float movementDuration = 2.0f;
    public Vector3 startingPoint;
    public Vector3 endingPoint;

    private Vector3 pointHolder;
    private float time;
    private float timeT;
    private Renderer rend;
    private Material mat;
    private GameManager gameManager;
    private GameObject newDT;
    private DestroyedObject[] dtParts;


    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timeT = 0;
        //destroyedTarget = GameObject.FindGameObjectWithTag("TargetDestroyed");

        if (isNotBouncy)
        {
            Collider collider = gameObject.GetComponent<Collider>();
            collider.material.bounciness = 0.0f;
        }
    }

    void Update()
    {
        if (isMovingTarget)
        {
            MoveTarget();
        }
    }

    //Move GO between two designated postions
    private void MoveTarget()
    {
        timeT += Time.deltaTime;
        time = timeT/ movementDuration;
        transform.position = new Vector3(Mathf.SmoothStep(startingPoint.x, endingPoint.x, time), Mathf.SmoothStep(startingPoint.y, endingPoint.y, time), 0);
        if (timeT >= movementDuration)
        {
            SwapPosition();   
        }

    }

    //Helper method for moving GO between two positions
    private void SwapPosition()
    {
        timeT = 0;
        pointHolder = startingPoint;
        startingPoint = endingPoint;
        endingPoint = pointHolder;
    }
    
    //Do stuff when GO gets hit
    private void OnCollisionEnter(Collision collision)
    {
        if (!isIndestuctible && collision.gameObject.tag == "Projectile")
        {
            hits--;
            if (hits <= 0)
            {
                gameManager.SubtractTarget();

                GameObject newDestroyedTarget = Instantiate(destroyedTarget, gameObject.transform.position, Quaternion.identity) as GameObject;
                newDestroyedTarget.transform.localScale = this.transform.lossyScale;
                dtParts = newDestroyedTarget.GetComponentsInChildren<DestroyedObject>();

                foreach (DestroyedObject part in dtParts)
                {
                    part.SetVisibility(false);
                    part.gameObject.AddComponent<Rigidbody>();
                }

                Destroy(gameObject);
            }
        }
    }
}
