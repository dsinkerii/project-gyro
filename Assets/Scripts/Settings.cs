using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle RenderCube;
    [SerializeField] Toggle RenderDebugVals;
    void Start(){
        if(PlayerPrefs.GetInt("NotFirstTime") == 1){
            RenderDebugVals.isOn = PlayerPrefs.GetInt("DebugOn") == 1 ? true : false;
            RenderCube.isOn = PlayerPrefs.GetInt("CubeOn") == 1 ? true : false;
        }else{
            PlayerPrefs.SetInt("NotFirstTime", 1);
            UpdateSettings();
        }
    }
    public void UpdateSettings(){
        PlayerPrefs.SetInt("DebugOn",(RenderDebugVals.isOn ? 1 : 0));
        PlayerPrefs.SetInt("CubeOn",(RenderCube.isOn ? 1 : 0));
    }
}
