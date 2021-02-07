using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    public bool IsGameOVer { get; set; }
    public int Stage { get; set; }
    public int Score { get; set; }

    
    public Knife SelectedKnifePrefab { get; set; }

    public float ScreenHeight => Camera.main.orthographicSize * 2;
    public float ScreenWidth => ScreenHeight / Screen.height * Screen.width;


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Score", Score);
        PlayerPrefs.SetInt("Stage", Stage);
    }


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


}
