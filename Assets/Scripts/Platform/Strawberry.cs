using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public GameObject bonusGameObject;
    Animator anim;
    AudioSource triggerAudio;

    void Start()
    {
        anim=bonusGameObject.GetComponent<Animator>();
        triggerAudio=transform.parent.GetComponent<AudioSource>();
    }

    public void trigger()
    {
        anim.Play("Bonus anim");
        triggerAudio.Play();
        Destroy(gameObject);
    }
}
