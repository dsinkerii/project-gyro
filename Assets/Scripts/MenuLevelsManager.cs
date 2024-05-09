using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Net;
using System.IO.Compression;
using UnityEngine.Networking;
public class MenuLevelsManager : MonoBehaviour
{
    [System.Serializable]
    public class TrackInfo{
        public string name;
        public int length;
        public int bpm;
        public string author;
        public float trackStartOffset;
        public int Version;
        public Color32 rightColor = new Color32(77,218,185,255);
        public Color32 upColor = new Color32(180,218,77,255);
        public Color32 leftColor = new Color32(218,77,109,255);
        public Color32 downColor = new Color32(115,77,218,255);
        public Color32 rollColor = new Color32(218,150,77,255);
        public Color32 heatModeColor = new Color32(49,48,102,255);
        public Color32 bar1Color = new Color32(133,239,255,30);
        public Color32 bar2Color = new Color32(18,84,142,255);
        [Header("DO NOT write to the track.json, or i will bite your ears off")]
        public Texture2D thumbnail; 
        public string trackAudio;
        public string ID;
        public List<TrackNotes.Note> notes;
    }
    [SerializeField] GameObject TrackPlaceholder;
    [SerializeField] GameObject DownloadedTracks;
    [SerializeField] ScaleContentHeightByGrid ContentGrid;
    [Header("Track display stuffs")]
    [SerializeField] GameObject SelectedTrackObject;
    [SerializeField] GameObject TrackView;
    [SerializeField] GameObject Pointer;
    [SerializeField] GameObject DeleteTrack;
    [SerializeField] RawImage imageDisplay;
    [SerializeField] TextMeshProUGUI NameDisplay;
    [SerializeField] TextMeshProUGUI NameDesc;
    private Transform PointerLerpPos;
    public Toggle DeleteMode;
    public List<TrackInfo> tracks = new List<TrackInfo>();
    public int SelectedId;
    public List<GameObject> GridTracks = new List<GameObject>();
    [Header("Track download stuff")]
    [SerializeField] GameObject StatusCodeOBJ;
    [SerializeField] GameObject downloadDefault;
    [SerializeField] TextMeshProUGUI StatusCodeText;
    void Start(){
        TrackView.SetActive(false);
        Refresh();
        if(tracks.Count == 0){
            downloadDefault.SetActive(true);
        }
        //SaveToFolder("cock", tracks[0]); -- ignore that please
    }
    void FixedUpdate(){
        if(PointerLerpPos != null){
            Pointer.transform.position = Vector3.Lerp(Pointer.transform.position, PointerLerpPos.position, 0.2f);
        }
    }
    public static void SaveToFolder(string name, TrackInfo info){ // todo
        string path = Path.Combine(Application.persistentDataPath, name);
        Directory.CreateDirectory(path);

        string jsonNotesInfo = JsonUtility.ToJson(info.notes);
        File.WriteAllText(Path.Combine(path, "notes.json"), jsonNotesInfo);

        info.notes = null;
        info.trackAudio = null;
        info.thumbnail = null;
        info.ID = null;
        string jsonInfo = JsonUtility.ToJson(info);
        File.WriteAllText(Path.Combine(path, "track.json"), jsonInfo);
    }
    public void Refresh(){
        foreach(Transform child in DownloadedTracks.transform){
            if(child.gameObject.active)
                Destroy(child.gameObject);
        }
        string[] trackFolders = Directory.GetDirectories(Application.persistentDataPath);
        print($"Searching: {Application.persistentDataPath}");
        foreach (string folder in trackFolders){
            GameObject TrackGridElement;
            TrackGridElement = Instantiate(TrackPlaceholder);
            TrackGridElement.transform.SetParent(DownloadedTracks.transform);
            TrackGridElement.transform.localEulerAngles = TrackPlaceholder.transform.localEulerAngles;
            TrackGridElement.transform.localScale = TrackPlaceholder.transform.localScale;
            TrackGridElement.transform.localPosition = TrackPlaceholder.transform.localPosition;

            GridTrackUI GTUI = TrackGridElement.GetComponent<GridTrackUI>();
            string subfolder = folder;
            while(Directory.GetDirectories(subfolder).Length != 0){
                subfolder = Directory.GetDirectories(subfolder)[0];
            }
            GTUI.folder = subfolder;

            try{   
                string trackInfoPath = Path.Combine(subfolder, "track.json");
                if (File.Exists(trackInfoPath)){
                    string json = File.ReadAllText(trackInfoPath);
                    TrackInfo track = JsonUtility.FromJson<TrackInfo>(json);
                    string thumbnailPath = Path.Combine(subfolder, "thumbnail.png");
                    if (File.Exists(thumbnailPath)){
                        byte[] thumbnailBytes = File.ReadAllBytes(thumbnailPath);
                        Texture2D thumbnailTexture = new Texture2D(160, 90);
                        thumbnailTexture.LoadImage(thumbnailBytes);
                        track.thumbnail = thumbnailTexture;
                    }

                    track.notes = TrackNotes.LoadNotes(Path.Combine(subfolder, "notes.json"));
                    track.trackAudio = Path.Combine(subfolder,"track.wav");
                    track.ID = Path.GetFileName(subfolder.TrimEnd(Path.DirectorySeparatorChar));
                    tracks.Add(track);

                    TrackGridElement.name = track.name;
                    GridTracks.Add(TrackGridElement);

                    GTUI.Info = track;
                    GTUI.manager = this;
                    GTUI.LoadTrack();

                    TrackGridElement.SetActive(true);
                }else{
                    Destroy(TrackGridElement);
                }
            }catch (System.Exception e) {
                GTUI.Error = true;
                GTUI.ErrorText = e.Message;
                GTUI.LoadTrack();
                TrackGridElement.SetActive(true);
            } 
        }
        ContentGrid.UpdateHeight(GridTracks.Count);
    }
    int SearchTrackID(TrackInfo trackInfo){
        int index = 0;
        foreach(TrackInfo track in tracks){
            if(track == trackInfo){
                return index;
            }
            index++;
        }
        return -1;
    }
    string folderToDelete;
    public void ShowOnDisplay(TrackInfo trackInfo, string folder, bool IsError){
        if(!DeleteMode.isOn){
            if(!IsError){
                StartCoroutine(DisplayDrop());
                SelectedId = SearchTrackID(trackInfo);
                SelectedTrackObject = GridTracks[SelectedId];
                PointerLerpPos = SelectedTrackObject.transform;
                TrackView.SetActive(true);
                Pointer.SetActive(true);
                imageDisplay.texture = trackInfo.thumbnail;
                NameDisplay.text = trackInfo.name;
                NameDesc.text = $"PB:{PlayerPrefs.GetInt($"PB:{trackInfo.ID}")}\nLast score:{PlayerPrefs.GetInt($"LAST:{trackInfo.ID}")}\nBy: {trackInfo.author}\nLength: {trackInfo.length}s\nBPM: {trackInfo.bpm}";
            }
        }else{
            folderToDelete = folder;
            DeleteTrack.SetActive(true);
        }
    }
    public void deleteIt(){
        if(folderToDelete != null){
            Directory.Delete(folderToDelete, true);
            DeleteMode.isOn = false;
            DeleteTrack.SetActive(false);
            Refresh();
        }
    }
    public void DownloadFile(TMP_InputField zipFileURL){
        if(zipFileURL.text.Length != 0)
            StartCoroutine(DownloadZipFile(zipFileURL.text));
    }
    public void DefaultDownload(TMP_InputField zipFileURL){
        StartCoroutine(DownloadZipFileDefault(zipFileURL));
    }
    IEnumerator DownloadZipFileDefault(TMP_InputField zipFileURL){
        zipFileURL.interactable = false;
        downloadDefault.SetActive(false);
        zipFileURL.text = "https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/RAVEKINGRAVE.zip";
        yield return StartCoroutine(DownloadZipFile("https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/RAVEKINGRAVE.zip"));
        zipFileURL.text = "https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/Chartreuse.Green.zip";
        yield return StartCoroutine(DownloadZipFile("https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/Chartreuse.Green.zip"));
        zipFileURL.text = "https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/Bad.apple.zip";
        yield return StartCoroutine(DownloadZipFile("https://github.com/dsinkerii/project-gyro/releases/download/PRE-1.1/Bad.apple.zip"));
        StatusCodeText.text = "downloaded 3 tracks!";
        zipFileURL.interactable = true;
    }
    IEnumerator DownloadZipFile(string zipFileURL){
        StatusCodeOBJ.SetActive(false);
        int random = Random.Range(0,1073741823);
        string zipFilePath = Path.Combine(Application.persistentDataPath,$"{random}.zip");
        while(File.Exists(zipFilePath)){
            random = Random.Range(0,1073741823);
            zipFilePath = Path.Combine(Application.persistentDataPath,$"{random}.zip");
        }
        UnityWebRequest ZipRequest = new UnityWebRequest(zipFileURL);
        ZipRequest.downloadHandler = new DownloadHandlerBuffer();

        StatusCodeOBJ.SetActive(true);
        StatusCodeText.color = Color.white;
        StatusCodeText.text = $"downloading...";
        yield return ZipRequest.SendWebRequest();

        System.IO.File.WriteAllBytes(zipFilePath, ZipRequest.downloadHandler.data);

        StatusCodeText.text = $"{ZipRequest.responseCode}";
        StatusCodeText.color = 
            StatusCodeText.text == "200" ? new Color32((byte)150,(byte)216,(byte)17, (byte)255) : 
            StatusCodeText.text == "404" ? new Color32((byte)255,(byte)179,(byte)11, (byte)255) : 
                                           new Color32((byte)255,(byte)35,(byte)11, (byte)255);

        if (File.Exists(zipFilePath)){
            ZipFile.ExtractToDirectory(zipFilePath, Path.Combine(Application.persistentDataPath,$"{random}"));
            File.Delete(zipFilePath);
        }
        else{
            Debug.LogError("Failed to download the zip file.");
        }
    }
    IEnumerator DisplayDrop(){
        TrackView.transform.localPosition = new Vector3(TrackView.transform.localPosition.x,-50,TrackView.transform.localPosition.z);
        float StartTime = Time.time;
        while(Time.time - StartTime < 1){
            TrackView.transform.localPosition = new Vector3(
                TrackView.transform.localPosition.x,
                Mathf.Lerp(TrackView.transform.localPosition.y,0,8f*Time.deltaTime),
                TrackView.transform.localPosition.z
            );
            yield return 1;
        }
    }
}