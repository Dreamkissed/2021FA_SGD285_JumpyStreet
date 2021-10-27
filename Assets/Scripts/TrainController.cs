//////////////////////////////////////////////
//Project: Jumpy Street
//Name: Jennifer Wenner
//Section: 2021FA.SGD.285.2141
//Instructor: Prof. Wold
//Date: 10/20/2021
/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour
{
    [SerializeField] private int mobileMonster_MaxCount = 3;
    [SerializeField] private int mobileMonster_MinCount = 0;
    [SerializeField] private float mobileMonster_MaxSpeed = 5.0f;
    [SerializeField] private float mobileMonster_MinSpeed = 1.0f;
    [SerializeField] private float mobileMonster_SpawnIntervalMax = 1.0f;
    [SerializeField] private float mobileMonster_SpawnIntervalMin = 2.5f;
    [SerializeField] private GameObject mobileMonster_prefab;
    [SerializeField] private GameObject terrainControllerObject;

    private float mobileMonster_SpawnTimer;
    private float mobileMonster_SpawnTarget;
    private GameObject mommaMonster;
    private GameObject[] pool_mobileMonster;
    private float[] pool_mobileMonsterSpeed;
    private bool[] pool_mobileMonsterisPacing;
    private Coroutine[] pool_mobileMonsterMovement;


    private void Awake()
    {
        InitializeTrains();
        mobileMonster_SpawnTimer = 0.0f;
    }

    private void Start()
    {
        PositionTrains();
    }

    private void Update()
    {
        Debug.Log("Ticking");
        mobileMonster_SpawnTarget = Random.Range(mobileMonster_SpawnIntervalMin, mobileMonster_SpawnIntervalMax);
        if (mobileMonster_SpawnTimer >= mobileMonster_SpawnTarget)
        {
            SpawnCreature();
            mobileMonster_SpawnTimer = 0.0f;
        }
        else
        {
            mobileMonster_SpawnTimer += Time.deltaTime;
        }
    }

    private bool InitializeTrains()
    {
        mommaMonster = Instantiate(mobileMonster_prefab, new Vector3(0.0f, -50.0f, 0.0f), Quaternion.identity);  //Game ending monster
        mommaMonster.transform.localScale *= 5.0f;
        mommaMonster.SetActive(false);
        pool_mobileMonster = new GameObject[mobileMonster_MaxCount];  //random train monsters side to side
        pool_mobileMonsterSpeed = new float[mobileMonster_MaxCount];
        pool_mobileMonsterisPacing = new bool[mobileMonster_MaxCount];
        pool_mobileMonsterMovement = new Coroutine[mobileMonster_MaxCount];

        for (int i = 0; i < pool_mobileMonster.Length; i++)
        {
            pool_mobileMonster[i] = Instantiate(mobileMonster_prefab, new Vector3(0.0f, -50.0f, 0.0f), Quaternion.identity);
            pool_mobileMonster[i].SetActive(false);
            pool_mobileMonsterSpeed[i] = Random.Range(mobileMonster_MinSpeed, mobileMonster_MaxSpeed);
            pool_mobileMonsterisPacing[i] = false;
        }
        
        return true;
    }

    private bool PositionTrains()
    {
        for (int i = 0; i <pool_mobileMonster.Length; i++)
        {
            int x = (i + 1) * 2;
            //Vector3 tempPosition = new Vector3(terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * x, 0.0f, terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * terrainControllerObject.GetComponent<TerrainGeneration>().GetMapHalfWidth * -1);
            Vector3 tempPosition = new Vector3(0.0f, 0.0f, 0.0f);
                tempPosition.x = terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * x;
                tempPosition.y = mobileMonster_prefab.transform.localScale.y / 2;
                tempPosition.z = terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * terrainControllerObject.GetComponent<TerrainGeneration>().GetMapHalfWidth * -1;
            pool_mobileMonster[i].transform.position = tempPosition;
            //pool_mobileMonster[i].SetActive(true);
        }

        return true;
    }

    public bool SpawnCreature()
    {
        //Debug.Log("Attempting to Spawn");
        for (int i = 0; i < pool_mobileMonster.Length; i++)
        {
            //Debug.Log("Spawning Loop Check" + i.ToString() + ", " + pool_mobileMonsterisPacing[i].ToString());
            if (!pool_mobileMonsterisPacing[i])
            {
                //Debug.Log("Picked mob to spawn");
                pool_mobileMonsterisPacing[i] = true;
                Vector3 tempPosition = pool_mobileMonster[i].transform.position;
                tempPosition.x = (float)Random.Range(1, terrainControllerObject.GetComponent<TerrainGeneration>().GetMapForwardLength) * terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval;
                tempPosition.z = terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * terrainControllerObject.GetComponent<TerrainGeneration>().GetMapHalfWidth * -1;
                pool_mobileMonster[i].transform.position = tempPosition;
                pool_mobileMonster[i].SetActive(true);

                pool_mobileMonsterMovement[i] = StartCoroutine(MoveCreature(i));

                return true; ;
            }
        }
        
        return true;
    }

    IEnumerator MoveCreature(int critter)
    {
        //Debug.Log("Moving Creature" + critter.ToString());
        while(pool_mobileMonster[critter].transform.position.z < terrainControllerObject.GetComponent<TerrainGeneration>().GetMapTileInterval * terrainControllerObject.GetComponent<TerrainGeneration>().GetMapHalfWidth)
        {
            //float speedTwitch = Random.Range(mobileMonster_MinSpeed, mobileMonster_MaxSpeed);
            Vector3 tempPosition = pool_mobileMonster[critter].transform.position;
            tempPosition.z += pool_mobileMonsterSpeed[critter] * Time.deltaTime;
            pool_mobileMonster[critter].transform.position = tempPosition;

            yield return null;
        }
        pool_mobileMonster[critter].SetActive(false);
        pool_mobileMonsterisPacing[critter] = false;
        StopCoroutine(pool_mobileMonsterMovement[critter]);
    }
}