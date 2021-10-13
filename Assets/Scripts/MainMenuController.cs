using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject highscorePanel;
    [SerializeField] private Text highscoreText;
    [SerializeField] private GameObject mainmenuPanel;

    private HighScoreScript scoreScript;

    private void Awake()
    {
        scoreScript = this.gameObject.GetComponent<HighScoreScript>();
        highscorePanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }

    public void OnPlayButtonClick()
    {
        //Loads the gamescene
        SceneManager.LoadScene(1);
    }

    public void OnHighScoreButtonClick()
    {
        //Sets the highscorepanel to active
        highscorePanel.SetActive(true);
        mainmenuPanel.SetActive(false);

        highscoreText.text = scoreScript.ReturnScoreForDisplay();        
    }

    public void OnQuitButtonClick()
    {
        //Quits the game
        Application.Quit();
    }

    public void OnCloseButtonClick()
    {
        //Sets the highscorepanel to inactive
        highscorePanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }
}
