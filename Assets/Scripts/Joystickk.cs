using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystickk : MonoBehaviour
{
    public static Joystickk Instance;
    public Action<float> released;

    public Vector3 direction;

    public Camera cam;
    public Transform refenceTrans;
    public Transform sapan;
    public float distance;

    public float maxDistanceGoingLines = 2;
    public GameObject way;
    public GameObject line;
    private List<GameObject> ways;
    private List<GameObject> lines;
    private Vector3 cachedScale;

    
    private bool dragged;
    private void Awake()
    {
        Instance = this;
        ways = new List<GameObject>();
        lines = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cachedScale = line.transform.localScale;
        for (int i = 0; i < 50; i++)
        {
            var lineess = Instantiate(line);
            var asdddd = Instantiate(way);
            ways.Add(asdddd);
            lines.Add(lineess);
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButton(0))
        {
            dragged = true;
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit))
            {
                Spawner.Instance.lastCar.transform.position = new Vector3(hit.point.x,
                    Spawner.Instance.lastCar.transform.position.y, hit.point.z);
                
                
                sapan.position = new Vector3(hit.point.x, sapan.position.y, hit.point.z);
                sapan.LookAt(refenceTrans);

                Spawner.Instance.lastCar.transform.LookAt(refenceTrans);
                
                for (var i = 0; i < ways.Count; i++)
                {
                    var wayssss = ways[i];
                    var lerp = Vector3.Lerp(sapan.position, refenceTrans.position, (float)i / ways.Count);
                    wayssss.transform.position = lerp;
                    wayssss.transform.LookAt(refenceTrans);

                    lerp += (lerp - sapan.position) * maxDistanceGoingLines;
                    lerp += (refenceTrans.position - sapan.position);
                    var lineeeee = lines[i];
                    lineeeee.transform.position = lerp;
                    lineeeee.transform.LookAt(refenceTrans);
                    lineeeee.transform.localScale = cachedScale * (1 - (float)i / ways.Count);
                }

                direction = sapan.forward;
            }
        }
        else
        {
            if (dragged)
            {
                released?.Invoke(distance);
                dragged = false;
            }
                
            foreach (var o in ways)
            {
                o.transform.position = new Vector3(0, -100, 0);
            }
            
            foreach (var o in lines)
            {
                o.transform.position = new Vector3(0, -100, 0);
            }
            sapan.position = refenceTrans.position;
            sapan.rotation = refenceTrans.rotation;
        }

        distance = Vector3.Distance(sapan.position, refenceTrans.position);
#endif
        
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            dragged = true;
            if(Physics.Raycast(cam.ScreenPointToRay(Input.GetTouch(0).position), out var hit))
            {
                Spawner.Instance.lastCar.transform.position = new Vector3(hit.point.x,
                    Spawner.Instance.lastCar.transform.position.y, hit.point.z);
                
                
                sapan.position = new Vector3(hit.point.x, sapan.position.y, hit.point.z);
                sapan.LookAt(refenceTrans);

                Spawner.Instance.lastCar.transform.LookAt(refenceTrans);
                
                for (var i = 0; i < ways.Count; i++)
                {
                    var wayssss = ways[i];
                    var lerp = Vector3.Lerp(sapan.position, refenceTrans.position, (float)i / ways.Count);
                    wayssss.transform.position = lerp;
                    wayssss.transform.LookAt(refenceTrans);

                    lerp += (lerp - sapan.position) * maxDistanceGoingLines;
                    lerp += (refenceTrans.position - sapan.position);
                    var lineeeee = lines[i];
                    lineeeee.transform.position = lerp;
                    lineeeee.transform.LookAt(refenceTrans);
                    lineeeee.transform.localScale = cachedScale * (1 - (float)i / ways.Count);
                }

                direction = sapan.forward;
            }
        }
        else
        {
            if (dragged)
            {
                released?.Invoke(distance);
                dragged = false;
            }
                
            foreach (var o in ways)
            {
                o.transform.position = new Vector3(0, -100, 0);
            }
            
            foreach (var o in lines)
            {
                o.transform.position = new Vector3(0, -100, 0);
            }
            sapan.position = refenceTrans.position;
            sapan.rotation = refenceTrans.rotation;
        }

        distance = Vector3.Distance(sapan.position, refenceTrans.position);
#endif
    }
}
