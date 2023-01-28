using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;
    
    public float force;
    public List<GameObject> prefab;
    public float time;
    public Transform direction;
    public GameObject lastCar;

    public List<Car> cars;
    public bool IsTaramaMode;
    public float TaramaModeSaniye;
    public float delayTarama;

    public GameObject lastFlaying;
    
    private void Awake()
    {
        Instance = this;
    }

    

    void Start()
    {
        Spanw();
        Joystickk.Instance.released += ApplyForce;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Tara()
    {
        while (IsTaramaMode)
        {
            if (GameManager.Instance.gameEnded)
            {
                break;
            }
            ApplyForce(10f);
            yield return new WaitForSeconds(delayTarama);
        }

        yield return null;
    }

    public IEnumerator TaramaMode()
    {
        IsTaramaMode = true;
        StartCoroutine(Tara());
        yield return new WaitForSeconds(TaramaModeSaniye);

        IsTaramaMode = false;
    }

    void Spanw()
    {
        if(GameManager.Instance.gameEnded) return;
        
        var index = Random.Range (0, prefab.Count);
        lastCar = Instantiate(prefab[index]);
        lastCar.transform.position = transform.position;
        lastCar.transform.rotation = transform.rotation;
        var rigd = lastCar.GetComponent<Rigidbody>();
        rigd.isKinematic = true;
        
        cars.Add(lastCar.GetComponent<Car>());

    }

    void ApplyForce(float distance)
    {
        if(GameManager.Instance.gameEnded) return;
        if(!GameManager.Instance.gameStarted) return;

        
        lastFlaying = lastCar;
        var rigd = lastCar.GetComponent<Rigidbody>();
        rigd.isKinematic = false;
        lastCar.GetComponent<Car>().throwed();
        lastCar.GetComponent<Deform>().distanceMultiplier = distance;
        rigd.AddForce(Joystickk.Instance.direction * (distance * force), ForceMode.VelocityChange);
        Spanw();
    }
}
