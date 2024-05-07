using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] Image LowestLayerImage;
    [SerializeField] Image MiddleLayerImage;
    [SerializeField] Image UpperLayerImage;
    [SerializeField] Image IconImage;
    public float Opacity = 255f;
    public void OnPress(){
        StartCoroutine(OnPressCoroutine());
    }
    IEnumerator OnPressCoroutine(){
        transform.localScale = new Vector3(0.9f,0.9f,0.9f);
        for(int i = 0; i < 100; i++){
            float _val = Mathf.Lerp(transform.localScale.x, 1, 0.2f);
            transform.localScale = new Vector3(_val,_val,_val);
            yield return new WaitForSeconds(0.01f);
        }
        transform.localScale = new Vector3(1,1,1);
    }

    void Update(){
        LowestLayerImage.color = new Color(LowestLayerImage.color.r,LowestLayerImage.color.g,LowestLayerImage.color.b,Opacity);
        MiddleLayerImage.color = new Color(MiddleLayerImage.color.r,MiddleLayerImage.color.g,MiddleLayerImage.color.b,Opacity);
        UpperLayerImage.color = new Color(UpperLayerImage.color.r,UpperLayerImage.color.g,UpperLayerImage.color.b,Opacity);
        IconImage.color = new Color(IconImage.color.r,IconImage.color.g,IconImage.color.b,Opacity);
    }
}


