using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameSwitchmanager : MonoBehaviour
{
    private MenuLevelsManager.TrackInfo _info;
    void Start(){
        SceneManager.activeSceneChanged += ChangedActiveScene;
    }
    public void Play(MenuLevelsManager.TrackInfo Info){
        _info = Info;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("MainGame");
    }
    private void ChangedActiveScene(Scene current, Scene next)
    {

        if(SceneManager.GetActiveScene().name == "MainGame"){
            GameObject Setup = GameObject.Find("!GameSetup");
            GameSetupReceiver GSR = Setup.GetComponent<GameSetupReceiver>();
            GSR.ReceiveData(_info);
        }
    }
}
