using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerTarget : MonoBehaviour
{
    public static ManagerTarget Instance { get; private set; }

    public float targetHeight = 300;
    public float targetCountdown = 300;
    public int targetPiecesAmount = 0;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //setters
    public void SetTargetHeight(float value)
    {
        targetHeight = value; 
    }
    
    public void SetTargetCountdown(float value)
    {
        targetCountdown = value; 
    }
    
    public void SetTargetPiecesAmount(int value)
    {
        targetPiecesAmount = value; 
    }
    //
}
