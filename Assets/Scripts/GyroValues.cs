using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroValues : MonoBehaviour
{
    public Quaternion Rotation;
    public Quaternion deltaRotation;
    public Vector3 localDeltaRotation;
    public Vector3 localDeltaSmoothRotation;
    public Vector3 direction;
    public float Threshold = 5;
    public Vector3 HittingPart;
    public float LerpSpeed = 0.5f;
    [SerializeField] bool Settings = false;

    void Start(){
        Input.gyro.enabled = true;
        if(!Settings){
            if(Application.platform != RuntimePlatform.Android){
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        LerpSpeed = PlayerPrefs.GetInt("lerpSpeed") == 0 ? 0.5f : 1;
    }

    private Vector3 BinaryVector(Vector3 input){
        Threshold = PlayerPrefs.GetInt("Threshold");
        return new Vector3(
            input.x > Threshold ? 1 : 0-input.x > Threshold ? -1 : 0,
            input.y > Threshold ? 1 : 0-input.y > Threshold ? -1 : 0,
            input.z > Threshold ? 1 : 0-input.z > Threshold ? -1 : 0
            );
    }

    void OnGUI(){
        if(Rotation != Input.gyro.attitude){
            deltaRotation = Input.gyro.attitude * Quaternion.Inverse(Rotation);
            Vector3 direction = Input.gyro.attitude * Vector3.forward;
        }
        Rotation = Input.gyro.attitude;
        if(Application.platform == RuntimePlatform.Android)
            localDeltaRotation = Input.gyro.rotationRateUnbiased* PlayerPrefs.GetInt("Intensity") * 0.1f;
        else{
            localDeltaRotation = Vector3.Lerp(localDeltaRotation,new Vector3(-Input.GetAxis("Mouse Y")*10,Input.GetAxis("Mouse X")*10,Input.mouseScrollDelta.y*50),Time.deltaTime*10)* PlayerPrefs.GetInt("Intensity") * 0.1f;
        }

        localDeltaSmoothRotation = Vector3.Lerp(localDeltaSmoothRotation, localDeltaRotation,10*Time.deltaTime * LerpSpeed);
        
        HittingPart = BinaryVector(localDeltaSmoothRotation);
    }
}

