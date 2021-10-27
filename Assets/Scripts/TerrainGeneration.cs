//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 09/08/2021
/////////////////////////////////////////////

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainGeneration : MonoBehaviour
{
    private const string TileTag_Kill = "Kill";
    private const string TileTag_Pass = "Pass";
    
    [SerializeField] private GameObject pfb_BlankTile;  //Placeholder tile prefabs
    [SerializeField] private GameObject pfb_GrassTile;
    [SerializeField] private GameObject pfb_GrassFence;
    [SerializeField] private GameObject pfb_GrassTree;
    [SerializeField] private GameObject pfb_RoadTile;
    [SerializeField] private GameObject pfb_WaterTile;
    [SerializeField] private GameObject pfb_WaterLog;
    [SerializeField] private int biomeCount = 3;
    [SerializeField] private GameObject creatureController;
    /* These two are magic number arrays, because for this project's scale, it
     * is easier and more time effective to just bruteforce the random tile
     * selection than mess with \resources, AddressableAssets, or other plug-in
     */

    //adding two(four) more for clear/blocked for generator
    private GameObject[] tileArray_GrassClear;
    private GameObject[] tileArray_GrassBlock;
    private GameObject[] tileArray_RoadClear;
    private GameObject[] tileArray_RoadBlock;
    private GameObject[] tileArray_WaterClear;
    private GameObject[] tileArray_WaterBlock;

    [SerializeField] private int map_lengthForward = 7;  //tiles ahead of player
    [SerializeField] private int map_lengthBack = 3;  //tiles behind player
    [SerializeField] private int map_widthHalf = 5;  //tiles on either side of middle column.
    private GameObject[,] map_arrayTiles;  //set up in MapGeneration();
    private int map_lengthTotal, map_widthTotal;  //set up in MapGeneration();
    private int map_backRow = 0;
    private int map_readFrame;
    private readonly float map_tileInterval = 5.0f;  //edge length of square tiles
    private bool map_isMoving = false;

    private byte player_tileMap;
    private bool[] map_RowPass, map_RowPassPrevious;

    private void Awake()
    {
        map_readFrame = map_widthHalf;

        tileArray_GrassClear = new GameObject[] { pfb_GrassTile};  //!!!MN!!!
        tileArray_GrassBlock = new GameObject[] { pfb_GrassFence, pfb_GrassTree };
        tileArray_RoadClear = new GameObject[] { pfb_RoadTile };
        tileArray_RoadBlock = new GameObject[] { };
        tileArray_WaterClear = new GameObject[] { pfb_WaterLog };
        tileArray_WaterBlock = new GameObject[] { pfb_WaterTile };

        MapGeneration();
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    private bool MapGeneration()
    {
        map_lengthTotal = map_lengthBack + 1 + map_lengthForward;  //Sets up map size
        map_widthTotal = (map_widthHalf * 2) + 1;
        map_arrayTiles = new GameObject[map_lengthTotal, map_widthTotal];
        map_RowPass = new bool[map_widthTotal];
        map_RowPassPrevious = new bool[map_widthTotal];

        for (int w = 0; w < map_RowPassPrevious.Length; w++)  //initializes previous row prior to generation.
        {
            map_RowPassPrevious[w] = false;
        }
        map_RowPassPrevious[map_widthHalf] = true;

        for (int l = 0; l < map_lengthBack; l++)  //sets initial background/behind path in woods
        {
            for (int w = 0; w < map_widthTotal; w++)  //Loops through the row and places the tiles
            {
                float temp_tilePosX = ((float)l - (float)map_lengthBack) * map_tileInterval;
                float temp_tilePosY = 0.0f;  //playfield base level
                float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
                Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

                if (w == map_widthHalf)  //leaves middle row clear for 'path' back.
                {
                    map_arrayTiles[l, w] = Instantiate(BiomeRandomBiomeRow(0, true), temp_tilePos, Quaternion.identity);
                }
                else
                {
                    map_arrayTiles[l, w] = Instantiate(BiomeRandomBiomeRow(0, false), temp_tilePos, Quaternion.identity);
                    //map_arrayTiles[l, w] = Instantiate(pfb_GrassTree, temp_tilePos, Quaternion.identity);
                }
            }
        }

        for (int w = 0; w < map_widthTotal; w++)
        {
            float temp_tilePosX = 0.0f; //((float)l - (float)map_lengthBack) * map_tileInterval;
            float temp_tilePosY = 0.0f;  //playfield base level
            float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
            Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

            if(w == map_widthHalf - 1 || w == map_widthHalf || w == map_widthHalf + 1)
            {
                map_arrayTiles[map_lengthBack, w] = Instantiate(BiomeRandomBiomeRow(0, true), temp_tilePos, Quaternion.identity);
            }
            else
            {
                map_arrayTiles[map_lengthBack, w] = Instantiate(BiomeRandomBiomeRow(0, false), temp_tilePos, Quaternion.identity);
            }
            
        }

        for (int l = map_lengthBack + 1; l < map_lengthTotal; l++)  //generates rest of map
        {
            map_RowPass[0] = false;  //ensures edges are unpassable for art reasons
            map_RowPass[1] = false;
            map_RowPass[map_RowPassPrevious.Length - 1] = false;
            map_RowPass[map_RowPassPrevious.Length - 2] = false;
            int rowBiome = Random.Range(0, biomeCount);

            for (int w = 2; w < map_widthTotal - 2; w++)  //random fill-in
            {
                //map_RowPass[w] = (Random.value > 0.5f);
                map_RowPass[w] = false;
            }
            
            map_RowPass[map_readFrame] = true;  //ensures there is a path for player overrides randomness
            if (Random.value > 0.5f)  //'path' is two clear tiles per row, this will randomly shift it one side or another to make it wander
            {
                if (map_readFrame < map_RowPass.Length - 3)
                {
                    map_readFrame++;
                }
            }
            else
            {
                if (map_readFrame > 2)
                {
                    map_readFrame--;
                }
            }
            map_RowPass[map_readFrame] = true;

            for (int w = 0; w < map_RowPass.Length; w++)  //instansiates the GameObject tiles
            {
                float temp_tilePosX = ((float)l - (float)map_lengthBack) * map_tileInterval;
                float temp_tilePosY = 0.0f;  //playfield base level
                float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
                Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

                map_arrayTiles[l, w] = Instantiate(BiomeRandomBiomeRow(rowBiome, map_RowPass[w]), temp_tilePos, Quaternion.identity);
            }

            Array.Copy(map_RowPass, map_RowPassPrevious, map_RowPass.Length);  //moves current pass-row into previous pass-row for next loop
        }

        return true;
    }

    private bool MapNewRowFront()
    {
        int rowBiome = Random.Range(0, biomeCount);

        map_RowPass[0] = false;  //ensures edges are unpassable for art reasons
        map_RowPass[1] = false;
        map_RowPass[map_RowPassPrevious.Length - 1] = false;
        map_RowPass[map_RowPassPrevious.Length - 2] = false;
        for (int w = 2; w < map_widthTotal - 2; w++)  //random fill-in
        {
            //map_RowPass[w] = (Random.value > 0.5f);
            map_RowPass[w] = false;
        }
        map_RowPass[map_readFrame] = true;  //ensures there is a path for player overrides randomness
        if (Random.value > 0.5f)
        {
            if (map_readFrame < map_RowPass.Length - 3)
            {
                map_readFrame++;
            }
        }
        else
        {
            if (map_readFrame > 2)
            {
                map_readFrame--;
            }
        }
        map_RowPass[map_readFrame] = true;

        for (int w = 0; w < map_widthTotal; w++)
        {
            Destroy(map_arrayTiles[map_backRow, w]);
            
            float temp_tilePosX = (float)map_lengthForward * map_tileInterval;
            float temp_tilePosY = 0.0f;  //playfield base level
            float temp_tilePosZ = ((float)w - (float)map_widthHalf) * map_tileInterval;
            Vector3 temp_tilePos = new Vector3(temp_tilePosX, temp_tilePosY, temp_tilePosZ);

            map_arrayTiles[map_backRow, w] = Instantiate(BiomeRandomBiomeRow(rowBiome,map_RowPass[w]), temp_tilePos, Quaternion.identity);
        }
        map_backRow++;
        map_backRow %= map_lengthTotal;

        Array.Copy(map_RowPass, map_RowPassPrevious, map_RowPass.Length);  //moves current pass-row into previous pass-row for next loop

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
            0 => tileArray_GrassClear[Random.Range(0, tileArray_GrassClear.Length)],
            1 => tileArray_RoadClear[Random.Range(0, tileArray_RoadClear.Length)],
            2 => tileArray_WaterClear[Random.Range(0, tileArray_WaterClear.Length)],
            _ => pfb_BlankTile,
        };
    }

    private GameObject BiomeRandomBiomeRow(int biome, bool pass)
    {
        if (pass)
        {
            return biome switch
            {
                0 => tileArray_GrassClear[Random.Range(0, tileArray_GrassClear.Length)],
                1 => tileArray_RoadClear[Random.Range(0, tileArray_RoadClear.Length)],
                2 => tileArray_WaterClear[Random.Range(0, tileArray_WaterClear.Length)],
                _ => pfb_BlankTile,
            };
        }
        else
        {
            return biome switch
            {
                0 => tileArray_GrassBlock[Random.Range(0, tileArray_GrassBlock.Length)],
                1 => tileArray_RoadClear[Random.Range(0, tileArray_RoadClear.Length)],
                2 => tileArray_WaterBlock[Random.Range(0, tileArray_WaterBlock.Length)],
                _ => pfb_BlankTile,
            };
        }
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

    private byte SurroundingTileMap(int x, int z, string tag)
    {
        player_tileMap = 0b00000000;

        if (map_arrayTiles[(x + map_lengthTotal + 1) % map_lengthTotal, (z + map_widthTotal + 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b10000000;
        }
        if (map_arrayTiles[(x + map_lengthTotal + 1) % map_lengthTotal, (z + map_widthTotal + 0) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b01000000;
        }
        if (map_arrayTiles[(x + map_lengthTotal + 1) % map_lengthTotal, (z + map_widthTotal - 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00100000;
        }
        if (map_arrayTiles[(x + map_lengthTotal + 0) % map_lengthTotal, (z + map_widthTotal + 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00010000;
        }
        if (map_arrayTiles[(x + map_lengthTotal + 0) % map_lengthTotal, (z + map_widthTotal - 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00001000;
        }
        if (map_arrayTiles[(x + map_lengthTotal + -1) % map_lengthTotal, (z + map_widthTotal + 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00000100;
        }
        if (map_arrayTiles[(x + map_lengthTotal - 1) % map_lengthTotal, (z + map_widthTotal + 0) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00000010;
        }
        if (map_arrayTiles[(x + map_lengthTotal - 1) % map_lengthTotal, (z + map_widthTotal - 1) % map_widthTotal].tag == tag)
        {
            player_tileMap += 0b00000001;
        }

        return player_tileMap;
    }

    public bool Map_MoveForward(int backstepCount)
    {
        if (backstepCount <= 0)
        {
            if (!map_isMoving)
            {
                map_isMoving = true;
                creatureController.GetComponent<TrainController>().Creatures_MoveForward();
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

    public byte Map_PlayerPassMap(Vector3 playerPos)
    {
        for (int x = 0; x < map_lengthTotal; x++)
        {
            float tempRow = map_arrayTiles[x, 0].transform.position.x;
            if (tempRow >= playerPos.x - 0.1 && tempRow <= playerPos.x + 0.1)
            {
                for (int z = 0; z < map_widthTotal; z++)
                {
                    float tempColumn = map_arrayTiles[x, z].transform.position.z;
                    if (tempColumn >= playerPos.z - 0.1 && tempColumn <= playerPos.z + 0.1)
                    {
                        byte tempPass = SurroundingTileMap(x, z, TileTag_Pass);
                        byte tempKill = SurroundingTileMap(x, z, TileTag_Kill);
                        return ((byte)(tempPass | tempKill));
                        //return SurroundingTileMap(x, z, TileTag_Pass);
                    }
                }
            }
        }
        return 0;
    }

    public byte Map_PlayerKillMap(Vector3 playerPos)
    {
        for (int x = 0; x < map_lengthTotal; x++)
        {
            float tempRow = map_arrayTiles[x, 0].transform.position.x;
            if (tempRow >= playerPos.x - 0.1 && tempRow <= playerPos.x + 0.1)
            {
                for (int z = 0; z < map_widthTotal; z++)
                {
                    float tempColumn = map_arrayTiles[x, z].transform.position.z;
                    if (tempColumn >= playerPos.z - 0.1 && tempColumn <= playerPos.z + 0.1)
                    {
                        return SurroundingTileMap(x, z, TileTag_Kill);
                    }
                }
            }
        }
        return 0;
    }

    public float GetMapTileInterval => map_tileInterval;
    public int GetMapHalfWidth => map_widthHalf;
    public int GetMapFullWidth => map_widthTotal;
    public int GetMapForwardLength => map_lengthForward;
}