using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIScore : MonoBehaviour
{
    public TMP_Text tex;
    //public int PiecesAmt;
    //public float TowerHeight;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
    }

    void Update()
    {
        if (tex)
        {
            tex.text = (ManagerTarget.Instance.targetPiecesAmount * ManagerTarget.Instance.targetHeight).ToString();
        }
    }

}