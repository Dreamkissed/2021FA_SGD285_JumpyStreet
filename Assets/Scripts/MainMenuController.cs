using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenuController : MonoBehaviour
{
    public GameObject highscorePanel;
    public Text highscoreText;
    public Scene GameScene;

    string scorelog = @"Assets\scorelog.txt"; //The file path of the scorelog text file
    List<string> scorelist = new List<string>(); //A list that holds the data from the scorelog file

    public void OnPlayButtonClick()
    {
        //Loads the gamescene
        SceneManager.LoadScene(GameScene.ToString());
    }

    public void OnHighScoreButtonClick()
    {
        //Sets the highscorepanel to active
        highscorePanel.SetActive(true);
        //Reads the text file to an array, and sets it to the list
        string[] scorearray = File.ReadAllLines(scorelog);
        scorelist.Clear();
        for (int i = 0; i <= scorearray.Length - 1; i++)
        {
            scorelist.Add(scorearray[i]);
        }
        //Writes the list to the textbox
        foreach (string s in scorelist)
        {
            highscoreText.text += s + "\n";
        }

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
    }

}
