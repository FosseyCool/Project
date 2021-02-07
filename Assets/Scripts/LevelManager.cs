using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public Wheel[] wheels;
    public Boss[] bosses;

    [SerializeField] private GameObject knifePrefab;

    [Header(header: "Wheel settings")]
    [SerializeField] public Transform wheelSpawnPosition;
    [Range(0, 1)] [SerializeField] private float wheelScale;

    [Header(header: "Knife settings")]
    [SerializeField] private Transform knifeSpawnPosition;
    [Range(0, 1)] [SerializeField] private float knifeScale;

    public string bossName;
    private Wheel currentWheel;
    private Knife currentKnife;

    public int TotalSpawnKnife { get; set; }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
           
        }
    }

    private void Start()
    {
        InitializedGame();
        
    }
    private void Update()
    {
        if (currentKnife==null)
        {
            return;

        }
        if (Input.GetMouseButtonDown(0) && !currentKnife.IsRealsed)
        {
            KnifeCounter.Instance.KnifeHit(TotalSpawnKnife);
            currentKnife.FireKnife();
            StartCoroutine(routine: GenerateKnife());
        }
    }
    private void InitializedGame()
    {
        GameManager.Instance.IsGameOVer = false;
        GameManager.Instance.Score = 0;
        GameManager.Instance.Stage = 1;
        SetupGame();
       
    }
    private void SetupGame()
    {
        SpawnWheel();
        KnifeCounter.Instance.SetupKnife(currentWheel.AvailableKnifes);

        
       StartCoroutine(routine: GenerateKnife());
    }

    private void SpawnWheel()
    {
        GameObject tmpWheel;
        if (GameManager.Instance.Stage%5==0)
        {
            TotalSpawnKnife = 1;
            Boss newBoss = bosses[UnityEngine.Random.Range(0, bosses.Length)];
            tmpWheel = Instantiate(newBoss.bossPrefab, wheelSpawnPosition.position, Quaternion.identity, wheelSpawnPosition).gameObject;
            bossName = "Boss" + newBoss.bossName;
        }
        else
        {
            TotalSpawnKnife =1;
            tmpWheel = Instantiate(wheels[GameManager.Instance.Stage-1],wheelSpawnPosition.position,Quaternion.identity,wheelSpawnPosition).gameObject;

        }

        float wheelScaleInScreen = GameManager.Instance.ScreenWidth * wheelScale / tmpWheel.GetComponent<SpriteRenderer>().bounds.size.x;
        tmpWheel.transform.localScale = Vector3.one * wheelScaleInScreen;
        currentWheel = tmpWheel.GetComponent<Wheel>();

    }

    private IEnumerator GenerateKnife()
    {
        yield return new WaitUntil(predicate: () => knifeSpawnPosition.childCount == 0);
        if (currentWheel.AvailableKnifes>=TotalSpawnKnife&&!GameManager.Instance.IsGameOVer)
        {
            
            GameObject tmpKnife ;
            if (GameManager.Instance.SelectedKnifePrefab==null)
            {
                tmpKnife = Instantiate(knifePrefab, knifeSpawnPosition.position, Quaternion.identity, knifeSpawnPosition).gameObject;
            }
            else
            {
                tmpKnife = Instantiate(GameManager.Instance.SelectedKnifePrefab, knifeSpawnPosition.position, Quaternion.identity, knifeSpawnPosition).gameObject;

            }

            float knifeScaleInScreen = GameManager.Instance.ScreenHeight * knifeScale / tmpKnife.GetComponent<SpriteRenderer>().bounds.size.y;
            tmpKnife.transform.localScale = Vector3.one * knifeScaleInScreen;
           currentKnife = tmpKnife.GetComponent<Knife>();
        }
    }
    public void NextLevel()
    {
        if (currentWheel != null)
        {
            currentWheel.DesstroyKnife();
        }

        if (GameManager.Instance.Stage%5==0)
        {
            GameManager.Instance.Stage++;
            StartCoroutine(BossDefeated());
        }
        else
        {
            GameManager.Instance.Stage++;
            if (GameManager.Instance.Stage%5==0)
            {
               StartCoroutine(BossFight());
            }
            else 
            {
                Invoke(nameof(SetupGame), time: 0.3f);
            }
        }


    }
    private IEnumerator BossFight()
    {
        StartCoroutine(UI.Instance.BossStart());
        yield return new WaitForSeconds(2f);
        SetupGame();
    }

    private IEnumerator BossDefeated()
    {
        StartCoroutine(UI.Instance.BossDefeated());
        yield return new WaitForSeconds(2f);
        SetupGame();
    }

}
[Serializable]
public class Boss
{
    public GameObject bossPrefab;
    public string bossName;
}