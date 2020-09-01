using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : MonoBehaviour {

    public List<FMInstrument> instruments;
    bool play;

    public void Play() { play = true; }
    public void Pause() { play = false; }
    
    void Awake()
    {
        instruments = new List<FMInstrument>();
    }

    public void AddInstrument(GameObject voice)
    {
        instruments.Add(voice.GetComponent<FMInstrument>());
    }

    public void RemoveInstrument()
    {
        instruments.RemoveAt(instruments.Count - 1);
    }

    public void clearInst()
    {
        instruments = new List<FMInstrument>();
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        if (play)
        {
            for (var i = 0; i < data.Length; i += channels)
            {
                foreach (FMInstrument inst in instruments)
                {
                    inst.increment();
                    data[i] += inst.dataReturn() / instruments.Count;
                }

                if (channels == 2) data[i + 1] = data[i];
            }
        }

    }
	

}
