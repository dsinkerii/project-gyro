using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleContentHeightByGrid : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    public void UpdateHeight(int GridSize){
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, GridSize * 138.2f+37);
    }
}
