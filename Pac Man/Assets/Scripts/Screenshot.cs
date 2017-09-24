using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    public float waitTime;
    public int scale;
    public string filename;
    private int i = 0;

    // Use this for initialization
    private void Start()
    {
        InvokeRepeating("Capture", waitTime, waitTime);
    }

    private void Capture()
    {
        //yield return new WaitForSeconds(waitTime);
        Application.CaptureScreenshot(filename + i + ".png", scale);
        i++;
        print("capture: " + i);
    }
}