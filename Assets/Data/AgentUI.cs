using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentUI : MonoBehaviour {

    public GameObject myVoice;
    public GameObject myPositioning;

    //public GameObject bassRoot;

    public GameObject myPitchUI;

	public void setVoice(GameObject voice)
    {
        myVoice = voice;
    }

    public void setChordDepth(float depth)
    {
        myVoice.GetComponent<GenericVoiceAgent>().setDepth((int)depth);
    }

    public void setMod(int mod)
    {
        myVoice.GetComponent<FMInstrument>().setBaseWave(mod);
    }

    public void setGain(float gain)
    {
        myVoice.GetComponent<FMInstrument>().setGain(gain);
    }

    public void setStartingPitch(string pitch)
    {
        var pitchNum = int.Parse(pitch);
        myVoice.GetComponent<GenericVoiceAgent>().setStartingPitch(pitchNum);

    }

    void InitialPitch()
    {
        var pitch = myVoice.GetComponent<GenericVoiceAgent>().pitch;
        myPitchUI.GetComponent<InputField>().text = pitch.ToString();
    }

    //public void setBass()
    //{
    //    bassRoot.SetActive(true);
    //}

    public void setMovement(float movement)
    {
        myVoice.GetComponent<GenericVoiceAgent>().setMovement(movement);
    }

    public void setRoot(bool root)
    {
        myVoice.GetComponent<BassVoiceAgent>().rootPosition = root;
    }

    public void setDuration(int dur)
        {
        myVoice.GetComponent<GenericVoiceAgent>().setDuration(dur);
    }

    public void setBehaviour(int behave)
    {
        myVoice.GetComponent<GenericVoiceAgent>().setBehaviour(behave);
    }

    public void Setup(int xPos, int yPos, GameObject voice)
    {
        myVoice = voice;
        //if (myVoice) { Debug.Log("MyVoice is not null"); }
        myPositioning.transform.position = new Vector3(myPositioning.transform.position.x + xPos, myPositioning.transform.position.y + yPos);
        //setStartingPitch("63");
        InitialPitch();
        setChordDepth(0);
        setMod(0);
        setGain(50);
    }
}
