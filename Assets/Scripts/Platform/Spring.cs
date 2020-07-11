using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    // public float jumpPower;
    Animator anim;
    bool isTriggered=false;

    void Start()
    {
        anim=GetComponent<Animator>();
    }

    public void trigger()
    {
        // 在动画结束前不会再次检测到这个弹簧
        isTriggered=true;
        anim.SetBool("isTriggered",true);
        GetComponent<BoxCollider2D>().enabled=false;
    }

    public void resume()
    {
        isTriggered=false;
        anim.SetBool("isTriggered",false);
        GetComponent<BoxCollider2D>().enabled=true;
    }
}
