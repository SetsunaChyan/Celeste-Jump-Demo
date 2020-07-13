using System;
using MySql.Data.MySqlClient;

public class RankEntity
{
    string userName,score;

    public RankEntity()
    {

    }

    public RankEntity(string userName,long score) 
    {
        setUserName(userName);
        setScore(score);
    }

    public void setUserName(string userName)
    {
        this.userName="";
        for(int i=0;i<Math.Min(userName.Length,7);i++)
            this.userName+=userName[i];
        if(userName.Length>7) this.userName+="...";
    }

    public void setScore(long score)
    {
        this.score=score.ToString();
    }

    public string getUserName()
    {
        return userName;
    }

    public string getScore()
    {
        return score;
    }
}