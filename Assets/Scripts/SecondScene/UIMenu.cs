using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIMenu : MonoBehaviour
{
    public int Score;
    public Text ScoreText;

    public int Stage;
    public Text StageText;

    public Text appleCount;
    private void Start()
    {
        Score=PlayerPrefs.GetInt("Score", Score);
        ScoreText.text ="Score "+ Score.ToString();

        Stage = PlayerPrefs.GetInt("Stage", Stage);
        StageText.text = "Stage " + Stage.ToString();

        appleCount.text = PlayerPrefs.GetInt("Apple", 0).ToString();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
