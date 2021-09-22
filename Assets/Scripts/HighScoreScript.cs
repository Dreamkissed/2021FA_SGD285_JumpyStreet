using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class HighScoreScript : MonoBehaviour
{
    private string scorelog = @"Assets\scorelog.txt"; //The file path of the scorelog text file
    private List<string> scorelist = new List<string>(); //A list that holds the data from the scorelog file
    private string newentry; //The new entry to be added
    public InputField nameInput; //An inputbox for the player's name
    private string playername = ""; //The name of the player
    public string playerscore; //The score of the player


    public void AddHighScore()
    {
        //Reads the text file to an array, and sets it to the list
        string[] scorearray = File.ReadAllLines(scorelog);
        scorelist.Clear();
        for (int i = 0; i <= scorearray.Length - 1; i++)
        {
            scorelist.Add(scorearray[i]);
        }

        //Writes the new score to the list
        if (playername == "")
        {
            nameInput.gameObject.SetActive(true);
        }
        else
        {
            nameInput.gameObject.SetActive(false);
        }
        newentry = playername + " " + playerscore;
        scorelist.Add(newentry);

        //Sorts the list
        //scorelist.Sort();

        //Writes the list to the text file
        StreamWriter sw = new StreamWriter(scorelog);
        foreach (string s in scorelist)
        {
            sw.WriteLine(s);
        }
        sw.Close();
    }

    public void ReadHighScores()
    {
        //Reads the text file to an array, and sets it to the list
        string[] scorearray = File.ReadAllLines(scorelog);
        scorelist.Clear();
        for (int i = 0; i <=scorearray.Length-1; i++)
        {
            scorelist.Add(scorearray[i]);
        }
    }
}
