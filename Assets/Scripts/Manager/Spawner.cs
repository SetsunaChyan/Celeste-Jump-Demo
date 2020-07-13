using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 所有能生成的平台
    public List<GameObject> platforms=new List<GameObject>();
    // 各种附加的预制件
    public List<GameObject> addons=new List<GameObject>();
    // 草莓预制件
    public GameObject strawberry;

    // 平台生成的间隔
    public float spawnTime;
    // 经过的时间
    float countTime;
    // 有1/x的概率生成一个草莓
    public float strawberryFrequency;

    void Start()
    {
        // 一开始就生成一个
        countTime=spawnTime;
    }

    void Update()
    {
        SpawnPlatform();
        
        // 随着时间的推移，生成刺的概率将加大
        if((long)MainSceneEventManager.getTimeScore()+MainSceneEventManager.getBonus()>10000&&addons.Count<4)
            addons.Add(addons[addons.Count-1]);
    }

    public void SpawnPlatform()
    {
        countTime+=Time.deltaTime;
        Vector3 spawnPos=transform.position;
        spawnPos.x=Random.Range(-1.6f,1.6f);

        if(countTime>=spawnTime)
        {
            CreateStrawberry();
            CreatePlatform(spawnPos);
            // 可以生成两个
            if(Mathf.Abs(spawnPos.x)>=1.2f)
            {
                Vector3 tmp=spawnPos;
                float deltaX=3f;
                if(spawnPos.x<=-1.2f) tmp.x+=deltaX;
                else tmp.x-=deltaX;
                CreatePlatform(tmp);
            }
            countTime=0;
        }
    }

    void CreateStrawberry()
    {
        Vector3 pos=transform.position;
        pos.y+=1f;
        pos.x=Random.Range(-1.6f,1.6f);
        if(Random.Range(0,strawberryFrequency)<=1)
        {
            GameObject berry=Instantiate(strawberry,pos,Quaternion.identity) as GameObject;
            berry.transform.parent=this.transform;
        }
    }

    void CreatePlatform(Vector3 pos)
    {
        // 随机一个平台
        int id=Random.Range(0,platforms.Count);
        // 生成一个实例
        GameObject platform=Instantiate(platforms[id],pos,Quaternion.identity) as GameObject;
        platform.transform.parent=this.transform;
        
        // 生成刺、或者弹簧、或者不生成
        int addonId=Random.Range(0,addons.Count);
        if(addons[addonId]!=null)
        {
            List<Transform> list=new List<Transform>();
            foreach(Transform child in platform.transform)
                list.Add(child);
            if(list.Count==0) list.Add(platform.transform);

            float upperX=0f,lowerX=0f;
            if(id==0)
            {
                lowerX=-0.5f;
                upperX=0.5f;
            }
            else if(id==1)
            {
                lowerX=-0.75f;
                upperX=0.75f;
            }

            int faId=Random.Range(0,list.Count);

            Vector3 addonPos=list[faId].position;
            addonPos.y+=0.5f;
            addonPos.x+=Random.Range(lowerX,upperX);
            GameObject obj=Instantiate(addons[addonId],addonPos,Quaternion.identity) as GameObject;
            obj.transform.parent=list[faId].transform;
        }
    }
}
