using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UITimer : MonoBehaviour
{
    public TMP_Text tex;
    public float countdown = 300;
    float time_elapsed;
    float initial_value;

    void Start()
    {
        initial_value = countdown;
    }

    void Update()
    {
        if (tex) 
        { 
            if (countdown > 0)
            {
                countdown -= Time.deltaTime;
                time_elapsed = initial_value - countdown;
            }
            float min = Mathf.FloorToInt(countdown / 60);
            float sec = Mathf.FloorToInt(countdown % 60);
            tex.text = string.Format("{0,00}:{1,00}", min, sec);
            //Minutes and seconds calculation for elapsed time
            float min_e = Mathf.FloorToInt(time_elapsed / 60);
            float sec_e = Mathf.FloorToInt(time_elapsed % 60);
        }
    }

}