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
    List<ButtonController> buttonControllerList;

    public ButtonType buttonType;

    private void Start()
    {
        canvasManager = FindObjectOfType<CanvasManager>();
        eventSystem = FindObjectOfType<EventSystem>();

        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);

        buttonControllerList = FindObjectsOfType<ButtonController>().ToList();
    }

    void OnButtonClicked()
    {
        switch (buttonType)
        {
            case ButtonType.BEGIN:
                SceneManager.LoadScene("Level1");
                break;
            case ButtonType.OPTIONS:
                canvasManager.SwitchCanvas(CanvasType.OptionsMenu);
                eventSystem.SetSelectedGameObject(buttonControllerList.Find(x => x.buttonType == ButtonType.OPTIONSRETURN).gameObject);
                break;
            case ButtonType.OPTIONSRETURN:
                canvasManager.SwitchCanvas(CanvasType.MainMenu);
                break;
            case ButtonType.ENDGAME:
                Application.Quit();
                break;
        }
    }
}
