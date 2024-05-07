using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupReceiver : MonoBehaviour
{
    [SerializeField] TrackSettings settings;
    [SerializeField] TrackNotes trackNotes;
    public void ReceiveData(MenuLevelsManager.TrackInfo Info, float volume = 0.149f)
    {
        print("Getting Data..");
        settings.TrackName = Info.name;
        settings.TrackID = Info.ID;

        settings.music.volume = volume;
        settings.BPM = Info.bpm;
        settings.TrackStartOffset = Info.trackStartOffset;
        trackNotes.notes = Info.notes;

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
