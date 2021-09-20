//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/08/2021
/////////////////////////////////////////////

using System.Collections;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private GameObject pfb_BlankTile;
    [SerializeField] private GameObject pfb_GrassTile;
    [SerializeField] private GameObject pfb_GrassFence;
    [SerializeField] private GameObject pfb_GrassTree;
    [SerializeField] private GameObject pfb_RoadTile;
    [SerializeField] private GameObject pfb_WaterTile;
    [SerializeField] private GameObject pfb_WaterLog;
    [SerializeField] private int biomeCount = 3;
    /* These two are magic number arrays, because for this project's scale, it
     * is easier and more time effective to just bruteforce the random tile
     * selection than mess with \resources, AddressableAssets, or other plug-in
     */
    private GameObject[] GrassArray;
    private GameObject[] WaterArray;

    [SerializeField] private int map_lengthForward = 7;  //tiles ahead of player
    [SerializeField] private int map_lengthBack = 3;  //tiles behind player
    [SerializeField] private int map_widthHalf = 5;  //tiles on either side of middle column.
    private GameObject[,] map_arrayTiles;  //set up in MapGeneration();
    private int map_lengthTotal, map_widthTotal;  //set up in MapGeneration();
    private int map_backRow = 0;
    private readonly float map_tileInterval = 5.0f;  //edge length of square tiles
    private bool map_isMoving = false;

    private void Awake()
    {
        GrassArray = new GameObject[] { pfb_GrassTile, pfb_GrassFence, pfb_GrassTree };  //!!!MN!!!
        WaterArray = new GameObject[] { pfb_WaterTile, pfb_WaterLog };  //!!!MN!!!

        MapGeneration();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
        }
    }

    private bool MapGeneration()
    {
        map_lengthTotal = map_lengthForward + 1 + map_lengthBack;
        map_widthTotal = map_widthHalf * 2 + 1;
        map_arrayTiles = new GameObject[map_lengthTotal, map_widthTotal];

        for (int l = 0; l < map_lengthTotal; l++)
        {
            //GameObject rowBiome = BiomeRandomBiomeRow();
            int rowBiome = Random.Range(0, biomeCount);

            for (int w = 0; w < map_widthTotal; w++)
            {
                float temp_tilePosX = ((float)l - (float)map_lengthBack) * map_tileInterval;
                float temp_tilePosY = 0.0f;  //playfield base level
                float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
                Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

                map_arrayTiles[l, w] = Instantiate(BiomeRandomBiomeRow(rowBiome), temp_tilePos, Quaternion.identity);
            }
        }

        return true;
    }

    private bool MapNewRowFront()
    {
        //GameObject rowBiome = BiomeRandomBiomeRow();
        int rowBiome = Random.Range(0, biomeCount);

        for (int w = 0; w < map_widthTotal; w++)
        {
            Destroy(map_arrayTiles[map_backRow, w]);
            
            float temp_tilePosX = (float)map_lengthForward * map_tileInterval;
            float temp_tilePosY = 0.0f;  //playfield base level
            float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
            Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

            map_arrayTiles[map_backRow, w] = Instantiate(BiomeRandomBiomeRow(rowBiome), temp_tilePos, Quaternion.identity);
            /*
            tempPositionVector = map_arrayTiles[map_backRow, w].transform.position;
            tempPositionVector.x = map_lengthForward * map_tileInterval;
            map_arrayTiles[map_backRow, w].transform.position = tempPositionVector;
            */
        }
        map_backRow++;
        map_backRow %= map_lengthTotal;

        return true;
    }

    private GameObject BiomeRandomBiomeRow()
    {
        int randomBiome = Random.Range(0, biomeCount);
        return randomBiome switch
        {
            0 => pfb_GrassTile,
            1 => pfb_RoadTile,
            2 => pfb_WaterTile,
            _ => pfb_BlankTile,
        };
    }

    private GameObject BiomeRandomBiomeRow(int biome)
    {
        return biome switch
        {
            0 => GrassArray[Random.Range(0, GrassArray.Length)],
            1 => pfb_RoadTile,
            2 => WaterArray[Random.Range(0, WaterArray.Length)],
            _ => pfb_BlankTile,
        };
    }

    private bool MapCleanFloatErrors()  //I REALLY HATE FLOATS!!!
    {
        for (int l = 0; l < map_lengthTotal; l++)
        {
            for (int w = 0; w < map_widthTotal; w++)
            {
                Vector3 temp_tilePos = map_arrayTiles[l, w].transform.position;
                temp_tilePos.x = Mathf.Round(temp_tilePos.x);  //I hate floats...
                temp_tilePos.y = Mathf.Round(temp_tilePos.y);
                temp_tilePos.z = Mathf.Round(temp_tilePos.z);
                
                map_arrayTiles[l, w].transform.position = temp_tilePos;
            }
        }
        return true;
    }

    private IEnumerator TreadmillForward()
    {
        float intervalStep = 0.1f; //!!!!!
        
        for (float f = 0.0f; f <= map_tileInterval; f += intervalStep)
        {
            for (int l = 0; l < map_lengthTotal; l++)
            {
                for (int w = 0; w < map_widthTotal; w++)
                {
                    Vector3 temp_tilePos = map_arrayTiles[l, w].transform.position;
                    temp_tilePos.x -= intervalStep;
                    map_arrayTiles[l, w].transform.position = temp_tilePos;
                }
            }
            yield return null;
        }
        MapCleanFloatErrors();
        MapNewRowFront();
        map_isMoving = false;
    }

    private IEnumerator TreadmillBackward()
    {
        float intervalStep = 0.1f; //!!!!!

        for (float f = 0.0f; f <= map_tileInterval; f += intervalStep)
        {
            for (int l = 0; l < map_lengthTotal; l++)
            {
                for (int w = 0; w < map_widthTotal; w++)
                {
                    Vector3 temp_tilePos = map_arrayTiles[l, w].transform.position;
                    temp_tilePos.x += intervalStep;
                    map_arrayTiles[l, w].transform.position = temp_tilePos;
                }
            }
            yield return null;
        }
        MapCleanFloatErrors();
        map_isMoving = true;
    }

    public bool Map_MoveForward(int backstepCount)
    {
        if (backstepCount <= 0)
        {
            if (!map_isMoving)
            {
                map_isMoving = true;
                StartCoroutine(TreadmillForward());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Map_MoveBackwards(int backstepCount)
    {
        return true;
    }

    public float GetMapTileInterval => map_tileInterval;
    public int GetMapHalfWidth => map_widthHalf;
    public int GetMapFullWidth => map_widthTotal;
}