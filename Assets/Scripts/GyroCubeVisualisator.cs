using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCubeVisualisator : MonoBehaviour
{
    [SerializeField] GyroValues Values;
    [SerializeField] Material CubeMat;
    [SerializeField] float lerpVal = 0.2f;
    [SerializeField] Transform UICube;
    [SerializeField] MeshRenderer Outline;
    bool isOn;
    void Start(){
        isOn = PlayerPrefs.GetInt("CubeOn") == 1 ? true : false;
        if(isOn){
            GetComponent<MeshRenderer>().enabled = true;
            Outline.enabled = true;
        }else{
            GetComponent<MeshRenderer>().enabled = false;
            Outline.enabled = false;
        }
    }

    void Update(){
        
        UICube.rotation = Quaternion.Lerp(UICube.rotation,
                                          Quaternion.Euler(Values.localDeltaRotation * -10.0f),
                                          lerpVal*Time.deltaTime*10); // currentRot * Quaternion.Inverse(Values.Rotation) means just subtracting them
        if(isOn){
            float xVal = UICube.eulerAngles.y / 90;
            float yVal = UICube.eulerAngles.x / 90;
            float zVal = UICube.eulerAngles.z / 90;

            // simple fix for negative values... thats it
            if (xVal > 2)
                xVal -= 4;

            if (yVal > 2)
                yVal -= 4;

            if (zVal > 2)
                zVal -= 4;
            
            zVal = Mathf.Abs(zVal);
            CubeMat.SetFloat("_xMult",xVal*2);

            CubeMat.SetFloat("_yMult",yVal*2);

            CubeMat.SetFloat("_zMult",zVal*4);
        }
    }
}