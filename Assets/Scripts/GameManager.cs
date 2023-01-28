using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Spawner sp => Spawner.Instance;
    
    public GameObject nuke1;
    public GameObject nuke2;
    public GameObject nuke3;

    public List<GameObject> powerUps;
    public Transform cameraOriginalPos;
    public AudioSource Source;
    public AudioSource cembo;

    public bool gameStarted;
    public bool gameEnded;
    public float timer = 59f;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI saglamArabalar;
    public TextMeshProUGUI patlamisArabalar;

    public Rigidbody tekne;
    public float tekneHiz;
    public Vector3 tekneYon;

    public List<GameObject> startUI;
    public List<GameObject> endUI;
    public List<GameObject> gameUI;
    public GameObject evacuateText;
    public TextMeshProUGUI EvacuateTextMeshProUGUI;
    
    public List<GameObject> nukessss;
    public GameObject FirstTimeText;
    
    public bool FirstTime => PlayerPrefs.GetInt("First") != 1;

    private void Awake()
    {
        Instance = this;
        Car.SomethingChanged += SomethingChanged;
    }

    private void SomethingChanged()
    {
        if(sp == null) return;
        if(sp.cars == null) return;

        var saglamlar = sp.cars.Count(car => !car.explodedNormalCar);
        var bozuklar = sp.cars.Count(car => car.explodedNormalCar);
        saglamArabalar.text = saglamlar+ "";
        patlamisArabalar.text = bozuklar + "";
    }
    


    private IEnumerator Startasd()
    {
        yield return new WaitForSeconds(2);
        
        evacuateText.SetActive(true);
        EvacuateTextMeshProUGUI.DOFade(0, 0.3f).SetEase(Ease.InCirc).SetLoops(-1, LoopType.Yoyo);
        
        yield return new WaitForSeconds(2);
        
        
        Source.DOFade(0, 3);
        cembo.DOFade(1, 3);
        
        yield return new WaitForSeconds(1);

        evacuateText.SetActive(false);
        Camera.main.transform.DOMove(cameraOriginalPos.position, 1f);
        Camera.main.transform.DORotate(cameraOriginalPos.eulerAngles, 1f).OnComplete(() => Camera.main.transform.parent = cameraOriginalPos);
        
        yield return new WaitForSeconds(1f);

        if (!FirstTime)
        {
            continueGame();
        }
        else
        {
            FirstTimeText.SetActive(true);
            PlayerPrefs.SetInt("First", 1);
        }
    }

    public void continueGame()
    {
        FirstTimeText.SetActive(false);
        gameStarted = true;   
        gameUI.ForEach(o => o.SetActive(true));
        StartCoroutine(RandomSpawn());
    }

    public void StartGame()
    {
        nukessss.ForEach(o => o.SetActive(true));
        Source.Play();
        startUI.ForEach(o => o.SetActive(false));
        StartCoroutine(Startasd());
    }

    private void Update()
    {
        if(gameEnded) return;
        
        if (gameStarted)
        {
            timer -= Time.deltaTime;
            timeText.text = (int)timer + "";
        }

        if (timer <= 0)
        {
            gameEnded = true;
            StartCoroutine(EndScreen());    
        }
    }

    public IEnumerator EndScreen()
    {
        yield return new WaitForSeconds(1);

        tekne.transform.DOMove(tekne.transform.position + tekneYon * tekneHiz, 50);
        
        endUI.ForEach(o => o.SetActive(true));
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void SpawnNuke()
    {
        Instantiate(nuke1).SetActive(true);
        Instantiate(nuke2).SetActive(true);
        Instantiate(nuke3).SetActive(true);
    }

    IEnumerator RandomSpawn()
    {
        while (true)
        {
            var obj = powerUps[Random.Range(0,powerUps.Count)];
            var objasd = Instantiate(obj);
            objasd.SetActive(true);
            objasd.transform.position = obj.transform.position;
            objasd.transform.rotation = obj.transform.rotation;
            Destroy(objasd, 5f);
            yield return new WaitForSeconds(10f);
        }
    }
}
