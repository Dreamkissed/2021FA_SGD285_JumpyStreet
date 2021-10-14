//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 10/13/2021
/////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreMenuController : MonoBehaviour
{
    [SerializeField] private Text scoreDisplay;
    [SerializeField] private InputField inputName;
    [SerializeField] private Button inputButton;
    private HighScoreScript scoreScript;

    //private string playerName = "";
    private int playerScore = 0;

    private void Awake()
    {
        scoreScript = this.gameObject.GetComponent<HighScoreScript>();
        
        inputName.interactable = true;
        inputButton.interactable = true;
    }

    private void Start()
    {
        scoreDisplay.text = scoreScript.ReturnScoreForDisplay();
    }

    public void ConfirmScore()
    {
        if (inputName.text != "")
        {
            scoreScript.AddHighScore(inputName.text, playerScore);
            scoreDisplay.text = scoreScript.ReturnScoreForDisplay();
        }
        inputName.interactable = false;
        inputButton.interactable = false;

        //disable buttons after entering so no padding scores.
    }

    public void SetScore(int score)
    {
        playerScore = score;
    }

    public void QuitToMenu()
    {
        inputName.interactable = true;
        inputButton.interactable = true;

        SceneManager.LoadScene(0);
    }
}
