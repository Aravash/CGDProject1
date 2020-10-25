using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    MainMenu,
    OptionsMenu,
    LevelSelectMenu
}

public class CanvasManager : MonoBehaviour
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;

    void Awake()
    {
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.MainMenu);
        
        // Default screen res
        Screen.SetResolution(1920, 1080, true);
    }

    public void SwitchCanvas(CanvasType _type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else { Debug.LogWarning("Canvas not found!"); }
    }
}