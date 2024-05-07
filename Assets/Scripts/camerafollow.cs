using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    [SerializeField] GyroValues Values;
    [SerializeField] float lerpVal = 0.1f;
    void Start(){
        Application.targetFrameRate = 60;
    }

    void LateUpdate()
    {
        Vector3 fixedVec = new Vector3(Values.localDeltaRotation.y, Values.localDeltaRotation.x, Values.localDeltaRotation.z/10);
        Vector3 fixedRotVec = new Vector3(Values.localDeltaRotation.x, -Values.localDeltaRotation.y, Values.localDeltaRotation.z);

        transform.rotation = Quaternion.Lerp(transform.rotation,
                                          Quaternion.Euler(fixedRotVec*2.0f),
                                          lerpVal);
        transform.position = Vector3.Lerp(transform.position, fixedVec/2.5f+new Vector3(0,0,-10f),lerpVal);
    }
}
