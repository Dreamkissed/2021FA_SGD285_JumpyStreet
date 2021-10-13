//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 10/13/2021
/////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

public class ScoreMenuController : MonoBehaviour
{
    [SerializeField] private Text scoreDisplay;
    [SerializeField] private InputField inputName;
    private HighScoreScript scoreScript;

    private string playerName = "";
    private int playerScore = 0;

    private void Awake()
    {
        scoreScript = this.gameObject.GetComponent<HighScoreScript>();
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

        //disable buttons after entering so no padding scores.
    }
}
