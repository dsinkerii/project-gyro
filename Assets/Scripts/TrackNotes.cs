using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class TrackNotes : MonoBehaviour
{
    [System.Serializable]
    public struct Note{
        public int beatTime;
        public int subBeatTime;
        public IncomingPartPath.Direction direction;
    };
    [Header("Please make sure that:\n- subBeatTime is >= 0 and < 16;\n- every note is ordered properly\n  so that one note cant have\n  a beatTime lower than previous.")]
    public List<Note> notes;

    public static void SaveNotes(string FilePath, List<Note> _notes)
    {
        string json = JsonUtility.ToJson(new NoteListWrapper { notes = _notes });
        File.WriteAllText(FilePath, json);
        print($"Notes saved to {FilePath}");
    }

    public static List<Note> LoadNotes(string FilePath)
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            NoteListWrapper wrapper = JsonUtility.FromJson<NoteListWrapper>(json);
            print($"Notes loaded from {FilePath}");
            return wrapper.notes;
        }
        else
        {
            print($"No notes found at {FilePath}");
            return null;
        }
    }

    [System.Serializable]
    private class NoteListWrapper
    {
        public List<Note> notes;
    }
}
