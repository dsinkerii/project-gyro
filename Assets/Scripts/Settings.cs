using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] TMP_InputField FPScap;
    [SerializeField] Toggle RenderCube;
    [SerializeField] Toggle RenderDebugVals;
    void Start(){
        UpdateSettings();
    }
    public void UpdateSettings(){
        PlayerPrefs.SetInt("DebugOn",(RenderDebugVals.isOn ? 1 : 0));
        PlayerPrefs.SetInt("CubeOn",(RenderCube.isOn ? 1 : 0));
        int fps = 30;
        bool success = int.TryParse(FPScap.text, out fps);
        if(success){
            if(fps < 5){
                fps = 5;
            }else if(fps > 120){
                fps = 120;
            }
        }
        Application.targetFrameRate = fps;
    }
}
