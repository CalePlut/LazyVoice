using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatorDefinitions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Sine : Oscillator
{
    public override void setInc()
    {
        base.setInc();
        inc = frequency * 2 * Mathf.PI / sampling_frequency;
    }

    public override float dataReturn()
    {
        var toReturn = gain * Mathf.Sin(phase);
        if (phase > 2 * Mathf.PI) { phase = 0; }
        return toReturn;
        // return base.dataReturn();
    }

    public override float dataReturn(Modulator mod)
    {
        var toReturn = gain * Mathf.Sin(phase+mod.getMod());
        if (phase > 2 * Mathf.PI) { phase = 0; }
        return toReturn;
    }

}

public class Sawtooth : Oscillator
{
    public override void setInc()
    {
        base.setInc();

        inc = frequency * 2 / sampling_frequency;

    }

    public override float dataReturn()
    {
        var toReturn = -1.0f + (gain * phase);
        if (phase > 2)
        {
            phase = 0;
            //Debug.Log("Resetting phase");
        }
        return toReturn;
    }

    public override float dataReturn(Modulator mod)
    {
        var toReturn = -1.0f + (gain * phase)+mod.getMod();
        if (phase > 2)
        {
            phase = 0;
            //Debug.Log("Resetting phase");
        }
        return toReturn;
    }

}

public class Triangle : Oscillator
{
    public override void setInc()
    {
        base.setInc();
        inc = frequency * 2 / sampling_frequency;
    }

    public override float dataReturn()
    {
        var triPhase = phase;
        if (triPhase > 1) { triPhase = 1.0f - (triPhase - 1); }
        var toReturn = -1.0f + (2 * gain * triPhase);

        //Debug.Log(toReturn);
        if (phase > 2) { phase = 0; }
        return toReturn;
    }

    public override float dataReturn(Modulator mod)
    {
        var triPhase = phase+mod.getMod();
        if (triPhase > 1) { triPhase = 1.0f - (triPhase - 1); }
        var toReturn = -1.0f + (2 * gain * triPhase);

        //Debug.Log(toReturn);
        if (phase > 2) { phase = 0; }
        return toReturn;
    }

}

public class Square : Oscillator
{
    public override void setInc()
    {
        base.setInc();
        inc = frequency * 2 * Mathf.PI / sampling_frequency;
    }

    public override float dataReturn()
    {
        var toReturn = gain * Mathf.Sign(Mathf.Sin(phase));
        if (phase > 2 * Mathf.PI) { phase = 0; }
        return toReturn;
    }

    public override float dataReturn(Modulator mod)
    {
        var toReturn = gain * Mathf.Sign(Mathf.Sin(phase)+mod.getMod());
        if (phase > 2 * Mathf.PI) { phase = 0; }
        return toReturn;
    }
}

public class Oscillator : wave
{
    protected float frequency = 440;
    protected float sampling_frequency = 48000;
    protected float phase;
    protected float gain = 0.04f;
    protected float inc;

    public void setGain(float gain)
    {
        this.gain = gain/100;
    }

    public void setPitch(int midiPitch)
    {
        frequency = midiToFreq(midiPitch);
    }

    public void setFrequency(float freq)
    {
        frequency = freq;
    }

    public virtual void setInc() { }
    public virtual void increment()
    {
        phase += inc;
    }
    public virtual void increment(Modulator mod)
    {
        //mod.increment ();
        phase += inc + mod.getMod();
    }
    public virtual float dataReturn()
    {
        return 0;
    }

    public virtual float dataReturn(Modulator mod)
    {
        return 0;
    }

    float midiToFreq(int pitch)
    {
        var flPitch = (float)pitch;
        var toPower = (flPitch - 69) / 12;
        var freq = Mathf.Pow(2, toPower) * 440;
        return freq;
    }

    void Awake()
    {
        sampling_frequency = (float)AudioSettings.outputSampleRate;
    }

}

