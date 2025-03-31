using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWinMenu : MonoBehaviour
{
    public Button NextLevelButton;
    public string NextSceneName;
    public Button QuitButton;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update

    void Start()
    {
        // Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        QuitButton.onClick.AddListener(TaskOnClick);
        NextLevelButton.onClick.AddListener(delegate { TaskWithParameters(NextSceneName); });
    }

    void TaskOnClick()
    {
        Application.Quit();

    }

    void TaskWithParameters(string message)
    {
        SceneManager.LoadScene(message);   

    }
}

