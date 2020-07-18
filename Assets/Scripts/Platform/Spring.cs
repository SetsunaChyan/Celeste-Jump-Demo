using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    Animator anim;
    bool isTriggered=false;
    AudioSource triggerAudio;

    void Start()
    {
        anim=GetComponent<Animator>();
        triggerAudio=GetComponent<AudioSource>();
    }

    public void trigger()
    {
        // 在动画结束前不会再次检测到这个弹簧
        if(isTriggered) return;
        isTriggered=true;
        anim.SetBool("isTriggered",true);
        GetComponent<BoxCollider2D>().enabled=false;

        // 播放音效
        triggerAudio.Play();
    }

    public void resume()
    {
        isTriggered=false;
        anim.SetBool("isTriggered",false);
        GetComponent<BoxCollider2D>().enabled=true;
    }
}
