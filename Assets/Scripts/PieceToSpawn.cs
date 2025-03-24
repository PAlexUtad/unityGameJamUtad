using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;


enum PieceShape
{
    Small = 0,
    SmallLarge = 1,
    Medium = 2, 
    MediumLarge = 3,
    Large = 4,
    LargeLarge = 5
}
public class PieceToSpawn : MonoBehaviour
{

    [SerializeField] public Transform spawnTransform;
    [SerializeField] private PieceShape shape;

    [SerializeField] private List<Mesh> MeshesList = new List<Mesh>();
    
    private Transform InitialTransform;
    // Start is called before the first frame update
    void Start()
    {
        int rand         = UnityEngine.Random.Range(0, MeshesList.Count) - 1;
        InitialTransform = transform;
        
        MeshFilter mesh  = GetComponent<MeshFilter>();
        mesh.mesh        = MeshesList[rand];

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = InitialTransform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.isKinematic = false;

    }
}
