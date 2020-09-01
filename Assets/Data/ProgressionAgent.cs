using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chord
{
    public List<int> scale { get; private set; }
    public int root { get; private set; }

    public Chord(int root, List<int> scale)
    {
        this.root = root;
        this.scale = new List<int>(scale);
    }

}

static class Scales
{
    public static List<int> Ionian = new List<int> { 0, 2, 4, 5, 7, 9, 11 };
    public static List<int> Dorian = new List<int> { 0, 2, 3, 5, 7, 9, 10 };
    public static List<int> Phrygian = new List<int> { 0, 1, 3, 5, 7, 8, 10 };
    public static List<int> Lydian = new List<int> { 0, 2, 4, 6, 7, 9, 11 };
    public static List<int> Myxolydian = new List<int> { 0, 2, 4, 5, 7, 9, 10 };
    public static List<int> Aeolian = new List<int> { 0, 2, 3, 5, 7, 8, 10 };
    public static List<int> Locrian = new List<int> { 0, 1, 3, 5, 6, 8, 10 };
}


public class ProgressionAgent : MonoBehaviour
{
    public List<GenericVoiceAgent> AllVoices;

    public int rate = 4;

    LinkedList<Chord> progression;
    LinkedListNode<Chord> currentChord;

    ProgressionKB progs;

    int beat;

    public void Beat()
    {
        foreach(GenericVoiceAgent ag in AllVoices)
        {
            ag.Beat();
        }
        beat++;
        if (beat > rate)
        {
            beat -= rate;
            NextChord();
        }

    }

    void NextChord()
    {
        var nextChord = GetNext();
        //foreach (GenericVoiceAgent ag in AllVoices)
        //{
        //    ag.newChord(nextChord);
        //}
    }

    //void TriggerChordMove()
    //{
    //    foreach(GenericVoiceAgent ag in AllVoices)
    //    {
    //        ag.TriggerChord();
    //    }
    //}

    public void AllOff()
    {
        foreach (GenericVoiceAgent ag in AllVoices)
        {
            ag.Off();
        }
    }

    public Chord GetNext()
    {
        if (currentChord.Next != null) { currentChord = currentChord.Next; }
        else { currentChord = progression.First; }
        return currentChord.Value; ;
    }

    public void loadProgression(int prog)
    {
       // Debug.Log("PRogreesion " + prog + " loaded");
        progression = new LinkedList<Chord>(progs.getProgression(prog));
        currentChord = progression.First;
    }

    public void DebugProgression()
    {
        progression = new LinkedList<Chord>();
        progression.AddFirst(new Chord(0, Scales.Ionian));
        progression.AddLast(new Chord(-3, Scales.Aeolian));
        progression.AddLast(new Chord(-7, Scales.Lydian));
        progression.AddLast(new Chord(-5, Scales.Myxolydian));
        currentChord = progression.First;
    }

    public void AddVoice(GameObject voice)
    {
        var agent = voice.GetComponent<GenericVoiceAgent>();
        AllVoices.Add(agent);
        agent.setFirstChord(currentChord.Value);
        agent.setProgAgent(this);
        //voice.GetComponent<GenericVoiceAgent>().newChord(currentChord.Value);
    }

    public void RemoveVoice()
    {
        AllVoices.RemoveAt(AllVoices.Count - 1);
    }

    public Chord chordNow()
    {
        return currentChord.Value;
    }

    public void setRate(int newRate)
    {
        rate = newRate;
    }
    public void clearVoices()
    {
        AllVoices = new List<GenericVoiceAgent>();
    }

    void Awake()
    {
        AllVoices = new List<GenericVoiceAgent>();
        progs = new ProgressionKB();
        loadProgression(0);
    }

    public void Play()
    {
        foreach(GenericVoiceAgent ag in AllVoices)
        {
            ag.TriggerChord();
        }
    }

    public void populateVoices()
    {
        //Iterate through voices with index
        for(int i = 0; i<AllVoices.Count; i++)
        {
            //Clear voice list of other voices
            AllVoices[i].clearOthers();
            //Iterate through all voices w. index again
            for(int j = 0; j<AllVoices.Count; j++)
            {
                //If the index's don't match, then this is a different agent - add it to agent i's list
                if (j != i) {
                    AllVoices[i].addOther(AllVoices[j]);
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        /*
        for(int i = 0; i<AllVoices.Count; i++)
        {
            AllVoices[i].SetAgent(i+1);
            AllVoices[i].newChord(currentChord.Value);
        }


    */
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class Progression
{
    List<Chord> chords;

    public Progression()
    {
        chords = new List<Chord>();
    }

    public Progression(Progression template)
    {
        chords = new List<Chord>(template.chords);
    }

    public void addChord(int root, List<int> scale)
    {
        chords.Add(new Chord(root, scale));
    }

    public List<Chord> getProg()
    {
        return new List<Chord>(chords);
    }
}

public class ProgressionKB
{
    List<Progression> progs;
    //Constructor initializes all the progressions into list, same order as dropdown
    public ProgressionKB()
    {
        progs = new List<Progression>();
        //I-vi-IV-V
        var newProg = new Progression();
        newProg.addChord(0, Scales.Ionian);
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

        //I-V-vi-IV
        newProg = new Progression();
        newProg.addChord(0, Scales.Ionian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(-7, Scales.Lydian);
        progs.Add(new Progression(newProg));

        //vi-IV-V-I
        newProg = new Progression();
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(0, Scales.Ionian);
        progs.Add(new Progression(newProg));

        //I-III-IV-V
        newProg = new Progression();
        newProg.addChord(0, Scales.Ionian);
        newProg.addChord(-8, Scales.Myxolydian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

        //vi-III-IV-V
        newProg = new Progression();
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(-8, Scales.Myxolydian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

        //I-IV-V-I
        newProg = new Progression();
        newProg.addChord(0, Scales.Ionian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(0, Scales.Ionian);
        progs.Add(new Progression(newProg));

        //vi-IV-V-I
        newProg = new Progression();
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(0, Scales.Ionian);
        progs.Add(new Progression(newProg));

        //Falling Fifths
        newProg = new Progression();
        newProg.addChord(0, Scales.Ionian);
        newProg.addChord(-7, Scales.Lydian);
        newProg.addChord(-1, Scales.Locrian);
        newProg.addChord(-8, Scales.Phrygian);
        newProg.addChord(-3, Scales.Aeolian);
        newProg.addChord(2, Scales.Dorian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(0, Scales.Ionian);
        progs.Add(new Progression(newProg));

        //Romanesca
        newProg = new Progression();
        newProg.addChord(3, Scales.Ionian);
        newProg.addChord(-2, Scales.Lydian);
        newProg.addChord(0, Scales.Aeolian);
        newProg.addChord(-7, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

        //Folia first half
        newProg = new Progression();
        newProg.addChord(0, Scales.Aeolian);
        newProg.addChord(-5, Scales.Myxolydian);
        newProg.addChord(0, Scales.Aeolian);
        newProg.addChord(-2, Scales.Myxolydian);
        newProg.addChord(3, Scales.Ionian);
        newProg.addChord(-2, Scales.Myxolydian);
        newProg.addChord(0, Scales.Aeolian);
        newProg.addChord(-5, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

        //Andalusian
        newProg = new Progression();
        newProg.addChord(0, Scales.Aeolian);
        newProg.addChord(-2, Scales.Myxolydian);
        newProg.addChord(-4, Scales.Lydian);
        newProg.addChord(-5, Scales.Myxolydian);
        progs.Add(new Progression(newProg));

    }

    public List<Chord> getProgression(int prog)
    {
        return progs[prog].getProg();
    }
}

