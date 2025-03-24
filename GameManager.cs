using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
   
public struct FloorTileInfo
{
    public int TileIndex;
    public GameObject FloorObj;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum Adjacent {ManhattanAdjacent, EulerAdjacent};
    public enum Generation {Single, Double, AllAdjacent};
    private NavMeshSurface NavSurface;

    [Header("Floor Generation Settings")]

    public List<GameObject> FloorPrefabs;
    [Tooltip("Growth based on when distance is less than 2 (including or not diagonals)")] public Adjacent PossibleGrowth = Adjacent.ManhattanAdjacent;
    [Tooltip("How many tiles to generate")] public Generation GrowthRate = Generation.Single;
    public GameObject FloorRoot;
    [Range(10,20)] public int MaxMapSize = 15;
    public bool GenerateTileOnStart = true;

    [Header("Enemy Generation Settings")]
    public List<GameObject> EnemiesPrefabs;
    public GameObject EnemiesRoot;
    [Range(1,5)] public int MaxNumEnemiesPerFloorTile = 2;
    [Range(10, 100)] public int MaxNumEnemiesTotal = 20;
    [ContextMenuItem("Spawn Enemies Once", "SpawnEnemiesRandomly")]
    public bool GenerateEnemiesOnStart = false;
    
    [HideInInspector]
    public FloorTileInfo CurrentFloor; // Floor child the player is currently standing on
    private List<Vector2> FloorMap = new List<Vector2>(); // This can be left unused if you get the children from the "FloorRoot" member

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        NavSurface = FloorRoot.GetComponent<NavMeshSurface>(); // Get Navmesh Surface From (Scene | Prefab) Floor Object 
        if(GenerateTileOnStart)
        {
            CurrentFloor.TileIndex = 0;
            CurrentFloor.FloorObj = Instantiate(FloorPrefabs[Random.Range(0, FloorPrefabs.Count)], new Vector3(0, 0, 0), Quaternion.identity, FloorRoot.transform);
            FloorMap.Add(new Vector2(0,0));
        }
        NavSurface.BuildNavMesh();
        
