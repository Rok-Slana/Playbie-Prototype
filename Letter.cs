using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public float velocity = 300.0f;

    private Vector3 singleLetterPosition;
    private bool moveLetter = false;
    private GameManager gameManager;
    private TextMesh letterTXT;
    private bool lockLetter = true;
    private Teacher teacher;
    private Letter[] otherLetters;

    public GameObject destroyedLetter;
    private DestroyedObject[] dlParts;
    private AudioManager aM;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        teacher = FindObjectOfType<Teacher>();
        aM = FindObjectOfType<AudioManager>();
    }

    public void Update()
    {
        //If switch is On, move the letter GO to the final position and turn switch OFF
        if (moveLetter)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, singleLetterPosition, velocity * Time.deltaTime);
            //Move the  asssigned letterTXT along (with adjusted depth)
            letterTXT.transform.position = Vector3.MoveTowards(this.transform.position, singleLetterPosition + new Vector3(0,0,-5), velocity * Time.deltaTime);
            if (this.transform.position == singleLetterPosition)
            {
                moveLetter = false;
            }
        }
    }

    //On Collision subtract one letter on gameManagerGO and destroy this GO
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            otherLetters = FindObjectsOfType<Letter>();

            if (!lockLetter && teacher.GetLearningPhase())
            {
                aM.Play("right_hit");
                //Lock all other letters
                foreach (Letter l in otherLetters)
                {
                    l.LockLetterSwitch(true);
                }

                teacher.SetTeacherSwitch(true);
                gameManager.SubtractLetterPublic();

                GameObject newDestroyedLetter = Instantiate(destroyedLetter, gameObject.transform.position, Quaternion.identity) as GameObject;
                newDestroyedLetter.transform.localScale = this.transform.lossyScale;
                dlParts = newDestroyedLetter.GetComponentsInChildren<DestroyedObject>();

                foreach (DestroyedObject part in dlParts)
                {
                    part.SetVisibility(false);
                    part.gameObject.AddComponent<Rigidbody>();
                }
                Destroy(letterTXT);
                Destroy(gameObject);
            }
            else
            {
                aM.Play("wrong_hit");
            }
        }
    }

    //Set final positon/destination of the letter GO
    public void AssignFinalPosition(Vector3 pos)
    {
        singleLetterPosition = pos;
    }

    //provide final position/destination of the letter GO
    public Vector3 GetAssignedPosition()
    {
        return singleLetterPosition;
    }

    //Switch for moving letters from triggerTarget position to own final position
    public void MoveLetterSwitch(bool state)
    {
        moveLetter = state;
    }

    //provide gameManager refference
    public GameManager GetGameManager()
    {
        return gameManager;
    }

    //Set refference to gameManager GO
    public void SetGameManager(GameManager gm)
    {
        gameManager = gm;
    }

    //Set proper position and parent to TextMesh
    public void AssignLetterTextMesh(TextMesh letter)
    {
        letterTXT = letter;
        letterTXT.transform.position = this.transform.position;
        letterTXT.transform.parent = this.transform;
    }

    //Lock or unlock letter and make it destructible
    public void LockLetterSwitch(bool state)
    {
        lockLetter = state;
    }

    public bool GetLockState()
    {
        return lockLetter;
    }
}
