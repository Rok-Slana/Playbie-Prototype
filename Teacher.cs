using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teacher : MonoBehaviour
{
    public float velocity = 100.0f;
    [SerializeField]
    private Letter letter;
    [SerializeField]
    private TextMesh letterTXT;
    [SerializeField]
    private AudioManager audioManager;


    private TriggerTarget triggerTarget;
    private TextMesh newTM;
    private bool learningPhase = false;
    private bool teacherSwitch = false;
    private List<Vector3> letterPositions = new List<Vector3>();
    private List<Letter> letters = new List<Letter>();
    private List<char> lettersTXT = new List<char>();

    void Start()
    {
        triggerTarget = FindObjectOfType<TriggerTarget>();                              //Find triggerTarget
        this.transform.position = triggerTarget.transform.position;                     //Set position of this GO same as position of triggerTarget
        this.transform.localScale = triggerTarget.transform.lossyScale;
        audioManager = FindObjectOfType<AudioManager>();                                //Get AudioManager (unable to do it via prefabs)
        if(!letter || !letterTXT) Debug.LogWarning("Letter or TextMesh missing!");      //Check for Letter and TextMeshLetter
    }

    private void Update()
    {
        if (teacherSwitch)
        {
            GiveLetter();
        }
    }

    public void LearningPhaseSwitch(bool state)
    {
        learningPhase = state;
    }

    //Instantiate and arrange letters as provided by TriggerTarget
    public void SetLearningMaterial(List<Vector3> letterPos, List<char> charList)
    {
        foreach (Vector3 pos in letterPos)
        {            
            Letter newLetter = Instantiate(letter) as Letter;               //Instantiate new letter
            newLetter.transform.localScale = this.transform.lossyScale; 
            newLetter.transform.position = this.transform.position;         //Asign current position of this GO to the new letter
            newLetter.AssignFinalPosition(pos);                             //Assign the final position/destination of the letter
            newLetter.SetGameManager(letter.GetGameManager());              //Assign gameManager reference of the newly created letter
            newLetter.MoveLetterSwitch(true);                               //Turn switch on the letter so it starts moving towards its final position
            TextMesh newLetterTXT = Instantiate(letterTXT);                 //Instantiate and assign a new letterTXT to the newLetter GO 
            newLetter.AssignLetterTextMesh(newLetterTXT);
            letters.Add(newLetter);
        }
        
        CharAlgo(charList);                                                 //Send provided chars to get a complete and shuffeled list

        for (int i = 0; i < lettersTXT.Count; i++)
        {
            letters[i].transform.GetChild(0).GetComponent<TextMesh>().text = "" + lettersTXT[i];
        }                       //Get all the TextMesh-es and assign their text/char values

        learningPhase = true;                                               //Learning phase ON (in addition to teacherSwitch) serves as a marker for Letters to help the learning game flow
        teacherSwitch = true;                                               //Bring on the teacher     
    }

    //CharAlgo recieves trimmed list of chars used for the current level and expands them to full list
    //It feeds the full list to the Shuffling algorithm below
    public void CharAlgo(List <char> charList)
    {
        lettersTXT = charList;
        List<char> newCharList = new List<char>();

        //Do stuff depending on the charList length
        if (lettersTXT.Count == 1)
        {
            newCharList = new List<char> { charList[0], charList[0], charList[0]};
            lettersTXT = newCharList;
            return;
        }
        else if (lettersTXT.Count == 2)
        {
            newCharList = new List<char> { charList[0], charList[1], charList[0], charList[1]};
        }
        else if (lettersTXT.Count == 3)
        {
            newCharList = new List<char> { charList[0], charList[0], charList[1], charList[1], charList[2], charList[2] };
        }
        else if (lettersTXT.Count == 4)
        {
            newCharList = new List<char> { charList[0], charList[0], charList[1], charList[1], charList[2], charList[2], charList[3], charList[3] };
        }
        
        lettersTXT = ShuffleChars(newCharList);        
    }

    //Method recieves full List of chars and shuffles & returns them accordingly
    public static List<char> ShuffleChars(List<char> aList)
    {
        //Shuffle method found @ answers.unity.com/questions/486626/how-can-i-shuffle-alist.html
        System.Random _random = new System.Random();

        char myChar;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1
            int r = i + (int)(_random.NextDouble() * (n - i));
            myChar = aList[r];
            aList[r] = aList[i];
            aList[i] = myChar;
        }

        return aList;
    }

    public void GiveLetter()
    {
        //Remove all items in letters list that have been destroyed so far
        letters.RemoveAll(item => item == null);
        //Pick a random letter from letters list for the player to shoot
        int len = letters.Count;
        if (len > 0)
        {
            int randIndex = Random.Range(0, len - 1);
            string str = letters[randIndex].GetComponentInChildren<TextMesh>().text;
            FindAndUnlockSameLetter(str);
            //Check if there is a specific letter to play, if not, skip this step (intended for tutorial level where there are no letters yet) 
            if(str != " ") StartCoroutine(audioManager.PlayTwoStrings("zadeni_crko", str));
            teacherSwitch = false;
        }
    }

    //HELPER METHOD : GiveLetter()
    public void FindAndUnlockSameLetter(string str)
    {
        for(int i = 0; i < letters.Count; i++)
        {
            string compareString = letters[i].GetComponentInChildren<TextMesh>().text;
            if (compareString == str)
            {
                letters[i].LockLetterSwitch(false);
            }
        }
    }  

    // GET SET METHODS
    public bool GetLearningPhase()
    {
        return learningPhase;
    }
    public void SetTeacherSwitch(bool state)
    {
        teacherSwitch = state;
    }
}
