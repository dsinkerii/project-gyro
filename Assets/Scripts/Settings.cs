using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] Toggle RenderCube;
    [SerializeField] Toggle RenderDebugVals;
    [SerializeField] Toggle ToggleLerp;
    [SerializeField] Slider Threshold;
    [SerializeField] TextMeshProUGUI ThresholdLabel;
    [SerializeField] Slider Intensity;
    [SerializeField] TextMeshProUGUI IntensityLabel;

    [SerializeField] Image ImageLeft;
    [SerializeField] Image ImageUp;
    [SerializeField] Image ImageRight;
    [SerializeField] Image ImageDown;
    [SerializeField] Image ImageRoll;
    [SerializeField] Color colorLeft;
    [SerializeField] Color colorUp;
    [SerializeField] Color colorRight;
    [SerializeField] Color colorDown;
    [SerializeField] Color colorRoll;

    [SerializeField] GyroCubeVisualisator visualisator;
    [SerializeField] GyroValues gyroValues;
    bool updateSettingsLock = true; // prevent from the PlayerPrefs being modified at the start of the script 
    void Start(){
        updateSettingsLock = true;
        if(PlayerPrefs.GetInt("NotFirstTime") == 1){
            RenderDebugVals.isOn = PlayerPrefs.GetInt("DebugOn") == 1 ? true : false;
            RenderCube.isOn = PlayerPrefs.GetInt("CubeOn") == 1 ? true : false;
            ToggleLerp.isOn = PlayerPrefs.GetInt("lerpSpeed") == 0 ? true : false;

            if(PlayerPrefs.GetInt("Threshold") == 0){
                PlayerPrefs.SetInt("Threshold", 5);
            }
            if(PlayerPrefs.GetInt("Intensity") == 0){
                PlayerPrefs.SetInt("Intensity", 10);
            }

            Threshold.value = PlayerPrefs.GetInt("Threshold");
            Intensity.value = PlayerPrefs.GetInt("Intensity");
            ThresholdLabel.text = $"({Threshold.value}) threshold";
            IntensityLabel.text = $"({Intensity.value}) intensity";
        }else{
            PlayerPrefs.SetInt("NotFirstTime", 1);
            updateSettingsLock = false;
            UpdateSettings();
        }
    }
    void LateUpdate(){
        updateSettingsLock = false;
    }
    public void UpdateSettings(){
        if(!updateSettingsLock){
            print("Settings update");
            PlayerPrefs.SetInt("DebugOn",(RenderDebugVals.isOn ? 1 : 0));
            PlayerPrefs.SetInt("CubeOn",(RenderCube.isOn ? 1 : 0));
            PlayerPrefs.SetInt("lerpSpeed", (ToggleLerp.isOn ? 0 : 1));

            PlayerPrefs.SetInt("Threshold", Mathf.RoundToInt(Threshold.value));
            PlayerPrefs.SetInt("Intensity", Mathf.RoundToInt(Intensity.value));

            ThresholdLabel.text = $"({Mathf.Round(Threshold.value)}) threshold";
            IntensityLabel.text = $"({Mathf.Round(Intensity.value)}) intensity";

            visualisator.lerpVal = PlayerPrefs.GetInt("lerpSpeed") == 0 ? 0.2f : 1;
            gyroValues.LerpSpeed = PlayerPrefs.GetInt("lerpSpeed") == 0 ? 0.5f : 1;
        }
    }

    void OnGUI(){
        ImageLeft.color =  gyroValues.HittingPart.y == -1 ? colorLeft : Color.black;
        ImageUp.color =    gyroValues.HittingPart.y == 1 ? colorUp : Color.black;
        ImageRight.color = gyroValues.HittingPart.x == -1 ? colorRight : Color.black;
        ImageDown.color =  gyroValues.HittingPart.x == 1 ? colorDown : Color.black;
        ImageRoll.color =  gyroValues.HittingPart.z != 0 ? colorRoll : Color.black;
    }
}