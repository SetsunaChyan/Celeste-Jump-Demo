using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public float speed=3f;
    public float dir=1;

    void Update()
    {
        transform.position-=new Vector3(speed*Time.deltaTime*dir,0,0);
    }

    void destoryMyself()
    {
        Destroy(gameObject);
    }
}
