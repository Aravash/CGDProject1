using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public enum ButtonType
{
    BEGIN,
    OPTIONS,
    ENDGAME,
    OPTIONSRETURN
}

public class ButtonController : MonoBehaviour
{
    CanvasManager canvasManager;
    EventSystem eventSystem;
    Button button;

    public ButtonType buttonType;
    public Button sendToButton;

    private void Start()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
        eventSystem = FindObjectOfType<EventSystem>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonType.BEGIN:
                LevelManager._i.loadLevel(1);
                break;
            case ButtonType.OPTIONS:
                canvasManager.SwitchCanvas(CanvasType.OptionsMenu);
                eventSystem.SetSelectedGameObject(sendToButton.gameObject);
                break;
            case ButtonType.OPTIONSRETURN:
                canvasManager.SwitchCanvas(CanvasType.MainMenu);
                eventSystem.SetSelectedGameObject(sendToButton.gameObject);
                break;
            case ButtonType.ENDGAME:
                Application.Quit();
                break;
        }
    }
}
