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
        public Texture2D thumbnail; 
        public string trackAudio;
        public List<TrackNotes.Note> notes;
        public float trackStartOffset;
        public string ID;
        public int Version;
    }
    [SerializeField] GameObject TrackPlaceholder;
    [SerializeField] GameObject DownloadedTracks;
    [SerializeField] ScaleContentHeightByGrid ContentGrid;
    [Header("Track display stuffs")]
    [SerializeField] GameObject SelectedTrackObject;
    [SerializeField] GameObject TrackView;
    [SerializeField] GameObject Pointer;
    [SerializeField] RawImage imageDisplay;
    [SerializeField] TextMeshProUGUI NameDisplay;
    [SerializeField] TextMeshProUGUI NameDesc;
    private Transform PointerLerpPos;
    public List<TrackInfo> tracks = new List<TrackInfo>();
    public int SelectedId;
    public List<GameObject> GridTracks = new List<GameObject>();
    [Header("Track download stuff")]
    [SerializeField] GameObject StatusCodeOBJ;
    [SerializeField] TextMeshProUGUI StatusCodeText;
    void Start(){
        TrackView.SetActive(false);
        Refresh();
    }
    void FixedUpdate(){
        if(PointerLerpPos != null){
            Pointer.transform.position = Vector3.Lerp(Pointer.transform.position, PointerLerpPos.position, 0.2f);
        }
    }
    public void Refresh(){
        foreach(Transform child in DownloadedTracks.transform){
            if(child.gameObject.active)
                Destroy(child.gameObject);
        }
        string[] trackFolders = Directory.GetDirectories(Application.persistentDataPath);
        print($"Searching: {Application.persistentDataPath}");
        foreach (string folder in trackFolders){
            string subfolder = folder;
            while(Directory.GetDirectories(subfolder).Length != 0){
                subfolder = Directory.GetDirectories(subfolder)[0];
            }
            
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

                GameObject TrackGridElement = Instantiate(TrackPlaceholder);
                TrackGridElement.transform.SetParent(DownloadedTracks.transform);
                TrackGridElement.transform.localEulerAngles = TrackPlaceholder.transform.localEulerAngles;
                TrackGridElement.transform.localScale = TrackPlaceholder.transform.localScale;
                TrackGridElement.transform.localPosition = TrackPlaceholder.transform.localPosition;
                TrackGridElement.name = track.name;
                GridTracks.Add(TrackGridElement);

                GridTrackUI GTUI = TrackGridElement.GetComponent<GridTrackUI>();
                GTUI.Info = track;
                GTUI.manager = this;
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
    public void ShowOnDisplay(TrackInfo trackInfo){
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
    public void DownloadFile(TMP_InputField zipFileURL){
        if(zipFileURL.text.Length != 0)
            StartCoroutine(DownloadZipFile(zipFileURL.text));
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
        for(int i = 0; i < 100; i++){
            TrackView.transform.localPosition = new Vector3(
                TrackView.transform.localPosition.x,
                Mathf.Lerp(TrackView.transform.localPosition.y,0,0.08f),
                TrackView.transform.localPosition.z
            );
            yield return new WaitForSeconds(0.01f);
        }
    }
}