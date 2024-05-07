using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    [SerializeField] GyroValues Values;
    [SerializeField] TrackSettings settings;
    [SerializeField] Material RingMaterial;
    [SerializeField] GameObject VisualiseHit;
    [SerializeField] GameObject DisplayCube;
    [SerializeField] Color[] colors;
    Vector3 LastUpdateVec3;
    [SerializeField] List<GameObject> ObjectsInFrame;
    public int ConsecNotes;
    void Start(){
        ObjectsInFrame = new List<GameObject>();
    }
    //Ring perfect IEnumerator
    bool RPAlreadyRunning = false;

    void Update(){
        bool IsNoteInFrame = ObjectsInFrame.Count > 0;
        if(IsNoteInFrame){
            bool WasHitSuccessful = false;

            if(Values.HittingPart.x != LastUpdateVec3.x || Values.HittingPart.y != LastUpdateVec3.y || Values.HittingPart.z != LastUpdateVec3.z){
                WasHitSuccessful=true;
            }

            if(WasHitSuccessful){
                IncomingPartPath.Direction dir = IncomingPartPath.Direction.UP;
                //cant switch case this sorry
                if(Values.HittingPart.x > 0) { dir = IncomingPartPath.Direction.DOWN; }
                else if(Values.HittingPart.x < 0) { dir = IncomingPartPath.Direction.UP; }
                else if(Values.HittingPart.y > 0) { dir = IncomingPartPath.Direction.RIGHT; }
                else if(Values.HittingPart.y < 0) { dir = IncomingPartPath.Direction.LEFT; }
                else if(Values.HittingPart.z != 0) { dir = IncomingPartPath.Direction.ROLL; }
                //allow only 1 direction at a time so you cant cheese this (for now)
                foreach (GameObject obj in ObjectsInFrame){
                    if(obj.GetComponent<IncomingPartPath>().Dir == dir){
                        ObjectsInFrame.Remove(obj);
                        settings.Score += (9-Mathf.RoundToInt(Vector3.Distance(transform.position, obj.transform.position)))/ (settings.HeatMode ? 1 : 2);
                        ConsecNotes++;
                        settings.NotesHit++;
                        if(Vector3.Distance(transform.position, obj.transform.position) < 2.8f){
                            settings.Score+=5;
                            if(!RPAlreadyRunning){
                                StartCoroutine(RingPerfect(dir));
                            }else{
                                StopCoroutine("RingPerfect");
                            }
                        }
                        if(ConsecNotes >= 7){
                            settings.NotesHitInHeatMode++;
                            settings.HeatModeToggle(true);
                        }
                        Destroy(obj);
                        StartCoroutine(StartHitVis(dir));
                    }
                }

                LastUpdateVec3 = Values.HittingPart;
            }
        }
    }

    IEnumerator RingPerfect(IncomingPartPath.Direction dir){
        RPAlreadyRunning = true;
        Color col;
        switch(dir){
        case IncomingPartPath.Direction.UP:
            col = colors[0];
            break;
        case IncomingPartPath.Direction.DOWN:
            col = colors[1];
            break;
        case IncomingPartPath.Direction.LEFT:
            col = colors[2];
            break;
        case IncomingPartPath.Direction.RIGHT:
            col = colors[3];
            break;
        default:
            col = colors[4];
            break;
        }
        print("YEAH!!");
        for(int i = 0; i < 25; i++){
            RingMaterial.SetColor("_RingColor", Color.Lerp(col,Color.white,i/25f));
            RingMaterial.SetFloat("_Offset", Mathf.Lerp(0.8f,0.71f,i/25f));
            yield return new WaitForSeconds(0.02f);
        }
        RPAlreadyRunning = false;
    }

    void OnTriggerEnter(Collider coll){
        if(coll.gameObject.tag == "Note"){
            ObjectsInFrame.Add(coll.gameObject);
        }
    }
    void OnTriggerExit(Collider coll){
        if(coll.gameObject.tag == "Note"){
            if(ObjectsInFrame.Contains(coll.gameObject)){
                ObjectsInFrame.Remove(coll.gameObject);
                ConsecNotes = 0;
                settings.HeatModeToggle(false);
            }
        }
    }

    IEnumerator StartHitVis(IncomingPartPath.Direction dir){
        //setup
        GameObject cubeHit = GameObject.Instantiate(VisualiseHit);
        cubeHit.transform.SetParent(DisplayCube.transform, false);
        //new material
        cubeHit.GetComponent<MeshRenderer>().materials[0] = new Material(cubeHit.GetComponent<MeshRenderer>().materials[0]);
        Material mat = cubeHit.GetComponent<MeshRenderer>().materials[0];

        switch(dir){
            case IncomingPartPath.Direction.UP:
                mat.SetColor("_Color", colors[0]);
                break;
            case IncomingPartPath.Direction.DOWN:
                mat.SetColor("_Color", colors[1]);
                break;
            case IncomingPartPath.Direction.LEFT:
                mat.SetColor("_Color", colors[2]);
                break;
            case IncomingPartPath.Direction.RIGHT:
                mat.SetColor("_Color", colors[3]);
                break;
            default:
                mat.SetColor("_Color", colors[4]);
                break;
        }

        cubeHit.SetActive(true);

        for(int i = 0; i < 50; i++){
            mat.SetFloat("_Opacity", i*2/10);

            cubeHit.transform.localScale = new Vector3(1+i/100f,1+i/100f,1+i/100f);

            yield return new WaitForSeconds(0.01f);
        }
        Destroy(cubeHit);
    }
}
