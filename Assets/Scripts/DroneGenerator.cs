using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DroneGenerator : MonoBehaviour
{
    public static DroneGenerator Instance;

    public GameObject droneRoot;
    public GameObject pieceRoot;
    public GameObject dronePrefab;
    public GameObject piecePrefab;
    public List<Mesh> piecesList;
    public Material pieceMaterialActive;
    public GameObject Floor;
    public int maxDronesAlive = 5;
    public float generationRateSeconds = 2.0f;
    public float targetHeight = 2.0f;


    private float timer = 0.0f;
    private Vector2 spawnBoundsX;
    private Vector2 spawnBoundsY;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnBoundsX = new Vector2(Floor.transform.position.x - (Floor.GetComponent<MeshRenderer>().bounds.size.x/2.0f) - 5.0f, Floor.transform.position.x + (Floor.GetComponent<MeshRenderer>().bounds.size.x / 2.0f) + 5.0f);
        spawnBoundsY = new Vector2(Floor.transform.position.z - (Floor.GetComponent<MeshRenderer>().bounds.size.z / 2.0f) - 5.0f, Floor.transform.position.z + (Floor.GetComponent<MeshRenderer>().bounds.size.z / 2.0f) + 5.0f);
        Debug.Log(spawnBoundsX);
        Debug.Log(spawnBoundsY);
    }

    public void GenerateDrone(bool useBoundsX) 
    {
        if (droneRoot.transform.childCount < maxDronesAlive) 
        {
            float xPos = 0, zPos = 0; 
            if (useBoundsX) 
            {
                zPos = Random.Range(-1.0f, 1.0f) <= 0 ? spawnBoundsY.x : spawnBoundsY.y;
                xPos = Random.Range(spawnBoundsX.x, spawnBoundsX.y);
            }
            else
            {
                xPos = Random.Range(-1.0f, 1.0f) <= 0 ? spawnBoundsY.x : spawnBoundsY.y;
                zPos = Random.Range(spawnBoundsY.x, spawnBoundsY.y);
            }

            Mesh DronePiece = piecesList[Random.Range(0, piecesList.Count)];

            GameObject Created = Instantiate(dronePrefab, new Vector3(xPos, targetHeight, zPos), Quaternion.identity, droneRoot.transform);
            Created.GetComponent<DroneDrop>().piecePrefab = piecePrefab;
            Created.GetComponent<DroneDrop>().piecePrefab.GetComponent<MeshFilter>().mesh = DronePiece;
            Created.GetComponent<DroneDrop>().piecePrefab.AddComponent<MeshCollider>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= 0) 
        { 
            GenerateDrone(Random.Range(-1.0f, 1.0f) <= 0 ? true : false);
            timer = generationRateSeconds;
        }
        else
        {
            timer -= Time.deltaTime;
        }
        if(droneRoot.transform.childCount > 0)
        {
            droneRoot.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>().material = pieceMaterialActive;
        }

        if (Input.GetMouseButtonDown(0)) DropPieceFromDrone();
    }

    void DropPieceFromDrone() { return; }
}
