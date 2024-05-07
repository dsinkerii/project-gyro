using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupReceiver : MonoBehaviour
{
    [SerializeField] TrackSettings settings;
    [SerializeField] TrackNotes trackNotes;
    [SerializeField] Material RightNote;
    [SerializeField] Material UpNote;
    [SerializeField] Material LeftNote;
    [SerializeField] Material DownNote;
    [SerializeField] Material RollNote;
    [SerializeField] Material BG;
    public void ReceiveData(MenuLevelsManager.TrackInfo Info, float volume = 0.149f)
    {
        print("Getting Data..");
        settings.TrackName = Info.name;
        settings.TrackID = Info.ID;

        settings.music.volume = volume;
        settings.BPM = Info.bpm;
        settings.TrackStartOffset = Info.trackStartOffset;
        trackNotes.notes = Info.notes;

        RightNote.SetColor("_Color", Info.rightColor);
        UpNote.SetColor("_Color", Info.upColor);
        LeftNote.SetColor("_Color", Info.leftColor);
        DownNote.SetColor("_Color", Info.downColor);
        RollNote.SetColor("_Color", Info.rollColor);
        BG.SetColor("_Color", Info.bar1Color);
        BG.SetColor("_Color2", Info.bar2Color);
        settings.ToColorHeat = Info.heatModeColor;

        StartCoroutine(LoadAudio(Info.trackAudio, Info.name));

        print("Data received! (Part 1)");
    }

    //https://discussions.unity.com/t/get-audioclip-from-path-how-to-get-audioclip-from-a-path/251327/2
    private IEnumerator LoadAudio(string soundPath,string name)
    {
        WWW request = GetAudioFromFile($"file://{soundPath}");
        yield return request;
        AudioClip audioClip = request.GetAudioClip();
        audioClip.name = name;
        settings.music.clip = audioClip;
        print("Data received! (Part 2)");
    }

    private WWW GetAudioFromFile(string path)
    {
        string audioToLoad = string.Format(path);
        WWW request = new WWW(audioToLoad);
        return request;
    }
}

