using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomProg : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var menu = GetComponent<Dropdown>();
        var max = menu.options.Count;
        var whichProg = Random.Range(0, max);
        menu.value = whichProg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
