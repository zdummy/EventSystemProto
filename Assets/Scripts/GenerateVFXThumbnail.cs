using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateVFXThumbnail : MonoBehaviour {
    public bool CaptureScreen = false;
    private string CapturePath = "VFXThumbnail";

    public List<GameObject> VFXList = new List<GameObject>();
    private int VFXIndex;
    private int CaptureIndex;
    private ParticleSystem curPS;
    private float timer;
    private bool ExportingDone = false;

    private int STATE_EXPORTING_THUMBNAIL = 1;
    private int STATE_MODIFYING_PREFAB = 2;
    private int STATE_FINISH = 3;
    private int m_state;

    // Use this for initialization
    void Start()
    {
        VFXIndex = 0;
        PlayNextParticle();
        m_state = STATE_EXPORTING_THUMBNAIL;
        if (!System.IO.Directory.Exists(CapturePath))
        {
            System.IO.Directory.CreateDirectory(CapturePath);
        }
    }

    void PlayNextParticle()
    {
        if (VFXList[VFXIndex] == null )
            return;

       //Debug.Log("PlayNextParticle");
        GameObject go = Object.Instantiate(VFXList[VFXIndex], Vector3.zero, Quaternion.identity);
        ParticleSystem ps = go.GetComponent<ParticleSystem>();

        if (ps != null)
        {
            Debug.Log("Play " + go.name);
            ps.enableEmission = true;
            curPS = ps;
            timer = Time.time + ps.main.duration;
            CaptureIndex = 0;
            Object.Destroy(go, ps.main.duration);
        }
        //else
        //{
        //    timer = Time.time + 1;
        //    Object.Destroy(go, 1);
        //}
    }

	// Update is called once per frame
	void Update () {

        if (VFXList.Count == 0 || m_state == STATE_FINISH)
            return;

        //capture to disk
        if (CaptureScreen && curPS != null)
        {
            string captureFileName = CapturePath +"/"+ VFXList[VFXIndex].name + CaptureIndex + ".png";
            Debug.Log("captureFileName: " + captureFileName);
            if (!System.IO.Directory.Exists(captureFileName))
            {
                ScreenCapture.CaptureScreenshot(captureFileName);
                CaptureIndex++;
            }
        }


        //switch to next 
        //play particles in order
        if (Time.time > timer)
        {
            if (curPS != null)
                curPS.enableEmission = false;
            if (VFXIndex < VFXList.Count-1)
            {
                VFXIndex++;
                PlayNextParticle();
            }
            else
            {
                m_state = STATE_MODIFYING_PREFAB;

            }
        }

        if (m_state == STATE_MODIFYING_PREFAB)
        {
            UpdatePrefab();
        }
    }

    void UpdatePrefab()
    {
        EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", 0);

        string[] ids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets/EffectExamples/Fire & Explosion Effects/Prefabs" });
        for (int i = 0; i < ids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(ids[i]);
            GameObject prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            // change instance
            GameObject thumbnail = new GameObject("thumbnail");
            thumbnail.transform.SetParent(instance.transform);

            thumbnail.transform.localPosition = new Vector3(0f, 0f, 0f);
            thumbnail.transform.rotation = Quaternion.Euler(0f, 60f, 0f);
            thumbnail.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            SpriteRenderer sprite = thumbnail.AddComponent<SpriteRenderer>();

            //instance.AddComponent(Sprite.Create(null,new Rect(), new Vector2(0f,0f)));

            // other change instance ... ...
            //Transform[] trans = instance.GetComponentsInChildren<Transform>();
            //int layer = LayerMask.NameToLayer("Player");
            //for (int j = 0; j < trans.Length; j++)
            //{
            //    trans[j].gameObject.layer = layer;
            //}


            // save
            PrefabUtility.ReplacePrefab(instance, prefab, ReplacePrefabOptions.ConnectToPrefab);

            DestroyImmediate(instance);

            EditorUtility.DisplayProgressBar("Modify Prefab", "Please wait...", i / (float)ids.Length);

            break;
        }

        AssetDatabase.SaveAssets();

        EditorUtility.ClearProgressBar();
        m_state = STATE_FINISH;
    }
}
