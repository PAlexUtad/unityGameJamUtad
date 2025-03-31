using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UITarget : MonoBehaviour
{
    public TMP_Text tex;
    public float target;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //gets the targetHeight
        target = ManagerTarget.Instance.targetHeight;
        //
        
        tex.text = target.ToString();
    }

    void Update()
    {
        if (tex) 
        {
            tex.text = target.ToString();
        }
    }
    
    /*public void SetTarget(float newTarget)
    {
        target = newTarget;
        ManagerTarget.Instance.targetHeight = newTarget;
    }*/

}