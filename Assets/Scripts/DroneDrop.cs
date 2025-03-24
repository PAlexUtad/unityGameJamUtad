using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDrop : MonoBehaviour
{
    [HideInInspector] public GameObject piecePrefab;
    private float currentTime = 0.0f;
    //public DroneMovement moveState;
    public float maxIndexWaypoint;

    private void Start()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - (GetComponent<MeshRenderer>().bounds.size.y/2.0f + piecePrefab.GetComponent<MeshRenderer>().bounds.size.y/2.0f), transform.position.z);
        Instantiate(piecePrefab, pos, Quaternion.identity, transform);
        //moveState = GetComponent<DroneMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float R = 1;
        float G = 1 - (1 / maxIndexWaypoint);
        float B = G;
        //piecePrefab.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", new Color (R,G,B, 1.0f)); // Changes all
        if (Input.GetMouseButtonDown(0)) DropPieceFromDrone();
    }

    void DropPieceFromDrone()
    {

    }
}
