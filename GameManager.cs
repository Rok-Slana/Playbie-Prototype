using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private int numberOfTargets = 0;
    public int NumberOfTargets
    {
        get
        {
            return numberOfTargets;
        }
        private set { }
    }

    private int numberOfLetters = 0;
    private int newLevel = 0;

    private TriggerTarget triggerTarget;

    private MyPanel fakePanel;

    private void Awake()
    {
        numberOfTargets = GameObject.FindGameObjectsWithTag("Target").Length;

    }
    // Start is called before the first frame update
    void Start()
    {
        //tutorialPointer.SetBool("showPointer", true);
        triggerTarget = FindObjectOfType<TriggerTarget>();
        //numberOfTargets = GameObject.FindGameObjectsWithTag("Target").Length;
        fakePanel = FindObjectOfType<MyPanel>();
        fakePanel.PanelSwitch(false);
    }

    //Subtract destroyed targets and switch ON Teacher
    public void SubtractTarget()
    {
        numberOfTargets--;
        //numberOfTargetsPublic = numberOfTargets;
        if(numberOfTargets <= 0)
        {
            triggerTarget.TriggerTargetSwitch(true);
        }
    }

    //Called by TiggerTarget's Start()
    public void SetLettersNum(int num)
    {
        numberOfLetters = num;
    }

    //Helper method for SubtractLetter()
    public void SubtractLetterPublic()
    {
        StartCoroutine(SubtractLetter());
    }

    //Subtract destroyed letters and load next level
    private IEnumerator SubtractLetter()
    {
        numberOfLetters--;
        if(numberOfLetters <= 0)
        {
            yield return new WaitForSeconds(1.8f);
            fakePanel.PanelSwitch(true);
            yield return new WaitForSeconds(2.0f);
            SceneManager.LoadScene(GetRandomLevel());            
        }
        yield return new WaitForSeconds(3f);
    }

    //Load First Tutorial Level
    public void PlayNewGame()
    {
        SceneManager.LoadScene(GetRandomLevel());
    }

    //Exit from current game to Main Menu
    public void MainMenuExit()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    //Quit Game
    public void QuitGame()
    {
        Application.Quit();
    }

    //Get a 'random' but still apropriate  next level. This method returns one of two possible levels for the next intended step-up
    public int GetRandomLevel()
    {

        /*
        //newLevel = Application.loadedLevel;
        newLevel = SceneManager.GetActiveScene().buildIndex;

        //Check for exception if game is still in MainMenu
        if (newLevel == 0)
        {
            newLevel = Random.Range(1, 3);
        }

        //If game is not in MainMenu anymore, check which one of the two possible levels it is
        //and offers the two corresponding/succeding levels depending on current scene 
        else
        {
            //Return for all the even scenes
            if(newLevel % 2 == 0)
            {
                Debug.Log("newLevel % 2 = "+ newLevel % 2);
                newLevel = Random.Range(newLevel + 1, newLevel + 3);
            }

            //Return for all the odd scenes
            else
            {
                Debug.Log("Else = " + newLevel % 2);
                newLevel = Random.Range(newLevel + 2, newLevel + 4);
            }

        }

        //If end of tutorial levels is reached, go back to MainMenu
        if (newLevel > 8)
        {
            MainMenuExit();
        }
        
        return newLevel;
        */
        newLevel = SceneManager.GetActiveScene().buildIndex+1;

        if (newLevel >= SceneManager.sceneCountInBuildSettings)
        {
            MainMenuExit();
        }

        return newLevel;
    }
}
