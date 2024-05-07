using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameFinish : MonoBehaviour
{
    [SerializeField] Transform GradientEnd;
    [SerializeField] GameObject Results;
    [SerializeField] TextMeshProUGUI PlayerResults;
    [SerializeField] TextMeshProUGUI TrackSettings;
    [SerializeField] AudioSource Key;
    [SerializeField] Image PlayerRImage;
    [SerializeField] Image TrackSImage;
    [SerializeField] Color ColorTo;
    [SerializeField] Color ColorFromTS;
    [SerializeField] TrackSettings settings;
    [SerializeField] Image TapToExit;
    public void GameEnd(){
        if(Application.platform != RuntimePlatform.Android){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if(PlayerPrefs.GetInt($"PB:{settings.TrackID}") < settings.Score){
            PlayerPrefs.SetInt($"PB:{settings.TrackID}",settings.Score);
        }
        PlayerPrefs.SetInt($"LAST:{settings.TrackID}",settings.Score);
        StartCoroutine(coroutine());
    }

    IEnumerator coroutine(){
        GradientEnd.gameObject.SetActive(true);
        GradientEnd.localPosition = new Vector3(0,-1000,0);
        for(int i = 0; i < 100; i++){
            GradientEnd.localPosition = new Vector3(0,Mathf.SmoothStep(-1000,1000,i/100f),0);
            yield return new WaitForSeconds(0.02f);
        }
        yield return StartCoroutine(ShowResultsCard());
        yield return StartCoroutine(ShowPlayerResultsCard());
        yield return StartCoroutine(ShowTrackSettingsCard());
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(TapToExitCoroutine());
    }
    IEnumerator ShowResultsCard(){
        Results.transform.localEulerAngles = new Vector3(0,0,45f);
        Results.transform.localPosition = new Vector3(0,-500,0);
        Results.SetActive(true);
        for(int i = 0; i < 50; i++){
            Results.transform.localPosition = new Vector3(0,Mathf.SmoothStep(-500,0,i/50f),0);
            Results.transform.localEulerAngles = new Vector3(0,0,Mathf.SmoothStep(45,0,i/50f));
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ShowPlayerResultsCard(){
        Vector3 FromPos = PlayerRImage.transform.localPosition;
        PlayerRImage.transform.localPosition = new Vector3(FromPos.x,FromPos.y+500,FromPos.z);

        PlayerResults.text = " ";
        PlayerRImage.gameObject.SetActive(true);
        for(int i = 0; i < 50; i++){
            PlayerRImage.transform.localPosition = new Vector3(FromPos.x,Mathf.SmoothStep(FromPos.y,FromPos.y-500,i/50f),FromPos.z);
            yield return new WaitForSeconds(0.01f);
        }
        string buffer = ""; // keep old stuff from the text here
        Key.Play();

        //score
        for(int i = 0; i < 50; i++){
            // fancy colors
            PlayerResults.text = $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(ColorTo,Color.white, i/25f))}>score</color>:\n {Mathf.Round(Mathf.SmoothStep(0,settings.Score,i/50f))}";
            yield return new WaitForSeconds(0.01f);
        }
        buffer+=$"score:\n{settings.Score}\n\n";
        //notes
        yield return new WaitForSeconds(0.5f);
        Key.Play();
        for(int i = 0; i < 50; i++){
            // fancy colors
            PlayerResults.text = buffer +
            $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(ColorTo,Color.white, i/25f))}>notes hit</color>:\n {Mathf.Round(Mathf.SmoothStep(0,settings.NotesHit,i/50f))}";
            yield return new WaitForSeconds(0.01f);
        }
        buffer+=$"notes hit:\n{settings.NotesHit}\n\n";
        //notesheatmode
        yield return new WaitForSeconds(0.5f);
        Key.Play();
        for(int i = 0; i < 50; i++){
            // fancy colors
            PlayerResults.text = buffer +
            $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(ColorTo,Color.white, i/25f))}>heat mode notes</color>:\n {Mathf.Round(Mathf.SmoothStep(0,settings.NotesHitInHeatMode,i/50f))}";
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ShowTrackSettingsCard(){
        Vector3 FromPos = TrackSImage.transform.localPosition;
        TrackSImage.transform.localPosition = new Vector3(FromPos.x,FromPos.y+500,FromPos.z);

        TrackSettings.text = " ";
        TrackSImage.gameObject.SetActive(true);
        for(int i = 0; i < 50; i++){
            TrackSImage.transform.localPosition = new Vector3(FromPos.x,Mathf.SmoothStep(FromPos.y,FromPos.y-500,i/50f),FromPos.z);
            yield return new WaitForSeconds(0.01f);
        }
        string buffer = ""; // keep old stuff from the text here
        Key.Play();

        //name
        for(int i = 0; i < 50; i++){
            // fancy colors
            TrackSettings.text = $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(Color.white,ColorFromTS, i/25f))}>track name</color>:\n {settings.TrackName}";
            yield return new WaitForSeconds(0.01f);
        }
        buffer+=$"track name:\n{settings.TrackName}\n\n";
        //BPM
        yield return new WaitForSeconds(0.5f);
        Key.Play();
        for(int i = 0; i < 50; i++){
            // fancy colors
            TrackSettings.text = buffer +
             $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(Color.white,ColorFromTS, i/25f))}>BPM</color>:\n {settings.BPM}";
            yield return new WaitForSeconds(0.01f);
        }
        buffer+=$"BPM:\n{settings.BPM}\n\n";
        //notesheatmode
        yield return new WaitForSeconds(0.5f);
        Key.Play();
        for(int i = 0; i < 50; i++){
            // fancy colors
            TrackSettings.text = buffer +
             $"<color=#{ColorUtility.ToHtmlStringRGB(Color.Lerp(Color.white,ColorFromTS, i/25f))}>notes in total</color>:\n {settings.TotalNotes}";
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator TapToExitCoroutine(){
        TapToExit.gameObject.SetActive(true);
        for(int i = 0; i < 50; i++){
            TapToExit.color = new Color(1,1,1,Mathf.SmoothStep(0,0.1f,i/50f));
            yield return new WaitForSeconds(0.01f);
        }
    }
    public void ToMenu(){
        SceneManager.LoadScene("MainMenu");
    }
}
