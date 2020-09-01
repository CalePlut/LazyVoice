using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour {

    //AT CURRENT SCALE: Difference between notes it 0.13
	

        public void SetPitch(int pitch)
    {
       // Debug.Log("Pitch = " + pitch);
        //Middle C is hardcoded at y=2.3
        var y = 2.3f;
        var baseOctave = 60;
        if (pitch >= 62) {
            baseOctave = 71;
            y = 2.42f; }
        if (pitch <= 59) {
            y = 0.3f;
            baseOctave = 47;
        }
    //    Debug.Log("Therefore, y starting point is " + y);
        var sharp = false;
        //D(62) is at 2.55 y
        var pitchClass = pitch % 12;
      //  Debug.Log("Pitch class = " + pitchClass); 
        switch (pitchClass)
        {
            case 0:
                break;
            case 1:
                sharp = true;
                break;
            case 2:
                y += 0.13f;
                break;
            case 3:
                y += 0.13f;
                sharp = true;
                break;
            case 4:
                y += 0.26f;
                break;
            case 5:
                y += 0.39f;
                break;
            case 6:
                y += 0.39f;
                sharp = true;
                break;
            case 7:
                y += 0.52f;
                break;
            case 8:
                y += 0.52f;
                sharp = true;
                break;
            case 9:
                y += 0.65f;
                break;
            case 10:
                y += 0.65f;
                sharp = true;
                break;
            case 11:
                y += 0.78f;
                break;
        }
        //Debug.Log("Therefore, y adjusted up to " + y);
        var octaveTest = pitch;


        while (octaveTest > baseOctave)
        {
            //Debug.Log("OctaveTest(" + octaveTest + ") is greater than baseOctave(" + baseOctave + "), adjusting note up one octave");
            octaveTest -= 12;
            y += 0.91f;
           // Debug.Log("new Y is " + y);
        }
     //   Debug.Log("Setting position y to " + y);
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
