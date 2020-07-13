using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brokenblock : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim=GetComponent<Animator>();
    }

    public void trigger()
    {
        anim.SetBool("isTriggered",true);
    }

    void DestoryMyself()
    {
        Destroy(gameObject);
    }
}
