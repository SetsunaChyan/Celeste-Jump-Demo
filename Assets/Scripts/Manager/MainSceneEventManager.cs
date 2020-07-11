using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneEventManager : MonoBehaviour
{
    static MainSceneEventManager instance;
    
    // UI中显示分数的Text
    public Text scoreText;
    long bonus;
    long timeScore;
    public GameObject gameoverUI;

    void Awake()
    {
        if(instance!=null) Destroy(gameObject);
        instance=this;
        bonus=0;
        timeScore=0;
    }

    void Update()
    {
        // 更新分数
        timeScore+=(long)(Time.deltaTime*1000);
        scoreText.text="Score: "+(timeScore+bonus).ToString("d8");

        // 用于Debug重启游戏
        if(Input.GetKeyDown(KeyCode.R)) RestartGame();
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GameOver()
    {
        instance.gameoverUI.SetActive(true);
    }
}
