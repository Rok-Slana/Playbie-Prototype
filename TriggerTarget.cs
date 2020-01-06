using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTarget : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyedTrigger;
    //[SerializeField]
    private AudioManager audioManager;

    public int triggerType;
    public int[] positionsArray;
    public Vector3[] letterPositions;
    public List<char> lettersTXT = new List<char>();

    private float emission = 0.025f;
    private float emissionMax = 4f;
    private bool triggered = false;
    private bool isOn = false;
    private Teacher teacher;
    private Renderer rend;
    private Material mat;
    private Color color;
    private GameManager gameManager;
    private GameObject[] targetsIndestructible;
    private GameObject[] bouncers;
    private List<Vector3> letterPositionsFinal = new List<Vector3>();    
    private DestroyedObject[] dtParts;    

    void Start()
    {
        //Check the type of this triggerTarget (possible values : 1, 2, 3, 4)
        if (triggerType <= 0 || triggerType > 4)
        {
            Debug.LogError("triggerType value missing on TriggerType Game Object or value out of bounds!");
        }
        //If triggerType is set correctly, do the following
        else
        {
            gameManager = FindObjectOfType<GameManager>();
            audioManager = FindObjectOfType<AudioManager>();
            //triggerTarget sets the correct number of Letters to be spawned later in game.
            //Number depends on triggerType value (possible values : 1, 2, 3, 4 which correspond to possible number of letters : 3, 4, 6, 8)
            if (positionsArray[triggerType - 1] != 3)
            {
                //Works for values 4, 6, 8 (triggerTypes 2, 3, 4)
                gameManager.SetLettersNum(positionsArray[triggerType - 1] - positionsArray[triggerType - 2]);
            }
            else
            {
                //Works for value 3 (triggerType 1)
                gameManager.SetLettersNum(positionsArray[triggerType - 1]);
            }

            //Find all indestrucible gameObjects
            targetsIndestructible = GameObject.FindGameObjectsWithTag("TargetIndestructible");
            //Find all bouncy gameObjects
            bouncers = GameObject.FindGameObjectsWithTag("Bouncer");
            //Find teacher gameObject
            teacher = FindObjectOfType<Teacher>();

            /////          !!!! This should go eventualy
            //Set color of TriggerTarget to given color with emission value of emission
            rend = GetComponent<Renderer>();
            mat = rend.material;
            color = mat.color * Mathf.LinearToGammaSpace(emission);
            mat.SetColor("_EmissionColor", color);

            //Get the appropriate positions from letterPositions[] depending on the type of the trigger

            // Set correct starting index for letters positionsArray
            int firstSlot = 0;
            if (triggerType == 1) ;                                 //Do not change starting index if triggerType is 1
            else firstSlot = positionsArray[triggerType - 2];       //Change starting index accordingly if triggerType is not 1
            
            //Prepare a list of Vector3 positions where Letters are going to be spawned later by Teacher GO
            for(int x = firstSlot; x < positionsArray[triggerType-1]; x++) {
                letterPositionsFinal.Add(letterPositions[x]);
            }
        }

        //Check for Teacher GO
        if (!teacher)
        {
            Debug.LogWarning("Teacher Game Object missing");
        }
        //Check for GameManager GO
        if (!gameManager)
        {
            Debug.LogWarning("Game Manager Game Object missing");
        }
    }

    void Update()
    {
        //Change GO color when this GO is triggered
        if (triggered)
        {
            ChangeColor();
        }
    }

    //Set triggerTarget switch
    public void TriggerTargetSwitch(bool arg)
    {
        triggered = arg;
    }

    //Change color of this GO
    public void ChangeColor()
    {
        isOn = true;
        if (emission < emissionMax)
        {
            emission += 0.11f;
            color = mat.color * Mathf.LinearToGammaSpace(emission);
            mat.SetColor("_EmissionColor", color);
        }
        else
        {
            triggered = false;            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {               
        if (isOn)
        {
            //Instantitate new destroyedObject, grab all the sub parts and set them accordingly (start fading, add rigidbodies for explosion)
            GameObject newDestroyedTrigger = Instantiate(destroyedTrigger, gameObject.transform.position, Quaternion.identity) as GameObject;
            newDestroyedTrigger.transform.localScale = this.transform.lossyScale;
            dtParts = newDestroyedTrigger.GetComponentsInChildren<DestroyedObject>();
            foreach (DestroyedObject part in dtParts)
            {
                part.SetVisibility(false);                      //Starts the fading proccess
                part.gameObject.AddComponent<Rigidbody>();      //Having RigidBody makes sure parts will explode
            }

            //Set learning material by sending the positions of the letters and letters characters themself to the teacher
            teacher.SetLearningMaterial(letterPositionsFinal, lettersTXT);

            //Deactivate all Indestructible GOs so they don't mess with letter to be spawned
            foreach (GameObject obstacle in targetsIndestructible)
            {
                obstacle.gameObject.SetActive(false);
            }
            //Deactivate all bouncy GOs so they don't mess with letter to be spawned
            foreach (GameObject bouncer in bouncers)
            {
                bouncer.gameObject.SetActive(false);
            }
            //Deactivate this GO
            this.gameObject.SetActive(false);
        }
        else
        {
            audioManager.Play("wrong_hit");
        }
    }
}
