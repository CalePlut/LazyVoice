using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMInstrument : MonoBehaviour {
	[Header("Waves")]
	public BaseWave BaseOscillator;
	public List<Modulator> Modulators;

	[Header("Debug and Testing")]
	public int startingPitch;

	public void SetPitch(int pitch){
		BaseOscillator.setPitch (pitch);
		foreach (Modulator mod in Modulators) {
			mod.setPitch (pitch);
		}

	}

    public void setBaseWave(int modType)
    {
        BaseOscillator.setWave((waveType)modType);
    }

    public void increment()
    {
        BaseOscillator.increment();
    }

    public void off()
    {
        BaseOscillator.off();
    }

    public void setGain(float gain)
    {
        BaseOscillator.setGain(gain);
    }

    public void changePitch(int pitch)
    {
        BaseOscillator.setPitch(pitch);
    }

    public float dataReturn()
    {
        return BaseOscillator.dataReturn();
    }

    
    
	void Awake(){

        if (Modulators.Count > 0)
        {
            BaseOscillator.setMod(Modulators[0]);
            for (int i = 0; i < Modulators.Count - 1; i++)
            {
                Modulators[i].setMod(Modulators[i + 1]);
            }
        }

        BaseOscillator.setupChain();
        //BaseOscillator.setIncrement();
        BaseOscillator.setPitch(startingPitch);
	}


}


