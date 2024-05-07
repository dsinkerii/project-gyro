using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GetReady : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upperText;
    [SerializeField] TextMeshProUGUI lowerText;
    [SerializeField] Color[] BGColors;
    [SerializeField] int readyStage;
    [SerializeField] Material GetReadyMaterials;
    [SerializeField] TrackSettings settings;
    [SerializeField] Image image;
    [SerializeField] AudioSource Ready;
    [SerializeField] AudioSource Go;

    void Start()
    {
        GetReadyMaterials.SetColor("_Color", BGColors[0]);
        StartCoroutine(TextChanger());
    }
    float lerpTime = 0f;
    [SerializeField] float t = 0f;

    void Update(){
        lerpTime += Time.deltaTime;
        
        t = lerpTime / (60f / settings.BPM*2);
        lowerText.transform.localPosition = Vector3.Lerp(lowerText.transform.localPosition, Vector3.zero,10*Time.deltaTime);
        
        switch(readyStage){
            case 4:
                GetReadyMaterials.SetColor("_Color", Color.Lerp(BGColors[0], BGColors[1], t));
                break;
            case 3:
                GetReadyMaterials.SetColor("_Color", Color.Lerp(BGColors[1], BGColors[2], t-1));
                break;
            case 2:
                GetReadyMaterials.SetColor("_Color", Color.Lerp(BGColors[2], BGColors[3], t-2));
                break;
            case 1:
                transform.localScale = Vector3.Lerp(transform.localScale,new Vector3(1,1,1), t-3);
                lowerText.transform.localPosition = new Vector3(Random.Range(-5,5),Random.Range(-5,5),Random.Range(-5,5));
                break;
            case 0:
                GetReadyMaterials.SetColor("_Color", Color.Lerp(BGColors[3], BGColors[4], t-4));
                image.color = Color.Lerp(BGColors[5], BGColors[6], t-4);
                break;
        }
    }

    IEnumerator TextChanger(){
        upperText.text = "3";
        lowerText.text = upperText.text;
        transform.localScale = new Vector3(1.2f,1.2f,1.2f);
        lowerText.transform.localPosition = new Vector3(0,-20,0);
        readyStage = 4;
        Ready.Play();
        yield return new WaitForSeconds(60f / settings.BPM*2);
        upperText.text = "2";
        lowerText.text = upperText.text;
        transform.localScale = new Vector3(1.3f,1.3f,1.3f);
        lowerText.transform.localPosition = new Vector3(0,-20,0);
        readyStage = 3;
        Ready.Play();
        yield return new WaitForSeconds(60f / settings.BPM*2);
        upperText.text = "1";
        lowerText.text = upperText.text;
        transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        lowerText.transform.localPosition = new Vector3(0,-20,0);
        readyStage = 2;
        Ready.Play();
        yield return new WaitForSeconds(60f / settings.BPM*2);
        upperText.text = "GO!!";
        lowerText.text = upperText.text;
        lowerText.transform.localPosition = new Vector3(0,-20,0);
        readyStage = 1;
        Go.Play();
        yield return new WaitForSeconds(60f / settings.BPM*2);
        readyStage = 0;
        upperText.text = "";
        lowerText.text = upperText.text;
        settings.StartTrack();
        yield return new WaitForSeconds(60f / settings.BPM*2);
        gameObject.SetActive(false);
    }
}
