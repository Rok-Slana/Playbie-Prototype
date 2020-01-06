using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAnimations : MonoBehaviour
{
    [SerializeField]
    private float delayTime = 1.0f;
    public GameObject tutorialPointer;
    private RawImage pointerImage;
    private bool animatePointer = false;
    private int numOfTargets;
    private int numOfTargetsNew;
    private GameManager gM;

    private void Awake()
    {
        pointerImage = tutorialPointer.GetComponent<RawImage>();//.enabled = false;
        pointerImage.enabled = false;   
    }

    void Start()
    {
        StartCoroutine(ShowTutorialPointer(delayTime));
        gM = FindObjectOfType<GameManager>();
        numOfTargets = gM.NumberOfTargets;
        numOfTargetsNew = numOfTargets;
    }

    void Update()
    {
        numOfTargetsNew = gM.NumberOfTargets;
        if (numOfTargetsNew < numOfTargets)
        {
            this.gameObject.SetActive(false);
        }
        
    }

    private IEnumerator ShowTutorialPointer(float delay)
    {
        yield return new WaitForSeconds(delay);
        pointerImage.enabled = true;
    }
}
