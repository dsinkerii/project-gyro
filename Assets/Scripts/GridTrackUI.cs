using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GridTrackUI : MonoBehaviour
{
    public MenuLevelsManager.TrackInfo Info;
    public MenuLevelsManager manager;
    public string folder;
    [SerializeField] TextMeshProUGUI _trackName;
    [SerializeField] TextMeshProUGUI _desc;
    [SerializeField] RawImage _thumbnail;
    [SerializeField] GameObject _errorOutline;
    public bool Error;
    public string ErrorText;
    public void LoadTrack(){
        if(!Error){
            _trackName.text = Info.name;
            _desc.text = $"By: {Info.author}\nLength: {Info.length}s\nBPM: {Info.bpm}";
            _thumbnail.texture = Info.thumbnail;
        }else{
            _desc.gameObject.SetActive(false);
            _errorOutline.SetActive(true);
            _trackName.text = ErrorText;
            _trackName.color = new Color32((byte)218, (byte)77, (byte)109,(byte)255);
        }
    }
    public void ToDisplay(){
        manager.ShowOnDisplay(Info, folder, Error);
    }
}