using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class fpsCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    bool DebugOn;
    void Start(){
        DebugOn = PlayerPrefs.GetInt("DebugOn") != 0;
        if(!DebugOn){
            text.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if(DebugOn){
        text.text = $"\nFps:{1.0f / Time.deltaTime}"+
                    $"\nVer:{Application.version}";
        }
    }
}
