using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButton : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Began||Input.GetKeyDown(KeyCode.R)) 
            MainSceneEventManager.RestartGame();
    }
}
