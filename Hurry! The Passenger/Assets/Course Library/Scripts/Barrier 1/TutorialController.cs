using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public GameManager gameManager;
     public TMP_Text readyText; 
     public bool textShown;
    // Start is called before the first frame update
    void Start()
    {
        //disable the ready text
        textShown = false;
        readyText = transform.Find("Environment/TutReady").GetComponent<TMP_Text>();
        readyText.gameObject.SetActive(false);  
          }

    // Update is called once per frame
    void Update()
    {
      
    }

      //if collides with player, set game state to running.
    private void OnTriggerEnter(Collider other){
        Debug.Log("yaaaaaaaaaaaaaaaaaaaaay " + other.gameObject.name);
        if (other.gameObject.name == "Player"  && gameManager.gameState != GameState.Over)
        {
            // Start countdown function
            StartCoroutine(gameManager.TimeRemain()); // set up time countdown

            gameManager.gameState = GameState.Running;
            //print("collided");
            Debug.Log("collided");

        GetComponent<TutorialController>().DisplayTextForDuration(5.0f);
        textShown = true;
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


