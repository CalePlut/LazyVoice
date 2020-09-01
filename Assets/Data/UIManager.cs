using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public List<GameObject> HideOnPlay;

    public void Play()
    {
        foreach(GameObject obj in HideOnPlay)
        {
            obj.SetActive(false);
        }
    }

    public void Pause()
    {
        foreach(GameObject obj in HideOnPlay)
        {
            obj.SetActive(true);
        }
    }
}
