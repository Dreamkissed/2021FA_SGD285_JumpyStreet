using System;
using UnityEngine;
using System.IO;

public class HighScoreScript : MonoBehaviour
{
    private readonly string scorelog = @"Assets\scorelog.txt"; //The file path of the scorelog text file
    private const string FileDivider = ",";
    private bool arraysBuilt = false;

    //private List<string> scorelist = new List<string>(); //A list that holds the data from the scorelog file
    [SerializeField] private string[] scoreListNames;  //Need to be able to sort names by score and then add by score, etc
    [SerializeField] private int[] scoreListScores;
    
    private string newScoreName; //The new player to be added
    private int newScoreValue;
    //public InputField nameInput; //An inputbox for the player's name  //Rewriting AddHighScore() to take name/score as params
    private string tempScoreName = "";
    private int tempScoreValue = 0;
    //private string playername = ""; //The name of the player
    //public string playerscore; //The score of the player

    private bool SortScores()  //quicksort on the arrays and writes at end
    {
        bool isSorted = false;

        if (arraysBuilt)
        {
            while (!isSorted)
            {
                isSorted = true;
                for (int i = scoreListScores.Length - 1; i > 0; i--)
                {
                    if (scoreListScores[i - 1] < scoreListScores[i])
                    {
                        int tempI = scoreListScores[i - 1];
                        string tempS = scoreListNames[i - 1];

                        scoreListScores[i - 1] = scoreListScores[i];
                        scoreListNames[i - 1] = scoreListNames[i];

                        scoreListScores[i] = tempI;
                        scoreListNames[i] = tempS;

                        isSorted = false;
                    }
                }
            }
            WriteHighScoresArray();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool BuildHighScoreArrays()
    {
        if (!arraysBuilt)
        {
            string[] tempScoreFileRead = File.ReadAllLines(scorelog);
            scoreListNames = new string[tempScoreFileRead.Length];
            scoreListScores = new int[tempScoreFileRead.Length];

            for (int i = 0; i < tempScoreFileRead.Length; i++)
            {
                int tempScoreCutPoint = tempScoreFileRead[i].IndexOf(FileDivider);
                scoreListNames[i] = tempScoreFileRead[i].Substring(0, tempScoreCutPoint);
                scoreListScores[i] = Convert.ToInt32(tempScoreFileRead[i].Substring(tempScoreCutPoint + 1));
            }

            arraysBuilt = true;
            SortScores();
            return true;
        }
        else
        {
            return false;
        }
    }

    public string ReturnScoreForDisplay()
    {
        if (!arraysBuilt)
        {
            BuildHighScoreArrays();
        }
        //else
        {
            //return "Score Array Not Built";
        }

        string tempOutputString = "   Player name --- Score\n";
        for (int i = 0; i < scoreListNames.Length; i++)
        {
            string tempRowBuilding = (i + 1).ToString() + ": " + scoreListNames[i] + "---" + scoreListScores[i].ToString() + "\n";
            tempOutputString += tempRowBuilding;
        }
        return tempOutputString;
    }

    public void AddHighScore(string player, int score)
    {
        if (!arraysBuilt)
        {
            BuildHighScoreArrays();
        }

        int newArrayLength = scoreListNames.Length + 1;
        string[] tempArrayNames = new string[newArrayLength];
        int[] tempArrayScores = new int[newArrayLength];

        for (int i = 0; i < scoreListScores.Length; i++)
        {
            tempArrayNames[i] = scoreListNames[i];
            tempArrayScores[i] = scoreListScores[i];
        }

        tempArrayNames[scoreListNames.Length] = player;
        tempArrayScores[scoreListScores.Length] = score;

        scoreListNames = tempArrayNames;
        scoreListScores = tempArrayScores;

        SortScores();
    }

    private bool WriteHighScoresArray()  //Writes the list to the text file
    {
        //SortScores();

        StreamWriter sw = new StreamWriter(scorelog);
        for (int i = 0; i < scoreListNames.Length; i++)
        {
            string tempString = scoreListNames[i] + FileDivider + scoreListScores[i].ToString();
            sw.WriteLine(tempString);
        }
        sw.Close();

        return true;
    }

    /* DNI!  Not smart enough for Quicksort today
    //https://www.w3resource.com/csharp-exercises/searching-and-sorting-algorithm/searching-and-sorting-algorithm-exercise-9.php

    private void Quick_Sort(int left, int right)
    {
        Debug.Log("Quicksorting");
        if (left < right)
        {
            int pivotI = Partition(left, right);
            if (pivotI > 1)
            {
                Quick_Sort(left, pivotI - 1);
            }
            if (pivotI + 1 < right)
            {
                Quick_Sort(pivotI + 1, right);
            }
        }
    }

    private int Partition(int left, int right)
    {
        Debug.Log("Partitioning"); 
        int pivotI = scoreListScores[left];
        //string pivotS = scoreListNames[left];
        while (true)
        {
            while (scoreListScores[left] < pivotI)
            {
                left++;
            }
            while (scoreListScores[right] > pivotI)
            {
                right--;
            }
            if (left < right)
            {
                if (scoreListScores[left] == scoreListScores[right]) return right;
                
                int tempI = scoreListScores[left];
                scoreListScores[left] = scoreListScores[right];
                scoreListScores[right] = tempI;

                string tempS = scoreListNames[left];
                scoreListNames[left] = scoreListNames[right];
                scoreListNames[right] = tempS;
            }
            else
            {
                return right;
            }
        }
    }
    */
}