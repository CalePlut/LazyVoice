using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum waveType{
Sine,Square, Tri, Saw
}

public interface wave{
	void setPitch (int midiPitch);
	void setFrequency(float freq);

    void setInc();
	void increment();
	float dataReturn ();


}

//Version of Modulator for inspector and UI
[System.Serializable]
public class Modulator{
	Oscillator osc;
    Modulator mod;

	[Header("Wave Settings")]
	public waveType WaveForm;
	public float Ratio;
	public float relativeGain;

	public float getMod(){
        if (mod != null) { return osc.dataReturn(mod); }

        return osc.dataReturn ();
	}

    public void setMod(Modulator mod)
    {
        this.mod = mod;
    }

    public void setWave(waveType wave)
    {

        WaveForm = wave;
        switch (wave){
            case waveType.Sine:
                osc = new Sine();
                break;
            case waveType.Square:
                osc = new Square();
                break;
            case waveType.Tri:
                osc = new Triangle();
                break;
            case waveType.Saw:
                osc = new Sawtooth();
                break;
        }
    }

	public void setPitch(int pitch){
		var freq = midiToFreq (pitch);
		freq*=Ratio;
		osc.setFrequency (freq);
        setIncrement();
        //Passes incoming pitch to modulator
        if (mod != null)
        {
            mod.setPitch(pitch);
        }
    }

    public void off()
    {
        osc.setGain(0.0f);
        if (mod != null) { mod.off(); }
    }

    public void setGain(float g)
    {
        relativeGain*=g;
        osc.setGain(relativeGain);
        if (mod != null) { mod.setGain(g); }
    }


    public void increment()
    {
        if (mod != null)
        {
            mod.increment();
        //    osc.increment(mod);
        }
        //else
        //{
            osc.increment();
        //}
    }

    public void setIncrement()
    {
        osc.setInc();
        if (mod != null)
        {

            mod.setIncrement();
        }

    }

    public void setupChain()
    {

        switch (WaveForm)
        {
            case waveType.Saw:
                osc = new Sawtooth();
                break;
            case waveType.Sine:
                osc = new Sine();
                break;
            case waveType.Square:
                osc = new Square();
                break;
            case waveType.Tri:
                osc = new Triangle();
                break;
        }

        osc.setGain(relativeGain);

        if (mod != null) { mod.setupChain(); }
    }

    float midiToFreq(int pitch){
		var flPitch = (float)pitch;
		var toPower = (flPitch - 69)/12;
		var freq = Mathf.Pow (2, toPower) * 440;
		return freq;
	}

}

//Versions of the waves for Editor and UI
[System.Serializable]
public class BaseWave{
	Oscillator osc;
    Modulator mod;

    public void setMod(Modulator mod)
    {
        this.mod = mod;
    }

	[Header("Wave Settings")]
	public waveType WaveForm;
    public float gain;

	public void setPitch(int pitch){
		osc.setPitch (pitch);
        setIncrement();
        if (mod != null)
        {
            mod.setPitch(pitch);
        }
	}

    public void setWave(waveType wave)
    {
        WaveForm = wave;
        switch (wave)
        {
            case waveType.Sine:
                osc = new Sine();
                break;
            case waveType.Square:
                osc = new Square();
                break;
            case waveType.Tri:
                osc = new Triangle();
                break;
            case waveType.Saw:
                osc = new Sawtooth();
                break;
        }
    }

    public void increment()
    {
        if (mod != null)
        {
            mod.increment();
           // osc.increment(mod);
        }
 //       else
 //       {
            osc.increment();
            //Debug.Log("Incrementing with no modulator");
 //       }
    }

    public float dataReturn()
    {
        if (mod != null) { return osc.dataReturn(mod); }
        return osc.dataReturn();
    }

    public void setIncrement()
    {
        osc.setInc();
        if (mod != null)
        {
            mod.setIncrement();
        }
    }

    public void off()
    {
        if (osc != null)
        {
            osc.setGain(0.0f);
        }
        if (mod != null) { mod.off(); }
    }

    public void setGain(float g)
    {
        gain = g;
        osc.setGain(g);
        if (mod != null) { mod.setGain(g); }
    }

    public void setupChain()
    {
        switch (WaveForm)
        {
            case waveType.Saw:
                osc = new Sawtooth();
                break;
            case waveType.Sine:
                osc = new Sine();
                break;
            case waveType.Square:
                osc = new Square();
                break;
            case waveType.Tri:
                osc = new Triangle();
                break;
        }
        osc.setGain(gain);

        if (mod != null) { mod.setupChain(); }
    }
}

