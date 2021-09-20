//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/13/2021
/////////////////////////////////////////////

using UnityEngine;
using System.Collections; //New

public class PlayerController : MonoBehaviour
{
    private const int Direction_Forward = 1;
    private const int Direction_Left = 2;
    private const int Direction_Right = 3;
    private const int Direction_Back = 4;

    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject PlayerModel; //New
    [SerializeField] private GameObject TerrainController;
    [SerializeField] private int player_MaxBackstepCount = 2;
    private int player_CurrentBackstepCount;
    private int player_ScoreCount;
    private float map_MaxEdgePos;
    private byte player_tileMap;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MovePlayerForward();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePlayerBackwards();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePlayerLeft();
            StartCoroutine(AlignPlayerModel()); //New
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayerRight();
            StartCoroutine(AlignPlayerModel()); //New
        }
    }

    private bool Setup()
    {
        map_MaxEdgePos = TerrainController.GetComponent<TerrainGeneration>().GetMapHalfWidth * TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
        
        return true;
    }

    private bool MovePlayerForward()
    {
        if (player_CurrentBackstepCount > 0 && CanPlayerMove(Direction_Forward))
        {
            player_CurrentBackstepCount--;
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.x += TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        else
        {
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
                Vector3 tempPlayerPos = PlayerObject.transform.position;
                tempPlayerPos.x -= TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
                PlayerObject.transform.position = tempPlayerPos;

                TerrainController.GetComponent<TerrainGeneration>().Map_MoveBackwards(player_CurrentBackstepCount);
            }
        }

        return true;    
    }

    private bool MovePlayerLeft()
    {
        if (PlayerObject.transform.position.z < map_MaxEdgePos && CanPlayerMove(Direction_Left))
        {
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
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.z -= TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        return true;
    }

    private bool CanPlayerMove(int direction)
    {
        player_tileMap = TerrainController.GetComponent<TerrainGeneration>().Map_PlayerTileMap(this.transform.position);

        switch (direction)
        {
            case Direction_Forward:
                if ((player_tileMap & 0b01000000) == 0b01000000)
                {
                    return true;
                }
                return false;
            case Direction_Left:
                if ((player_tileMap & 0b00001000) == 0b00001000)
                {
                    return true;
                }
                return false;
            case Direction_Right:
                if ((player_tileMap & 0b00010000) == 0b00010000)
                {
                    return true;
                }
                return false;
            case Direction_Back:
                if ((player_tileMap & 0b00000010) == 0b00000010)
                {
                    return true;
                }
                return false;
            default:
                return false;
        }

    IEnumerator AlignPlayerModel() //New
    {
        float timeSinceStarted = 0f;
        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            PlayerModel.transform.position = Vector3.Lerp(PlayerModel.transform.position, PlayerObject.transform.position, timeSinceStarted);
            if (PlayerModel.transform.position == PlayerObject.transform.position)
            {
                yield break;
            }
            yield return null;
        }
    }
}