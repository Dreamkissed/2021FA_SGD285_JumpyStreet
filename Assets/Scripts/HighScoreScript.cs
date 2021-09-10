using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HighScoreScript : MonoBehaviour
{
    string scorelog = @"Assets\scorelog.txt"; //The file path of the scorelog text file
    List<string> scorelist = new List<string>(); //A list that holds the data from the scorelog file
    string newscore; //The new score to be added


    void AddHighScore()
    {
        //Reads the text file to an array, and sets it to the list
        string[] scorearray = File.ReadAllLines(scorelog);
        scorelist.Clear();
        for (int i = 0; i <= scorearray.Length - 1; i++)
        {
            scorelist.Add(scorearray[i]);
        }

        //Writes the new score to the list
        scorelist.Add(newscore);

        //Sorts the list
        scorelist.Sort();

        //Writes the list to the text file
        StreamWriter sw = new StreamWriter(scorelog);
        foreach (string s in scorelist)
        {
            sw.WriteLine(s);
        }
        sw.Close();
    }

    void ReadHighScores()
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
