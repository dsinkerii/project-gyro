using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForGyro : MonoBehaviour
{
    void Start(){
        gameObject.SetActive(!SystemInfo.supportsGyroscope);
    }
}
