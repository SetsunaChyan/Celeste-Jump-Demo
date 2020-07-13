using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strawberry : MonoBehaviour
{
    public GameObject bonusGameObject;
    Animator anim;

    void Start()
    {
        anim=bonusGameObject.GetComponent<Animator>();    
    }

    public void trigger()
    {
        anim.Play("Bonus anim");
        Destroy(gameObject);
    }
}
