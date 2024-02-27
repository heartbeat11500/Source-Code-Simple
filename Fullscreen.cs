using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fullscreen : MonoBehaviour
{
    public void Change()
    {
        Screen.fullScreen =!Screen.fullScreen;
        print("change screen mode");
    }
}