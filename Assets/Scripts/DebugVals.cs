using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugVals : MonoBehaviour
{
    [SerializeField] GyroValues Values;
    [SerializeField] TrackSettings settings;
    [SerializeField] Transform Cube;
    [SerializeField] TextMeshProUGUI text;
    bool DebugOn;
    void Start(){
        DebugOn = PlayerPrefs.GetInt("DebugOn") == 1;
        if(!DebugOn){
            text.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if(DebugOn){
        text.text = $"Rot:{Values.Rotation.eulerAngles}"+
                    $"\nDeltaRot:{Values.localDeltaRotation}"+
                    $"\nHittingPart:{Values.HittingPart}"+
                    $"\nCub:{Cube.eulerAngles}"+
                    $"\nFps:{1.0f / Time.deltaTime}"+
                    $"\nSec:{Time.time}"+
                    $"\nBPM:{settings.BPM}"+
                    $"\nScore:{settings.Score}"+
                    $"\nbeat:{settings.BeatCounter}:{settings.SubBeatCounter % 4}"+
                    $"\nheatMode:{settings.HeatMode}"+
                    $"\nVer:{Application.version}";
        }
    }
}
