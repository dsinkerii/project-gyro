using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TitleCardOffset : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upper;
    [SerializeField] TextMeshProUGUI middle;
    [SerializeField] TextMeshProUGUI lower;
    [SerializeField] string Name;
    void Update(){
        string processedName = "";
        int index = 0;
        foreach(char c in Name){
            index++;
            processedName += $"<voffset={Mathf.Sin(index/3f+Time.time)/5f}em>{c}</voffset>";
        }
        upper.text = processedName;
        middle.text = processedName;
        lower.text = processedName;
    }
}
