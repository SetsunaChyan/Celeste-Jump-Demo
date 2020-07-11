using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    Vector3 movement;
    const float speed=1.5f;
    GameObject buttomLine;

    void Start()
    {
        movement.y=speed;
        buttomLine=GameObject.Find("TopLine");
    }

    void Update()
    {
        movePlatform();
    }

    void movePlatform()
    {
        // 平台向下固定移动
        transform.position+=movement*Time.deltaTime;
        // 如果超过边界则销毁
        if(transform.position.y>=buttomLine.transform.position.y)
            Destroy(gameObject);
    }
}
