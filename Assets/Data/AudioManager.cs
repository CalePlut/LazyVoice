using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public int tempo = 120;
    float secondsPerSixteenth;
    int sixteenthCounter;
    float metro;

    int voiceNum = 4;

    int x = -400;
    int y = 0;

    int[] startingPitches;
    int pitchCounter = 0;

    bool play;

    List<GameObject> voices;

    public GameObject voicePrefab;
    public GameObject bassPrefab;

    BassVoiceAgent bassVoice;

    public void Sixteenth()
    {
        sixteenthCounter++;
        if (sixteenthCounter >= 4)
        {
            sixteenthCounter = 0;
            Beat();
        }
    }

    public void Play()
    {
        play = true;
        GetComponent<Synthesizer>().Play();
        GetComponent<ProgressionAgent>().Play();
        StartCoroutine(metronome());
    }

    public void Pause()
    {
        play = false;
        GetComponent<Synthesizer>().Pause();
    }

    public void setRootPos(bool root)
    {
        bassVoice.rootPosition = root;
    }


    public void setTempo(int newTempo)
    {
        tempo = newTempo;
        var secondsPerBeat = 60.0f / tempo;
        secondsPerSixteenth = secondsPerBeat / 4;
    }

    public void SetVoices(float Number)
    {
        var voiceInt = Mathf.RoundToInt(Number);

        //Debug.Log("VoiceInt = " + voiceInt + ", voices.count = " + voices.Count);
        if (voiceInt > voices.Count)
        {
            for(int i = voices.Count; i<voiceInt; i++)
            {
                var pitch = startingPitches[pitchCounter];
                pitchCounter++;

                addVoice(x, y, pitch);

                x += 210;
                if (x >= 250)
                {
                    x = -400;
                    y -= 110;
                }
            }
        }
        if (voiceInt < voices.Count)
        {
            for(int i = voiceInt; i<voices.Count; i++)
            {
                pitchCounter--;
                removeVoice();
                x -= 210;
                if (x <= -410)
                {
                    x = 230;
                    y += 110;
                }
            }
        }
    }

    public void Beat()
    {
        GetComponent<ProgressionAgent>().Beat();
    }

    void addVoice(int x, int y, int pitch)
    {
        var voice = Instantiate(voicePrefab);
        voice.GetComponent<GenericVoiceAgent>().Setup(x, y, pitch);
        voices.Add(voice);


        populateOtherScripts();
    }

    void removeVoice()
    {
        var toDestroy = voices[voices.Count - 1];
        voices.Remove(toDestroy);
        //Destroy(toDestroy);
        toDestroy.GetComponent<GenericVoiceAgent>().Kill();

        populateOtherScripts();
    }

    IEnumerator metronome()
    {
        while (play)
        {
            metro += Time.deltaTime;
            if (metro >= secondsPerSixteenth)
            {
                metro -= secondsPerSixteenth;
                Sixteenth();
            }

            yield return null;
        }
    }

    void OnApplicationQuit()
    {
        GetComponent<ProgressionAgent>().AllOff();
    }

    void populateOtherScripts()
    {
        var prog = GetComponent<ProgressionAgent>();
        var synth = GetComponent<Synthesizer>();

        prog.clearVoices();
        synth.clearInst();

        foreach(GameObject obj in voices)
        {
            prog.AddVoice(obj);
            synth.AddInstrument(obj);
        }

        prog.populateVoices();
    }

    public void setKey(int _key)
    {
        if (_key > 6) { _key -= 12; }
        foreach (GameObject voice in voices)
        {
            voice.GetComponent<GenericVoiceAgent>().setKey(_key);
        }
    }

    void Start()
    {
        setTempo(120);

        startingPitches = new int[10] { 55, 60, 64, 67, 72, 76, 79, 84, 88,96 };

        voices = new List<GameObject>();
        //Create Voices
        var bassVoiceobj = Instantiate(bassPrefab);
        voices.Add(bassVoiceobj);
        bassVoice = bassVoiceobj.GetComponent<BassVoiceAgent>();
        bassVoice.GetComponent<BassVoiceAgent>().Setup(-750, 0, 48);
        SetVoices(4.0f);

        //for (int i = 0; i<voiceNum; i++)
        //{
        //    var voice = Instantiate(voicePrefab);
        //    voices.Add(voice);
        //    voice.GetComponent<GenericVoiceAgent>().Setup(x, y);
        //    x += 200;
        //}
        populateOtherScripts();
    }
}