using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingPartPath : MonoBehaviour
{
    public float Time;
    public TrackSettings settings;
    public enum Direction {UP, RIGHT, DOWN, LEFT, ROLL};
    double spawnTime;
    public double PauseTime;
    private double LastPauseTime;
    bool IsOnPause;

    public Direction Dir;
    Vector3 GetPathPos(float time){
        Vector3 pos;
        time = time/100;
        pos = new Vector3(
            0f,
            Mathf.LerpUnclamped(-108f,0,1-time*(time-2)-1), // y = 1-x*(x-2)-1
            Mathf.LerpUnclamped(127.5f,15.2f,time));
        return pos;
    }
    void Update()
    {
        transform.position = GetPathPos(Time);
        if(!IsOnPause && settings.Pause){
            LastPauseTime = settings.TimeDspFixed;
            IsOnPause = true;
        }else if (IsOnPause && !settings.Pause){
            PauseTime = settings.TimeDspFixed - LastPauseTime;
            IsOnPause = false;
        }
    }
    void Start(){
        spawnTime = settings.TimeDspFixed;
    }
    void FixedUpdate()
    {
        double elapsedTime = (settings.TimeDspFixed - PauseTime - spawnTime);

        double progress = elapsedTime / ((60f / settings.BPM) * 8); 

        Time = Mathf.LerpUnclamped(0f, 100f, (float)progress); // additional +25 for more time for the player
        // "solution" makes it 6 beats to arrive

        if(Time > 150){
            Destroy(this.gameObject);
        }
    }
}
