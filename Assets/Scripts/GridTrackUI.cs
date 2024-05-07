using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GridTrackUI : MonoBehaviour
{
    public MenuLevelsManager.TrackInfo Info;
    public MenuLevelsManager manager;
    [SerializeField] TextMeshProUGUI _trackName;
    [SerializeField] TextMeshProUGUI _desc;
    [SerializeField] RawImage _thumbnail;
    public void LoadTrack(){
        _trackName.text = Info.name;
        _desc.text = $"By: {Info.author}\nLength: {Info.length}s\nBPM: {Info.bpm}";
        _thumbnail.texture = Info.thumbnail;
    }
    public void ToDisplay(){
        manager.ShowOnDisplay(Info);
    }
}