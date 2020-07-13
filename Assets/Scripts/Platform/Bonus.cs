using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    void DestoryMyself()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
