using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BassVoiceAgent : GenericVoiceAgent
{
    public bool rootPosition = true;
    //FMInstrument inst;

    public override void Setup(int xPos, int yPos, int startingPitch)
    {
        base.Setup(xPos, yPos, startingPitch);
        // myUI.GetComponent<AgentUI>().setBass();
    }

    public override void newChord(Chord chord)
    {
        if (rootPosition)
        {
            pitch = chord.root + 48 + key;
            //set pitch here!
            var inst = GetComponent<FMInstrument>();
            inst.SetPitch(pitch);
            createNote(pitch);
            //note.SetPitch(pitch);
            //Remember to override here as well

            //auxOut.SendData(agentNum, pitch, 100);
        }
        else
        {
            base.newChord(chord);
        }
        //   }

        //   // Use this for initialization
        //   void Start () {
        //       //inst = GetComponent<FMInstrument>();
        //}

        //// Update is called once per frame
        //void Update () {
    }
}