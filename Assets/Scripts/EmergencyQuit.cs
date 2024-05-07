using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EmergencyQuit : MonoBehaviour
{
    public void emergencyQuit(){
        SceneManager.LoadScene("MainMenu");
    }
}
