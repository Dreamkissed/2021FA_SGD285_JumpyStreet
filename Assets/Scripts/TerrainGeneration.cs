//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/08/2021
/////////////////////////////////////////////

using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] GameObject tile;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            float tempTileX = tile.transform.position.x + 5.0f;
            Vector3 tempTilePos = new Vector3(tempTileX, tile.transform.position.y, tile.transform.position.z);
            tile.transform.position = tempTilePos;
        }
    }
}
