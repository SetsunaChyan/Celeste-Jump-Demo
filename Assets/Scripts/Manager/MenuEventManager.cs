using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEventManager : MonoBehaviour
{
    void Update()
    {
        if(Input.touchCount>0&&Input.GetTouch(0).phase==TouchPhase.Began||Input.GetKeyDown(KeyCode.Return)) 
            SceneManager.LoadScene("Scenes/MainScene");
    }
}
