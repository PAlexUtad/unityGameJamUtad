using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using Unity.VisualScripting;
using System.Reflection;

public class UIVisManager : MonoBehaviour
{
    public List<GameObject> UIObjects;
    // Start is called before the first frame update
    void Start()
    {
        disableAll();
        enableObject(0);
        enableObject(1);
    }

    public void enableObject(int index)
    {
        UIObjects[index].SetActive(true);
    }

    public void disableObject(int index)
    {
        UIObjects[index].SetActive(false);
    }
    public void toggleObject(int index)
    {
        UIObjects[index].SetActive(!UIObjects[index].activeSelf);
    }
    public void disableAll()
    {
        foreach(GameObject ob in UIObjects)
        {
            ob.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
