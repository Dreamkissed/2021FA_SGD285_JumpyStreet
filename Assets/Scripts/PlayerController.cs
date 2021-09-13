//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/13/2021
/////////////////////////////////////////////

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private GameObject TerrainController;
    [SerializeField] private int player_MaxBackstepCount = 2;
    private int player_CurrentBackstepCount;
    private float map_MaxEdgePos;

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
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePlayerRight();
        }
    }

    private bool Setup()
    {
        map_MaxEdgePos = TerrainController.GetComponent<TerrainGeneration>().GetMapHalfWidth * TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
        
        return true;
    }

    private bool MovePlayerForward()
    {
        if (player_CurrentBackstepCount > 0)
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
        if (player_CurrentBackstepCount < 2)
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
        if (PlayerObject.transform.position.z < map_MaxEdgePos)
        {
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.z += TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }
        
        return true;
    }

    private bool MovePlayerRight()
    {
        if (PlayerObject.transform.position.z > -map_MaxEdgePos)
        {
            Vector3 tempPlayerPos = PlayerObject.transform.position;
            tempPlayerPos.z -= TerrainController.GetComponent<TerrainGeneration>().GetMapTileInterval;
            PlayerObject.transform.position = tempPlayerPos;
        }

        return true;
    }
}