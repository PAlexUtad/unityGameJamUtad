using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UITarget : MonoBehaviour
{
    public TMP_Text tex;
    public float target = 300;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        tex.text = target.ToString();
    }

    void Update()
    {
        if (tex) 
        {
            tex.text = target.ToString();
        }
    }

}