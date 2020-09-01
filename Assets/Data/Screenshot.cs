using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Screenshot : MonoBehaviour
{
    public int resolutionMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            var identifier = Random.Range(0, 1001);
            var fileName = "Screenshot" + identifier+".png";
            ScreenCapture.CaptureScreenshot(fileName, resolutionMultiplier);
            Debug.Log("Captured screenshot");
        }
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }
    }
}
