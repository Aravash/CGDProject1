using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    CanvasManager canvasManager;

    private void Start()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OpenMenu(string _type)
    {
        switch (_type)
        {
            case "OptionsMenu":
                canvasManager.SwitchCanvas(CanvasType.OptionsMenu);
                break;
            case "MainMenu":
                canvasManager.SwitchCanvas(CanvasType.MainMenu);
                break;
        }
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
