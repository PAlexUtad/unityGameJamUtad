using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIScore : MonoBehaviour
{
    public TMP_Text tex;
    public int PiecesAmt;
    public float TowerHeight;

    void Start()
    {
    }

    void Update()
    {
        if (tex)
        {
            tex.text = (PiecesAmt * TowerHeight).ToString();
        }
    }

}