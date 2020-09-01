using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteToSpawn : MonoBehaviour
{
    public List<int> pitchClass;

    public bool pitchCompare(int pitch)
    {
        if (pitchClass.Contains(pitch))
        {
            return true;
        }
        else { return false; }
    }

}
