using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : MonoBehaviour
{
    public List<GameObject> hairList;
    public int posCnt;
    List<Vector3> posList;
    PlayerController playerController;
    float countTime;
    // 头发更新的间隔
    public float hairFlowInterval;

    void Start()
    {
        posList=new List<Vector3>();
        for(int i=0;i<posCnt;i++)
            posList.Add(transform.position);
        playerController=GetComponent<PlayerController>();
    }

    void Update()
    {
        if(playerController.isDead) return;

        countTime+=Time.deltaTime;
        if(countTime<hairFlowInterval) return;
        countTime-=hairFlowInterval;

        Vector3 curPos=transform.position;
        posList.RemoveAt(0);
        posList.Add(curPos);
        
        // 依次设置每个头发的位置
        for (int i=0;i<hairList.Count;i++)
        {
            int index=i;
            hairList[hairList.Count-i-1].transform.position=posList[index];
        }
    }
}
