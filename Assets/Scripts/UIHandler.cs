//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 10/13/2021
/////////////////////////////////////////////

using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private const KeyCode PauseKey = KeyCode.Escape;
    
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject scoreMenu;
    private GameObject playerObject;

    private bool isPaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        scoreMenu.SetActive(false);
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerObject.GetComponent<PlayerController>().SetGamePause(isPaused);
    }

    private void Update()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            Debug.Log("PauseKey Pressed");
            if (isPaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private bool PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        playerObject.GetComponent<PlayerController>().SetGamePause(isPaused);
        Time.timeScale = 0.0f;

        return true;
    }

    private bool UnpauseGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        playerObject.GetComponent<PlayerController>().SetGamePause(isPaused);
        Time.timeScale = 1.0f;

        return true;
    }

    public void ResumeGame()
    {
        UnpauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public bool PlayerDies()
    {
        PauseGame();
        this.gameObject.GetComponent<ScoreMenuController>().SetScore(playerObject.GetComponent<PlayerController>().GetPlayerScore);
        scoreMenu.SetActive(true);

        return true;
    }
}