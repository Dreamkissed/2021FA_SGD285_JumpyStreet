//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/13/2021
/////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using System.Collections; //New

public class PlayerController : MonoBehaviour
{
    private const int Direction_Forward = 1;
    private const int Direction_Left = 2;
    private const int Direction_Right = 3;
    private const int Direction_Back = 4;

    [SerializeField] private GameObject UIGameObject;

    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject PlayerModel; //New
    [SerializeField] private GameObject TerrainController;
    [SerializeField] private GameObject ScoreText;
    [SerializeField] private int player_MaxBackstepCount = 2;
    private string text_ScorePrefix = "Score: ";
    private int player_CurrentBackstepCount;
    private int player_ScoreCount;
    private float map_MaxEdgePos;
    private byte player_passMap;
    private byte player_killMap;
    private bool player_IsMoving = false;

    private bool game_IsPaused = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (!game_IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MovePlayer(Direction_Forward);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MovePlayer(Direction_Back);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MovePlayer(Direction_Left);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MovePlayer(Direction_Right);
            }
        }
    }

    private bool Setup()
    {
        map_MaxEdgePos = TerrainController.GetComponent<TerrainGeneration>().GetMapHalfWidth * TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
        player_ScoreCount = 0;
        UpdateScoreDisplay(player_ScoreCount);

        return true;
    }

    private bool MovePlayer(int direction)
    {
        if (!player_IsMoving)
        {
            player_IsMoving = true; 
            switch (direction)
            {
                case Direction_Forward:
                    MovePlayerForward();
                    break;
                case Direction_Left:
                    MovePlayerLeft();
                    break;
                case Direction_Right:
                    MovePlayerRight();
                    break;
                case Direction_Back:
                    MovePlayerBackwards();
                    break;
                default:
                    return false;
            }
            PlayerCleanFloatErrors();
            StartCoroutine(AlignPlayerModel());
        }

        return true;
    }

    private bool MovePlayerForward()
    {
        if (player_CurrentBackstepCount > 0 && CanPlayerMove(Direction_Forward))
        {
            PlayerLethalMove(Direction_Forward);
            player_CurrentBackstepCount--;
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.x += TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        else if (CanPlayerMove(Direction_Forward))
        {
            PlayerLethalMove(Direction_Forward);
            player_ScoreCount++;
            UpdateScoreDisplay(player_ScoreCount);
            TerrainController.GetComponent<TerrainGeneration>().Map_MoveForward(player_CurrentBackstepCount);
        }

        return true;
    }

    private bool MovePlayerBackwards()
    {
        if (player_CurrentBackstepCount < 2 && CanPlayerMove(Direction_Back))
        {
            player_CurrentBackstepCount++;

            if (player_CurrentBackstepCount <= player_MaxBackstepCount)
            {
                PlayerLethalMove(Direction_Back);
                Vector3 tempPlayerPos = PlayerObject.transform.position;
                tempPlayerPos.x -= TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
                PlayerObject.transform.position = tempPlayerPos;

                TerrainController.GetComponent<TerrainGeneration>().Map_MoveBackwards(player_CurrentBackstepCount);
            }
        }
        else if (player_CurrentBackstepCount >= 2)
        {
            PlayerKill();
        }

        return true;
    }

    private bool MovePlayerLeft()
    {
        if (PlayerObject.transform.position.z < map_MaxEdgePos && CanPlayerMove(Direction_Left))
        {
            PlayerLethalMove(Direction_Left);
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.z += TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        return true;
    }

    private bool MovePlayerRight()
    {
        if (PlayerObject.transform.position.z > -map_MaxEdgePos && CanPlayerMove(Direction_Right))
        {
            PlayerLethalMove(Direction_Right); 
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.z -= TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        return true;
    }

    private bool CanPlayerMove(int direction)
    {
        player_passMap = TerrainController.GetComponent<TerrainGeneration>().Map_PlayerPassMap(this.transform.position);

        switch (direction)
        {
            case Direction_Forward:
                if ((player_passMap & 0b01000000) == 0b01000000)
                {
                    return true;
                }
                return false;
            case Direction_Left:
                if ((player_passMap & 0b00010000) == 0b00010000)
                {
                    return true;
                }
                return false;
            case Direction_Right:
                if ((player_passMap & 0b00001000) == 0b00001000)
                {
                    return true;
                }
                return false;
            case Direction_Back:
                if ((player_passMap & 0b00000010) == 0b00000010)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool PlayerLethalMove(int direction)
    {
        player_killMap = TerrainController.GetComponent<TerrainGeneration>().Map_PlayerKillMap(this.transform.position);
        
        switch (direction)
        {
            case Direction_Forward:
                if ((player_killMap & 0b01000000) == 0b01000000)
                {
                    PlayerKill();
                    return true;
                }
                return false;
            case Direction_Left:
                if ((player_killMap & 0b00010000) == 0b00010000)
                {
                    PlayerKill();
                    return true;
                }
                return false;
            case Direction_Right:
                if ((player_killMap & 0b00001000) == 0b00001000)
                {
                    PlayerKill();
                    return true;
                }
                return false;
            case Direction_Back:
                if ((player_killMap & 0b00000010) == 0b00000010)
                {
                    PlayerKill();
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    private bool PlayerCleanFloatErrors()  //I REALLY HATE FLOATS!!!
    {
        Vector3 temp_PlayerPos = PlayerObject.transform.position;
        temp_PlayerPos.x = Mathf.Round(temp_PlayerPos.x);  //I hate floats...
        temp_PlayerPos.y = Mathf.Round(temp_PlayerPos.y);
        temp_PlayerPos.z = Mathf.Round(temp_PlayerPos.z);

        PlayerObject.transform.position = temp_PlayerPos;
        return true;
    }

    private bool PlayerKill()
    {
        Debug.Log("PLAYER DIE!!!!");
        UpdateScoreDisplay(-666);
        UIGameObject.GetComponent<UIHandler>().PlayerDies();

        return true;
    }

    private bool UpdateScoreDisplay(int score)
    {
        ScoreText.GetComponent<Text>().text = text_ScorePrefix + score.ToString();

        return true;
    }

    public bool SetGamePause(bool isPaused)
    {
        game_IsPaused = isPaused;
        return game_IsPaused;
    }

    public int GetPlayerScore => player_ScoreCount;

    IEnumerator AlignPlayerModel()
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            PlayerModel.transform.position = Vector3.Lerp(PlayerModel.transform.position, PlayerObject.transform.position, timeSinceStarted);
            if (PlayerModel.transform.position == PlayerObject.transform.position)
            {
                player_IsMoving = false;
                yield break;
            }
            yield return null;
        }
    }
}