using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UITimer : MonoBehaviour
{
    public TMP_Text tex;
    //public float countdown;
    float time_elapsed;
    float initial_value;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        initial_value = ManagerTarget.Instance.targetCountdown;
    }

    void Update()
    {
        if (tex) 
        { 
            if (ManagerTarget.Instance.targetCountdown > 0)
            {
                ManagerTarget.Instance.targetCountdown -= Time.deltaTime;
                time_elapsed = initial_value - ManagerTarget.Instance.targetCountdown;
            }
            float min = Mathf.FloorToInt(ManagerTarget.Instance.targetCountdown / 60);
            float sec = Mathf.FloorToInt(ManagerTarget.Instance.targetCountdown % 60);
            tex.text = string.Format("{0,00}:{1,00}", min, sec);
            //Minutes and seconds calculation for elapsed time
            float min_e = Mathf.FloorToInt(time_elapsed / 60);
            float sec_e = Mathf.FloorToInt(time_elapsed % 60);
        }
    }

}