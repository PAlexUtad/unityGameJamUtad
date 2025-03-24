using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIRestartMenu : MonoBehaviour
{
    public Button RestartButton;
    public Button QuitButton;
    // Start is called before the first frame update

    void Start()
    {
        // Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        RestartButton.onClick.AddListener(TaskOnClick);
        QuitButton.onClick.AddListener(delegate { TaskWithParameters("Hello"); });
    }

    void TaskOnClick()
    {
        // Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void TaskWithParameters(string message)
    {
        // Output this to console when Button2 is clicked
        Debug.Log(message);
        Application.Quit();
    }
}

