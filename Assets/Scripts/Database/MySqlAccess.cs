using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

public class MySqlAccess
{

    // 连接类对象
    private static MySqlConnection mySqlConnection;
    // IP地址
    private static string host;
    // 端口号
    private static string port;
    // 用户名
    private static string userName;
    // 密码
    private static string password;
    // 数据库名称
    private static string databaseName;
 
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="_host">ip地址</param>
    /// <param name="_userName">用户名</param>
    /// <param name="_password">密码</param>
    /// <param name="_databaseName">数据库名称</param>
    public MySqlAccess(string _host,string _port, string _userName, string _password, string _databaseName) 
    {
        host=_host;
        port=_port;
        userName=_userName;
        password=_password;
        databaseName=_databaseName;
        // OpenSql();
    }
 
    void OpenSql()
    {
        try 
        {
            string mySqlString=string.Format("Database={0};Data Source={1};User Id={2};Password={3};port={4}",databaseName,host,userName,password,port);
            mySqlConnection=new MySqlConnection(mySqlString);
            mySqlConnection.Open();
        } 
        catch (Exception e) 
        {
            throw new Exception("Database connect error: "+e.Message.ToString());
        }
    }

    public int ExecuteQuery(String sql)
    {
        OpenSql();
        MySqlCommand cmd=new MySqlCommand(sql,mySqlConnection);
        int ret=cmd.ExecuteNonQuery();
        CloseSql();
        return ret;
    }

    public List<RankEntity> getRankList()
    {
        OpenSql();
        String sql="select * from rank order by score desc limit 10;";
        MySqlCommand cmd=new MySqlCommand(sql,mySqlConnection);
        List<RankEntity> ret=new List<RankEntity>();
        using(MySqlDataReader reader=cmd.ExecuteReader())
        {
            while(reader.Read())
            {
                RankEntity tmp=new RankEntity();
                tmp.setUserName(reader.GetString("deviceName"));
                tmp.setScore((long)reader.GetUInt64("score"));
                ret.Add(tmp);
            }
        }
        CloseSql();
        return ret;
    }
 
    void CloseSql() 
    {
        if(mySqlConnection!=null) 
        {
            mySqlConnection.Close();
            mySqlConnection.Dispose();
            mySqlConnection=null;
        }
    }
}
