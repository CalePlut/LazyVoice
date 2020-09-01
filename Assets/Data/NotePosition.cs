using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotePosition : MonoBehaviour
{
    public List<GameObject> noteSpawnLocations;

    public List<NoteToSpawn> noteSpawns;

    public Vector3 returnPosition(int pitch)
    {
        for(int i = 0; i<noteSpawns.Count; i++)
        {
            if (noteSpawns[i].pitchCompare(pitch))
            {
                return noteSpawns[i].gameObject.transform.position;
            }
        }
        return Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {
        noteSpawns = new List<NoteToSpawn>();
        foreach(GameObject ob in noteSpawnLocations)
        {
            noteSpawns.Add(ob.GetComponent<NoteToSpawn>());
        }
    }

}

public static class Accidentals
{
    public static List<int> sharps = new List<int>(20) { 37, 39, 42, 44, 46, 49, 51, 54, 56, 58, 61, 63, 66, 68, 70, 73, 75, 78, 80, 82 };
}