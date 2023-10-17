using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private PlayerTask tutorialTask;
    GameManager gameManager;
    public TMP_Text readyText; 
    public bool textShown;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        //disable the ready text
        textShown = false;
        readyText.gameObject.SetActive(false);  
    }

      //if collides with player, set game state to running.
    private void OnTriggerEnter(Collider other){
        if (other.gameObject.name == "Player"  && gameManager.gameState != GameState.Over)
        {
            if (!gameManager.GetTaskState(tutorialTask).isComplete)
            {
                Debug.Log("Tutorial Over");
                gameManager.CompleteTask(tutorialTask);

                // Start timer
                gameManager.timerEnabled = true;

                gameManager.gameState = GameState.Running;

                GetComponent<TutorialController>().DisplayTextForDuration(5.0f);
                textShown = true;
            }
        }
    }


    public void DisplayTextForDuration(float duration)
    {
        if (!textShown)
        {
            StartCoroutine(ShowTextForDuration(duration));
            
        }
        
    }

 private IEnumerator ShowTextForDuration(float duration)
    {
    
        readyText.gameObject.SetActive(true); // Show the text

        yield return new WaitForSeconds(duration); // Wait for the specified duration

        readyText.gameObject.SetActive(false); // Hide the text
    }

}


