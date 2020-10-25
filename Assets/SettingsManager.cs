using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Text resolutionLabel;
    public Toggle reverseModeToggle;
    
    List<string> resOptions;
    int selectedResOption;

    private void Start()
    {
        resOptions = new List<string>();
        selectedResOption = 3;

        SetupResolutions();
    }

    private void SetupResolutions()
    {
        resOptions.Add("1080 x 720");
        resOptions.Add("1080 x 720 (FS)");
        resOptions.Add("1920 x 1080");
        resOptions.Add("1920 x 1080 (FS)");
        resOptions.Add("2560 x 1440");
        resOptions.Add("2560 x 1440 (FS)");
        resOptions.Add("3840 x 2160");
        resOptions.Add("3840 x 2160 (FS)");
    }

    public void ChangeResolution()
    {
        if (selectedResOption < resOptions.Count - 1)
        {
            selectedResOption++;
        }
        else
        {
            selectedResOption = 0;
        }

        SetResolution();
    }

    private void SetResolution()
    {
        resolutionLabel.text = resOptions[selectedResOption];

        char[] chars = resOptions[selectedResOption].ToCharArray();

        int xIndex = 0;
        int endNumberIndex = 0;
        bool isFS = false;
        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] == 'x')
            {
                xIndex = i;
            }

            if (char.IsDigit(chars[i]))
            {
                endNumberIndex = i;
            }

            if (chars[i] == '(')
            {
                isFS = true;
            }
        }

        int resW = int.Parse(resOptions[selectedResOption].Substring(0, xIndex - 1));
        int resH = int.Parse(resOptions[selectedResOption].Substring(xIndex + 2, endNumberIndex - (xIndex + 1)));

        Screen.SetResolution(resW, resH, isFS);
    }

    public void ToggleReverseMode()
    {
        LevelManager._i.setReverseMode(reverseModeToggle.isOn);
    }
}
