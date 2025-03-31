using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneDrop : MonoBehaviour
{
    [HideInInspector] public GameObject piecePrefab;
    [HideInInspector] public Mesh pieceMesh;
    private float currentTime = 0.0f;
    //public DroneMovement moveState;
    public float maxIndexWaypoint;
    MeshFilter currMeshFilter;
    MeshCollider currMeshCollider;

    private void Start()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y - (GetComponent<MeshRenderer>().bounds.size.y/2.0f + piecePrefab.GetComponent<MeshRenderer>().bounds.size.y/2.0f), transform.position.z);
        GameObject piece = Instantiate(piecePrefab, pos, Quaternion.identity, transform);
        currMeshFilter = piece.GetComponent<MeshFilter>();
        currMeshCollider = piece.GetComponent<MeshCollider>();
        currMeshFilter.sharedMesh = pieceMesh;
        currMeshCollider.sharedMesh = pieceMesh;
    }
}
