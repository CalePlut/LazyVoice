using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DurationUI : MonoBehaviour
{
    public Image whole, half, quarter;

    public void Start()
    {
        setWhole();
    }

    public void setWhole()
    {
        whole.color = Color.green;
        half.color = Color.black;
        quarter.color = Color.black;
    }
    public void setHalf()
    {
        whole.color = Color.black;
        half.color = Color.green;
        quarter.color = Color.black;
    }
    public void setQuarter()
    {
        whole.color = Color.black;
        half.color = Color.black;
        quarter.color = Color.green;
    }
}
