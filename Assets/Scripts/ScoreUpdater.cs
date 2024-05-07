using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreUpdater : MonoBehaviour
{
    [SerializeField] TextMeshPro score;
    [SerializeField] TrackSettings settings;
    Vector3 InitialPosition;
    float beatInterval;
    double currentTime;
    void Start(){
        InitialPosition = transform.position;
        beatInterval = 60f / settings.BPM;
    }
    void Update(){
        if(!settings.HeatMode){
            transform.position = InitialPosition;
            score.text = $"score: {settings.Score}";
        }else{
            currentTime = AudioSettings.dspTime;
            transform.position = InitialPosition + new Vector3(0, Mathf.Abs(Mathf.Sin((float)((currentTime - settings.lastBeatTime) / beatInterval)*2))*1.2f,0);
            score.text = $"score: {settings.Score}\n<size=3.5>heat mode!!</size>";
        }
    }
}
