using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void OpenLinkInBrowser(string link){
        Application.OpenURL(link);
    }
}
