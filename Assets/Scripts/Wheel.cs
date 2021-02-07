using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    private float timer=1f;
    public int AvailableKnifes;

    [SerializeField]private Sprite firstLog;
    [SerializeField]private Sprite secondLog;
    [SerializeField]private Sprite thirdLog;

    [SerializeField]private bool isBoss;

    [Header(header: "Prefabs")]
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] GameObject brokenLog;

    [Header(header: "Settings")]
    [SerializeField] private float rotationTime;
    [SerializeField] private float rotationZ;

    [SerializeField] private Settings settings;

    public List<Level> levels;
   
   
    public List<Knife> Knifes;
    private int levelIndex;
    public int AvailableKnife => AvailableKnifes;
    private void Start()
    {
       if (!isBoss)
       {
            if (GameManager.Instance.Stage<5)
            {
                GetComponent<SpriteRenderer>().sprite = firstLog;
            }
            else if(GameManager.Instance.Stage>5&&GameManager.Instance.Stage<10)
            {
                GetComponent<SpriteRenderer>().sprite = secondLog;
            }
            else if (GameManager.Instance.Stage>10)
            {
                GetComponent<SpriteRenderer>().sprite = thirdLog;
            }
       }
       
       

        levelIndex = Random.Range(0, levels.Count);

        levels[levelIndex].appleSpawnChance = settings.AppleSpawnChance;
        if (levels[levelIndex].appleSpawnChance>Random.value)
        {
            SpawnApple();
        }
        SpawnKnifes();
    }

    private void RotateWheel()
    {
        transform.Rotate(xAngle: 0f, yAngle: 0f, zAngle: rotationZ * Time.deltaTime);
    }

    private void Update()
    {
        if (Knifes.Count-levels[levelIndex].knifeAngleFromWheel.Count >= AvailableKnifes)
        {
           
            timer -= Time.deltaTime;
        }
        else
        {
            RotateWheel();
        }

        if (timer <= 0)
        {
            Vibration.Vibrate(100);
           
            LevelManager.Instance.NextLevel();
        }


    }
    private void SpawnKnifes()
    {
        foreach (var knifeAngle in levels[levelIndex].knifeAngleFromWheel)
        {
            GameObject knifeTmp = Instantiate(knifePrefab);
            knifeTmp.transform.SetParent(transform);
            SetRotationFromWheel(wheel: transform, objectToPlace: knifeTmp.transform, knifeAngle, spaceFromObject:-3.3f, objectRotation:-360f);
            knifeTmp.transform.localScale = new Vector3(x: 1.2f, y: 1.3f, z: 1.2f);
        }
    }
    private void SpawnApple()
    {
        foreach (var appleAngle in levels[levelIndex].appleAngleFromWheel)
        {
            GameObject appleTmp = Instantiate(applePrefab);
            appleTmp.transform.SetParent(transform);

            SetRotationFromWheel(wheel: transform, objectToPlace: appleTmp.transform, appleAngle, spaceFromObject: -2.7f, objectRotation: 0f);
          

        }
    }


    public void SetRotationFromWheel(Transform wheel ,Transform objectToPlace, float angle, float spaceFromObject, float objectRotation)
    {
        Vector2 offSet = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * (wheel.GetComponent<CircleCollider2D>().radius + spaceFromObject);
        objectToPlace.localPosition = (Vector2)wheel.localPosition + offSet;
        objectToPlace.localRotation = Quaternion.Euler(x: 0, y: 0, z: -angle+objectRotation);
    }
   
    public void KnifeHit(Knife knife)
    {
        knife.rigidbody2D.isKinematic = true;
        knife.rigidbody2D.velocity = Vector2.zero;
        knife.transform.SetParent(transform);
        knife.Hit = true;
        Knifes.Add(knife);
        if (Knifes.Count- levels[levelIndex].knifeAngleFromWheel .Count>= AvailableKnifes)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            GameObject logTMP= Instantiate(brokenLog, LevelManager.Instance.wheelSpawnPosition.position, Quaternion.identity);

            Rigidbody2D[] rbk = GetComponentsInChildren<Rigidbody2D>();
            
            foreach (Rigidbody2D item in rbk)
            {
                item.isKinematic = false;
                item.gravityScale = 1;
                item.velocity =new Vector2(0,0) ;
                item.angularVelocity = 0;
                item.AddForce(new Vector2(Random.Range(-50, 50), Random.Range(-50, 50)));
            }
            Rigidbody2D[] rbs = logTMP.GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D tmp in rbs )
            {
                tmp.gravityScale = 1;

                tmp.AddForce(new Vector2 (Random.Range(-50, 50), Random.Range(-50, 50)));
            }

            

        }
        GameManager.Instance.Score++;
       
    }

        public void DesstroyKnife()
        {
            foreach(var knife in Knifes)
            {
                Destroy(knife.gameObject);
            }
            Destroy(gameObject);
        }
   
}

[System.Serializable]
public class Level
{
    [HideInInspector]
    [Range(0, 5)]  public float appleSpawnChance;


    public List<float> appleAngleFromWheel = new List<float>();
    public List<float> knifeAngleFromWheel = new List<float>();
}