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
    // 游戏结束时显示的分数Text
    public Text lastScoreText;
    static long bonus;
    static float timeScore;
    public GameObject gameoverUI;
    static bool isGameover;
    static MySqlAccess mySql;
    public List<Text> rankNameList;
    public List<Text> rankScoreList;

    void Awake()
    {
        if(instance!=null) Destroy(gameObject);
        instance=this;
        Time.timeScale=1;
        bonus=0;
        timeScore=0;
        isGameover=false;
        Random.InitState((int)System.DateTime.Now.Ticks);
        MySqlSystemInit();
    }

    void Update()
    {
        if(isGameover) return;

        // 更新分数
        timeScore+=Time.deltaTime*300;
        scoreText.text="Score: "+((long)timeScore+bonus).ToString("d8");

        // 用于Debug重启游戏
        if(Input.GetKeyDown(KeyCode.R)) RestartGame();
    }

    public static void addScore(long delta)
    {
        bonus+=delta;
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void GameOver()
    {
        isGameover=true;
        long lastScore=(long)timeScore+bonus;
        instance.MySqlAddScore(lastScore);
        instance.UpdateRank();
        instance.lastScoreText.text=lastScore.ToString("d");
        instance.scoreText.gameObject.SetActive(false);
        instance.gameoverUI.SetActive(true);
        Time.timeScale=0;
    }

    public static float getTimeScore()
    {
        return timeScore;
    }

    public static long getBonus()
    {
        return bonus;
    }

    void MySqlSystemInit()
    {
        mySql=new MySqlAccess("47.115.54.167","3306","root","setsuna","CelesteDB"); // 真的会有人要学生机吗
    }

    void MySqlAddScore(long score)
    {
        string sql=string.Format("insert into rank (deviceUniqueIdentifier,deviceName,deviceModel,submission_date,score) values ('{0}','{1}','{2}','{3}','{4}')",
            SystemInfo.deviceUniqueIdentifier,SystemInfo.deviceName,SystemInfo.deviceModel,System.DateTime.Now.ToString(),score);
        mySql.ExecuteQuery(sql);
    }

    void UpdateRank()
    {
        List<RankEntity> list=mySql.getRankList();
        int len=list.Count;
        
        for(int i=0;i<len;i++)
        {
            rankNameList[i].text=(i+1).ToString();
            if(i==0) rankNameList[i].text="%";
            else if(i==9) rankNameList[i].text="X";
            rankNameList[i].text+=".  "+list[i].getUserName();
            rankScoreList[i].text=list[i].getScore();
        }

        // 不满10条则不显示完
        for(int i=len;i<10;i++) 
        {
            rankNameList[i].text="";
            rankScoreList[i].text="";
        }
    }
}
