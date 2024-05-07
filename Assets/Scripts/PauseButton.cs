using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class PauseButton : MonoBehaviour
{
    [SerializeField] TrackSettings settings;
    [SerializeField] Image PauseImage;
    [SerializeField] Image PauseImageIcon;
    [SerializeField] TextMeshProUGUI PauseImageText;
    [SerializeField] Color FromColor;
    [SerializeField] Color ToColor;
    
    bool Toggle;
    public void Pause(){
        if(settings.TrackStarted){
            Toggle = !Toggle;
            settings.PauseTrack(Toggle);
            if(Toggle){
                if(Application.platform != RuntimePlatform.Android){
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }else{
                if(Application.platform != RuntimePlatform.Android){
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Confined;
                }
            }
        }
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
        if(Toggle){
            PauseImage.color = Color.Lerp(PauseImage.color, ToColor, 0.2f);
            PauseImageIcon.color = Color.Lerp(PauseImageIcon.color, new Color(1,1,1,ToColor.a), 0.2f);
            PauseImageText.color = Color.Lerp(PauseImageText.color, new Color(1,1,1,ToColor.a), 0.2f);
            PauseImage.gameObject.SetActive(true);
        }else{
            PauseImage.color = Color.Lerp(PauseImage.color, FromColor, Time.deltaTime*3.5f);
            PauseImageIcon.color = Color.Lerp(PauseImageIcon.color, new Color(1,1,1,FromColor.a), Time.deltaTime*3.5f);
            PauseImageText.color = Color.Lerp(PauseImageText.color, new Color(1,1,1,FromColor.a), Time.deltaTime*3.5f);
            if(PauseImage.color.a < 0.1f){
                PauseImage.gameObject.SetActive(false);
            }
        }
    }
}
