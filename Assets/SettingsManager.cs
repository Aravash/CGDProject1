using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Text resolutionLabel;

    Button resolutionChangeButton;

    Resolution[] resolutions;
    List<string> resolutionOptions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        //resolutionDropdown.text = "test";
        //resolutionChangeButton = GetComponentInChildren<Button>();

        resolutionOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
        }
    }
}
