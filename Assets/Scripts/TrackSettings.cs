using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSettings : MonoBehaviour
{
    [Header("Actual track settings.")]
    public int BPM;
    public string TrackName;
    public string TrackID;
    public AudioSource music;
    public float TrackStartOffset;
    public int TotalNotes;
    [SerializeField] TrackNotes trackNotes;
    public bool HeatMode;

    [Header("Scene BG.")]
    [SerializeField] GameObject Level;
    [SerializeField] Material Background;
    [SerializeField] Color FromColorHeat;
    [SerializeField] Color ToColorHeat;
    Color _currColorHeat;
    float[] spectrum = new float[64];
    float[] spectrumBGData = new float[10];
    [SerializeField] float LerpSpeed = 0.15f;
    [SerializeField] float Multiplier = 200;
    [SerializeField] TrackNotes.Note nextNote;
    int nextFrameUpdate = 3; // update once in 3 frames

    [Header("Read Only.")]
    public int BeatCounter;
    public int SubBeatCounter;
    public int Score;
    public int NotesHit;
    public int NotesHitInHeatMode;
    public bool IsTrackPlaying;
    private float beatInterval;
    public double lastBeatTime;
    private double lastSubBeatTime;
    public bool TrackStarted;
    public double TimeDspFixed;
    private double TimeDspLastPause;
    [SerializeField] GameFinish finish;
    [Header("Note Objects.")]
    [SerializeField] GameObject NoteUp;
    [SerializeField] GameObject NoteRight;
    [SerializeField] GameObject NoteDown;
    [SerializeField] GameObject NoteLeft;
    [SerializeField] GameObject NoteRoll;
    [SerializeField] Transform NoteParent;
    void Start(){
        beatInterval = 60f / BPM;
        lastBeatTime = AudioSettings.dspTime;
        lastSubBeatTime = AudioSettings.dspTime;

        TimeDspLastPause = TimeDspFixed;
        SubBeatCounter=-1; // setting this to -1 so that at the start of the script, the beat will be 1:0
        StartCoroutine(WaitForSound());
    }
    public bool Pause;
    void Update(){
        nextFrameUpdate = nextFrameUpdate < 0 ? 3 : nextFrameUpdate-1;
        if(nextFrameUpdate == 0){
            music.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        }
        for(int i = 0; i < 10; i++){
            spectrumBGData[i] = Mathf.Lerp(spectrumBGData[i], spectrum[Mathf.FloorToInt(i*6.4f)]*Multiplier,LerpSpeed);
            Background.SetFloat($"_val_{i}",spectrumBGData[i]);
        }
    }

    public void HeatModeToggle(bool Toggle){
        HeatMode = Toggle;
    }
    public void StartTrack(){
        if(trackNotes.notes.Count > 0){
            TotalNotes = trackNotes.notes.Count;
            nextNote = trackNotes.notes[0];
        }
        music.Play();
        TrackStarted = true;
        IsTrackPlaying = true;
        Level.SetActive(true);
        lastBeatTime = AudioSettings.dspTime-TrackStartOffset;
        lastSubBeatTime = AudioSettings.dspTime-TrackStartOffset;
    }
    public void PauseTrack(bool Toggle){
        if(Toggle){
            TimeDspLastPause = TimeDspFixed;
            if(TrackStarted)
                music.Pause();
            Time.timeScale = 0;
            IsTrackPlaying = false;
            Pause = true;
        }else{
            TimeDspLastPause = AudioSettings.dspTime - TimeDspLastPause;
            if(TrackStarted)
                music.Play();
            Time.timeScale = 1;
            IsTrackPlaying = true;
            Pause = false;
        }
    }
    void FixedUpdate()
    {
        TimeDspFixed = AudioSettings.dspTime - TimeDspLastPause;
        if(HeatMode){
            _currColorHeat = Color.Lerp(_currColorHeat, ToColorHeat, 0.1f);
        }else{
            _currColorHeat = Color.Lerp(_currColorHeat, FromColorHeat, 0.1f);
        }
        Background.SetColor("_heatcolor",_currColorHeat);
        if(TrackStarted && IsTrackPlaying){
            double currentTime = TimeDspFixed;

            Background.SetFloat("_beat",Mathf.Lerp(0.025f,0.01f, (float)((currentTime - lastBeatTime) / beatInterval)));

            if (currentTime - lastSubBeatTime >= (beatInterval/4))
            {
                //Sub-Beat!!
                SubBeatCounter++;
                lastSubBeatTime = currentTime;
            }
            if (currentTime - lastBeatTime >= beatInterval)
            {
                //Beat!!
                BeatCounter++;
                SubBeatCounter = 0;
                lastBeatTime = currentTime;
            }
            CheckAndSpawnNotes();
        }
    }

    void CheckAndSpawnNotes(){
        if(trackNotes.notes.Count > 0){
        if(nextNote.beatTime == BeatCounter && nextNote.subBeatTime == (SubBeatCounter%4)){
            trackNotes.notes.Remove(nextNote);
            //Note spawn
            GameObject SpawnNote;
            switch(nextNote.direction){
                case IncomingPartPath.Direction.UP:
                    SpawnNote = Instantiate(NoteUp);
                    break;
                case IncomingPartPath.Direction.RIGHT:
                    SpawnNote = Instantiate(NoteRight);
                    break;
                case IncomingPartPath.Direction.DOWN:
                    SpawnNote = Instantiate(NoteDown);
                    break;
                case IncomingPartPath.Direction.LEFT:
                    SpawnNote = Instantiate(NoteLeft);
                    break;
                default:
                    SpawnNote = Instantiate(NoteRoll);
                    break;
            }
            SpawnNote.GetComponent<IncomingPartPath>().settings = this;
            SpawnNote.transform.SetParent(NoteParent);
            SpawnNote.SetActive(true);

            if(trackNotes.notes.Count > 0)
                nextNote = trackNotes.notes[0];
        }
        }
    }
    public IEnumerator WaitForSound(){
       yield return new WaitUntil(() => music.time >= music.clip.length);
       finish.GameEnd();
    }
}