        if(GenerateEnemiesOnStart) SpawnEnemiesRandomly(MaxNumEnemiesPerFloorTile);
    }

    public void SetCurrentFloor(GameObject CurrFloor)
    {
        int i = 0;
        foreach(Transform Child in FloorRoot.transform)
        {
            if(Child.gameObject == CurrFloor) break;
            else i++;
        }
        CurrentFloor.TileIndex = i;
        CurrentFloor.FloorObj = CurrFloor;
    }

    public void SpawnEnemiesRandomly(float NumEnemies)
    {
        
        if(MaxNumEnemiesTotal <= 0) return;
        int ToGenerate = Random.Range(1, MaxNumEnemiesPerFloorTile+1);
        for (int i = 0; i < ToGenerate; i++)
        {
            MaxNumEnemiesTotal -= 1;
            if(MaxNumEnemiesTotal > 0){
                Vector3 MinimumDistance = CurrentFloor.FloorObj.GetComponent<MeshRenderer>().bounds.size;
                
                Vector3 SpawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
                SpawnPoint.x = Random.Range(MinimumDistance.x * 0.15f, MinimumDistance.x * 0.45f);
                SpawnPoint.z = Random.Range(MinimumDistance.z * 0.15f, MinimumDistance.z * 0.45f);
                SpawnPoint.x = CurrentFloor.FloorObj.transform.position.x + (Random.Range(-1,1) < 0 ? SpawnPoint.x * -1 : SpawnPoint.x);
                SpawnPoint.z = CurrentFloor.FloorObj.transform.position.z + (Random.Range(-1,1) < 0 ? SpawnPoint.z * -1 : SpawnPoint.z);
                Instantiate(EnemiesPrefabs[Random.Range(0, EnemiesPrefabs.Count)], SpawnPoint, Quaternion.identity, EnemiesRoot.transform); //EnemiesPrefabs
            }
        }
    }

    Vector2 GeneratePossibleManhattan(int[] PossibleValues)
    {
        Vector2 Generated = new Vector2(0,0);
        List<int> PossibleValues_X = new List<int>();
        List<int> PossibleValues_Y = new List<int>();
        for (var i = 1; i < PossibleValues.Length; i++)
        {
            if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x + PossibleValues[i], FloorMap[CurrentFloor.TileIndex].y + PossibleValues[0]))){
                PossibleValues_X.Add(i);
                PossibleValues_Y.Add(0);
            }
            if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x + PossibleValues[0], FloorMap[CurrentFloor.TileIndex].y + PossibleValues[i]))){
                PossibleValues_X.Add(0);
                PossibleValues_Y.Add(i);
            }
        }
        if(PossibleValues_X.Count == 0)
        {
            return new Vector2(0,0);
        }
        int IdxGenerated = Random.Range(0, PossibleValues_X.Count);
        Generated.x = PossibleValues[PossibleValues_X[IdxGenerated]];
        Generated.y = PossibleValues[PossibleValues_Y[IdxGenerated]];
        
        return Generated;
    }

    Vector2 GeneratePossibleEuler(int[] PossibleValues)
    {
        Vector2 Generated = new Vector2(0,0);
        List<int> PossibleValues_X = new List<int>();
        List<int> PossibleValues_Y = new List<int>();
        for (var i = 0; i < PossibleValues.Length; i++)
        {
            for (var j = 0; j < PossibleValues.Length; j++)
            {
                if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x + PossibleValues[i], FloorMap[CurrentFloor.TileIndex].y + PossibleValues[j]))){
                    PossibleValues_X.Add(i);
                    PossibleValues_Y.Add(j);
                }
            }
        }
        if(PossibleValues_X.Count == 0)
        {
            return new Vector2(0,0);
        }
        int IdxGenerated = Random.Range(0, PossibleValues_X.Count);
        Generated.x = PossibleValues[PossibleValues_X[IdxGenerated]];
        Generated.y = PossibleValues[PossibleValues_Y[IdxGenerated]];
        return Generated;
    }

    public void GenerateNewFloorTile()
    {
        
        //if(MaxMapSize <= 0) return;

        int[] PossibleValues = { 0, 1, -1 };
        int TilesToGenerate = 0;

        switch (PossibleGrowth)
        {
            case Adjacent.ManhattanAdjacent:
                switch(GrowthRate) 
                {
                    case Generation.Single:
                        TilesToGenerate = 1;
                        break;

                    case Generation.Double:
                        TilesToGenerate = 2;
                        break;

                    default:
                        TilesToGenerate = 4;
                        break;
                }
                break;
            default:
                switch(GrowthRate) 
                {
                    case Generation.Single:
                        TilesToGenerate = 1;
                        break;

                    case Generation.Double:
                        TilesToGenerate = 2;
                        break;

                    default:
                        TilesToGenerate = 8;
                        break;
                }
                break;
        }
        GameObject FloorToGenerate;
        switch (TilesToGenerate)
        {
            case 4:
                if(MaxMapSize > 0)
                {
                    MaxMapSize--;
                    // [0, 1]
                    if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x, FloorMap[CurrentFloor.TileIndex].y + 1))){
                        FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                        Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x, 0, CurrentFloor.FloorObj.transform.position.z + FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.z), Quaternion.identity, FloorRoot.transform);
                        FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x, FloorMap[CurrentFloor.TileIndex].y + 1));
                    }
                }

                if(MaxMapSize > 0)
                {
                    MaxMapSize--;
                    // [0, -1]
                    if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x, FloorMap[CurrentFloor.TileIndex].y - 1))){
                        FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                        Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x, 0, CurrentFloor.FloorObj.transform.position.z - FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.z), Quaternion.identity, FloorRoot.transform);
                        FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x, FloorMap[CurrentFloor.TileIndex].y - 1));
                    }
                }

                if(MaxMapSize > 0)
                {
                    MaxMapSize--;
                    // [1, 0]
                    if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x + 1, FloorMap[CurrentFloor.TileIndex].y))){
                        FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                        Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x + FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.x, 0, CurrentFloor.FloorObj.transform.position.z), Quaternion.identity, FloorRoot.transform);
                        FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x+1, FloorMap[CurrentFloor.TileIndex].y));
                    }
                }

                if(MaxMapSize > 0)
                {
                    MaxMapSize--;
                    // [-1, 0]
                    if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x - 1, FloorMap[CurrentFloor.TileIndex].y))){
                        FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                        Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x - FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.z, 0, CurrentFloor.FloorObj.transform.position.z), Quaternion.identity, FloorRoot.transform);
                        FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x-1, FloorMap[CurrentFloor.TileIndex].y));
                    }
                }
                break;

            case 8:
                for (int i = 0; i < 3; i++)
                {
                    Vector2 Generated = new Vector2(0,0);
                    for (int j = 0; j < 3; j++)
                    {
                        if(!(i == 0 && j == 0))
                        {
                            if(MaxMapSize > 0)
                            {
                                MaxMapSize--;
                                if(!FloorMap.Contains(new Vector2(FloorMap[CurrentFloor.TileIndex].x + PossibleValues[i], FloorMap[CurrentFloor.TileIndex].y + PossibleValues[j]))){
                                    FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                                    Generated.x = PossibleValues[i] * FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.x;
                                    Generated.y = PossibleValues[j] * FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.z;
                                    Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x + Generated.x, 0, CurrentFloor.FloorObj.transform.position.z + Generated.y), Quaternion.identity, FloorRoot.transform);
                                    FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x + PossibleValues[i], FloorMap[CurrentFloor.TileIndex].y + PossibleValues[j]));
                                }
                            }
                        }
                    }
                }
                break;

            default:
            
                for (int i = 0; i < TilesToGenerate; i++)
                {
                    if(MaxMapSize > 0)
                    {
                        MaxMapSize--;
                        FloorToGenerate = FloorPrefabs[Random.Range(0, FloorPrefabs.Count)];
                        Vector2 Generated = new Vector2(0,0);

                        switch (PossibleGrowth)
                        {
                            case Adjacent.ManhattanAdjacent:
                                Generated = GeneratePossibleManhattan(PossibleValues);
                                break;
                            default:
                                Generated = GeneratePossibleEuler(PossibleValues);
                                break;
                        }

                        if(!(Generated.x == 0 && Generated.y == 0))
                        {
                            FloorMap.Add(new Vector2(FloorMap[CurrentFloor.TileIndex].x + Generated.x, FloorMap[CurrentFloor.TileIndex].y + Generated.y));
                            Generated.x = Generated.x * FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.x;
                            Generated.y = Generated.y * FloorToGenerate.GetComponent<MeshRenderer>().bounds.size.z;
                            Instantiate(FloorToGenerate, new Vector3(CurrentFloor.FloorObj.transform.position.x + Generated.x, 0, CurrentFloor.FloorObj.transform.position.z + Generated.y), Quaternion.identity, FloorRoot.transform);
                        }
                    }
                }
                break;
        }

        NavSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        //Instantiate(prefab, new Vector3(0, -0.5f, 0), Quaternion.identity);
    }
}
