using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class MainMenuScript : MonoBehaviour
{
    [Header("Title card objects")]
    [SerializeField] GameObject Upperbar;
    [SerializeField] GameObject Lowerbar;
    [SerializeField] GameObject TitleCard;
    [SerializeField] GameObject ContinueButton;
    [SerializeField] TextMeshProUGUI PressToContinue;

    [Header("Main menu objects")]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject MainMenuButtons;
    [SerializeField] GameObject UpperbarGradient;
    [SerializeField] GameObject LowerbarGradient;
    [Header("Main menu (game list) objects")]
    [SerializeField] GameObject ListObject;
    [Header("Settings")]
    [SerializeField] GameObject SettingsObject;
    [Header("Blackout image")]
    [SerializeField] Image blackout;
    [SerializeField] TextMeshProUGUI ExitText;
    [System.Serializable]
    struct exitText{
        public string Text;
        public int Weight; // rarity: higher value = more common
    }
    [SerializeField] List<exitText> ExitStrings;
    [SerializeField] GameSwitchmanager GSmanager;
    [SerializeField] MenuLevelsManager MLManager;
    float yVelocity = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 30;
        StartCoroutine(OnStartCoroutine());
    }

    public void ContinueTitleCard(){
        print("Continue");
        StartCoroutine(ContinueTitleCardCoroutine());
    }
    IEnumerator ContinueTitleCardCoroutine(){
        float StartTime = Time.time;
        while(Time.time - StartTime < 1){
            yield return 1;
            Upperbar.transform.localPosition = new Vector3(0,Mathf.Lerp(Upperbar.transform.localPosition.y,800,6f * Time.deltaTime),0);
            Lowerbar.transform.localPosition = new Vector3(0,Mathf.Lerp(Lowerbar.transform.localPosition.y,-800,6f * Time.deltaTime),0);
            TitleCard.transform.localPosition = new Vector3(Mathf.Lerp(TitleCard.transform.localPosition.x,-1000,6f * Time.deltaTime),0,0);
        }
        Upperbar.SetActive(false);
        Lowerbar.SetActive(false);
        TitleCard.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        MainMenu.transform.localPosition = new Vector3(1280,0,0);
        MainMenu.SetActive(true);
        print(LowerbarGradient.transform.localPosition);
        RectTransform _upGradRect = UpperbarGradient.GetComponent<RectTransform>();
        RectTransform _lowGradRect = LowerbarGradient.GetComponent<RectTransform>();
        _upGradRect.anchoredPosition = new Vector3(0,150,0);
        _lowGradRect.anchoredPosition = new Vector3(0,-150,0);
        StartTime = Time.time;
        while(Time.time - StartTime < 1){
            _upGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_upGradRect.anchoredPosition.y,-50,6f * Time.deltaTime),0);
            _lowGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_lowGradRect.anchoredPosition.y,50,6f * Time.deltaTime),0);
            MainMenu.transform.localPosition= new Vector3(Mathf.Lerp(MainMenu.transform.localPosition.x,0,6f * Time.deltaTime),0,0);
            yield return 1;
        }
        MainMenu.transform.localPosition = new Vector3(0,0,0);
    }

    IEnumerator OnStartCoroutine(){
        Upperbar.transform.localPosition = new Vector3(0,250,0);
        Lowerbar.transform.localPosition = new Vector3(0,-250,0);
        float StartTime = Time.time;
        while(Time.time - StartTime < 2){
            Upperbar.transform.localPosition = new Vector3(0,Mathf.Lerp(Upperbar.transform.localPosition.y,485,2f * Time.deltaTime),0);
            Lowerbar.transform.localPosition = new Vector3(0,Mathf.Lerp(Lowerbar.transform.localPosition.y,-485,2f * Time.deltaTime),0);
            yield return 1;
        }
        ContinueButton.SetActive(true);
        StartTime = Time.time;
        while(Time.time - StartTime < 1){
            float _val = Mathf.SmoothDamp(0f,255f, ref yVelocity, 5f);
            PressToContinue.color = new Color(1,1,1,_val);
            yield return 1;
        }
    }

    public void OpenGameList(){
        StartCoroutine(OpenGameListCoroutine(ListObject));
    }
    public void CloseGameList(){
        StartCoroutine(CloseGameListCoroutine(ListObject));
    }
    public void OpenSettingsMenu(){
        StartCoroutine(OpenGameListCoroutine(SettingsObject));
    }
    public void CloseSettingsMenu(){
        StartCoroutine(CloseGameListCoroutine(SettingsObject));
    }
    IEnumerator OpenGameListCoroutine(GameObject objectToLerp){
        RectTransform _upGradRect = UpperbarGradient.GetComponent<RectTransform>();
        RectTransform _lowGradRect = LowerbarGradient.GetComponent<RectTransform>();
        //tryin out a new way to interpolate in time
        float StartTime = Time.time;
        while(Time.time - StartTime < 1){
            _upGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_upGradRect.anchoredPosition.y,150,6f * Time.deltaTime),0);
            _lowGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_lowGradRect.anchoredPosition.y,-150,6f * Time.deltaTime),0);
            MainMenuButtons.transform.localPosition= new Vector3(Mathf.Lerp(MainMenuButtons.transform.localPosition.x,-1280,6f * Time.deltaTime),0,0);
            yield return 1;
        }
        objectToLerp.SetActive(true);
        objectToLerp.transform.localRotation = Quaternion.Euler(-178.4f,14.3200016f,2.21868277e-07f);
        objectToLerp.transform.localPosition = new Vector3(0,-850,0);
        StartTime = Time.time;
        while(Time.time - StartTime < 1){
            objectToLerp.transform.localRotation = Quaternion.Lerp(objectToLerp.transform.localRotation, Quaternion.Euler(344.160004f,14.3200016f,2.21868277e-07f),6f * Time.deltaTime);
            objectToLerp.transform.localPosition = Vector3.Lerp(objectToLerp.transform.localPosition, Vector3.zero, 6f * Time.deltaTime);
            yield return 1;
        }
    }
    IEnumerator CloseGameListCoroutine(GameObject objectToLerp){
        RectTransform _upGradRect = UpperbarGradient.GetComponent<RectTransform>();
        RectTransform _lowGradRect = LowerbarGradient.GetComponent<RectTransform>();
        objectToLerp.transform.localRotation = Quaternion.Euler(344.160004f,14.3200016f,2.21868277e-07f);
        objectToLerp.transform.localPosition = Vector3.zero;
        float StartTime = Time.time;
        while(Time.time - StartTime < 1){
            objectToLerp.transform.localRotation = Quaternion.Lerp(objectToLerp.transform.localRotation, Quaternion.Euler(-178.4f,14.3200016f,2.21868277e-07f),6f * Time.deltaTime);
            objectToLerp.transform.localPosition = Vector3.Lerp(objectToLerp.transform.localPosition, new Vector3(0,-850,0), 6f * Time.deltaTime);
            yield return 1;
        }
        objectToLerp.SetActive(false);
        StartTime = Time.time;
        while(Time.time - StartTime < 1){
            _upGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_upGradRect.anchoredPosition.y,-50,6f * Time.deltaTime),0);
            _lowGradRect.anchoredPosition = new Vector3(0,Mathf.Lerp(_lowGradRect.anchoredPosition.y,50,6f * Time.deltaTime),0);
            MainMenuButtons.transform.localPosition= new Vector3(Mathf.Lerp(MainMenuButtons.transform.localPosition.x,0,6f * Time.deltaTime),0,0);
            yield return 1;
        }
    }
    public void PlayButtonPress(){
        StartCoroutine(PlayButtonPressed());
    }
    IEnumerator PlayButtonPressed(){
        for(int i = 0; i < 100; i++){
            blackout.color = new Color(0,0,0,i/100f*2.55f);
            yield return new WaitForSeconds(0.01f);
        }
        GSmanager.Play(MLManager.tracks[MLManager.SelectedId]);
    }
    string getRandomExitText(){
        int TotalWeight = 0;
        foreach(exitText et in ExitStrings){
            TotalWeight += et.Weight;
        }
        int RandomRaw = UnityEngine.Random.Range(1,TotalWeight);

        foreach(exitText et in ExitStrings){
            RandomRaw-=et.Weight;
            if(RandomRaw <= 0){
                return et.Text;
            }
        }
        return ExitStrings.Last().Text;
    }
    public void OnExitPress(){
        StartCoroutine(Exit());
    }
    IEnumerator Exit(){
        blackout.raycastTarget = true;
        ExitText.text = getRandomExitText();
        ExitText.gameObject.SetActive(true);
        for(int i = 0; i < 100; i++){
            ExitText.color = new Color(1,1,1,i/100f*2.55f);
            blackout.color = new Color(0,0,0,i/100f*2.55f);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

}
