using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public static UI Instance;
    [Header(header:"UI Settings")]
    [SerializeField] private Text ScoreText;
    [SerializeField] private int saveScore;
    [SerializeField] private Text StageText;
    [SerializeField] private int saveStage;



    public Text Apples;
    public int AmountApple;


    [SerializeField] private GameObject StageContainer;

    [SerializeField] private Color stageColor;
    [SerializeField] private Color stageCompletedColor;
    public List<Image> stageIcons;

    [Header(header: "UI BOSS")]
    [SerializeField] private GameObject bossFight;
    [SerializeField] private GameObject bossDefeated;

    [Header(header: "GameOver UI")] [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField] private Text gameOverScores;
    [SerializeField] private Text gameOverStages;



    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Apple", AmountApple);

    }
    private void Start()
    {
        saveScore = PlayerPrefs.GetInt("Score");
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

    private void Update()
    {
        ScoreText.text = GameManager.Instance.Score.ToString();
        gameOverScores.text = GameManager.Instance.Score.ToString();

        StageText.text = "Stage" + GameManager.Instance.Stage;
        gameOverStages.text = "Stage" + GameManager.Instance.Stage;

        UpdateUI();
    }

    public IEnumerator BossStart()
    {
        bossFight.SetActive(true);
        yield return new WaitForSeconds(1f);
        bossFight.SetActive(false);
    }
    public IEnumerator BossDefeated()
    {
        bossDefeated.SetActive(true);
        yield return new WaitForSeconds(1f);
        bossDefeated.SetActive(false);
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        StageContainer.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



    private void UpdateUI()
    {
        if (GameManager.Instance.Stage%5==0)
        {
            foreach (var icon in stageIcons)
            {
                icon.gameObject.SetActive(false);
                stageIcons[stageIcons.Count - 1].color = stageColor;
                StageText.text = "Boss" + LevelManager.Instance.bossName;
            }
        }
        else
        {
            for (int i = 0; i <stageIcons.Count; i++)
            {
                stageIcons[i].gameObject.SetActive(true);
                stageIcons[i].color = GameManager.Instance.Stage % 5 <= i ? stageColor : stageCompletedColor;
            }
        }
    }
}
