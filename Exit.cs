using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public GameObject menuController;
    public void QuitGame()
    {
        Application.Quit();
    }
}
