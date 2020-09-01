using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericVoiceAgent : MonoBehaviour {
    //public int agentNum;
    //public bool rootPosition = true; //Used only for bass line

    public int pitch; //{ get; protected set; }
                      //public NoteManager note;
    [Range(1,4)]
    public int duration = 4;

    ProgressionAgent prog;

    public int minPitch = 36, maxPitch=84;

    public GameObject note;
        // public AudioManager auxOut;
    List<GenericVoiceAgent> others;
    bool converge = false;
    bool expand = false;
    bool avoid = false;
    float movement = 0;
    [Range(0, 5)]
    public int chordDepth = 4;
    int beat=0;
    protected int key = 0;


    int tempo;

    Chord currentChord;

    //UI stuff
    public GameObject UIPrefab;
    protected GameObject myUI;

    //variables for negotiating with other agents
    int rejectedPitch;
    //    Chord currentChord;

    //public void SetAgent(int num)
    //{
    //    agentNum = num;
    //    //MidiPlayer.Play(new ProgramChange(0, (byte)agentNum, Instrument));
    //    inst = GetComponent<FMInstrument>();
    //}


    public void setDepth(int depth)
    {
        chordDepth = depth;
    }

    public void setProgAgent(ProgressionAgent ag)
    {
        prog = ag;
    }

    public void setKey(int _key)
    {
        key = _key;
    }

    public void setStartingPitch(int startingPitch)
    {
        pitch = startingPitch;
    }

    public void clearOthers()
    {
        others = new List<GenericVoiceAgent>();
    }

    public void addOther(GenericVoiceAgent agent)
    {
        others.Add(agent);
    }

    public void setDuration(int dur) {
        duration = dur;
    }
    
    public void Beat()
    {
        beat++;
        //This is the logic to have differing durations
        if (beat > duration)
        {
            beat -= duration;
            newChord(prog.chordNow());
        }
    }

    public void Off()
    {
        var inst = GetComponent<FMInstrument>();
        inst.off();
        
    }

    public void TriggerChord()
    {
        var inst = GetComponent<FMInstrument>();
        inst.SetPitch(pitch);
        createNote(pitch);
        //note.SetPitch(pitch);
        //place note spawning code here.
    }

    public void setBehaviour(int behave)
    {
        switch (behave)
        {
            //none
            case 0:
                avoid = false;
                converge = false;
                expand = false;
                break;
            //Avoid
            case 1:
                avoid = true;
                converge = false;
                expand = false;
                break;
            //Converge
            case 2:
                avoid = false;
                converge = true;
                expand = false;
                break;
            //Expand
            case 3:
                avoid = false;
                converge = false;
                expand = false;
                break;
        }
    }

    public void setMovement(float mvmt)
    {
        movement = mvmt;
    }

    protected void createNote(int notePitch)
    {
        var newNote = Instantiate(note, this.transform);
        newNote.GetComponent<NoteController>().Setup(notePitch, duration);
    }

    public void setFirstChord(Chord firstChord)
    {
        currentChord = firstChord;
    }

    public virtual void newChord(Chord chord)
    {
        if (currentChord != (chord))
        {
            currentChord = chord;
        }

        pitch = AdjustPitch(pitch, chord.root, acceptablePitches(currentChord));
        // Debug.Log("Pitch for " + agentNum + " = " + pitch);
        if (avoid)
        {
            var otherPitches = new List<int>();
            foreach (GenericVoiceAgent ag in others)
            {
                otherPitches.Add(ag.pitch);
            }
            pitch = Negotiate(otherPitches, chord);
        }

        while (pitch > maxPitch) { pitch -= 12; }
        while (pitch < minPitch) { pitch += 12; }

        TriggerChord();
    }

    public virtual void Setup(int xPos, int yPos, int startingPitch)
    {
        setStartingPitch(startingPitch);

        var UI = Instantiate(UIPrefab, transform.position, Quaternion.identity);

  //      UI.GetComponent<AgentUI>().setVoice(this.gameObject);

        UI.GetComponent<AgentUI>().Setup(xPos, yPos, this.gameObject);

        myUI = UI;
    }

    public virtual void Kill()
    {
        Destroy(myUI);
        Destroy(this.gameObject);
    }

    List<int> acceptablePitches(Chord chord)
    {
        var pitches = new List<int>();
        if (chordDepth > 3) { pitches = new List<int>(chord.scale); }
        else
        {
            pitches.Add(chord.scale[0]);
            pitches.Add(chord.scale[2]);
            pitches.Add(chord.scale[4]);
            if (chordDepth > 2)
            {
                pitches.Add(chord.scale[6]);
                pitches.Add(chord.scale[1]);
            }
        }
        return pitches;
    }

    int Negotiate(List<int> other, Chord currentChord)
    {
        if (other.Contains(pitch))
        {
            if (!other.Contains(rejectedPitch))
            {
                return rejectedPitch;
            }
            else
            {
                var testchange = Random.Range(-1, 1);
                if (testchange < 0) { pitch -= 2; }
                else { pitch += 2; }
                return AdjustPitch(pitch, currentChord.root, acceptablePitches(currentChord));
            }

        }
        else { return pitch; }
    }

    //Adjusts incoming pitch to fit in the chord. Also can coordinate with other agents to avoid collisions, expand, or converge
    int AdjustPitch(int incomingPitch, int root, List<int> potentialPitches)
    {
        var distanceToNewPitch = 0;
        var newPitch = incomingPitch;
        var scale = new List<int>(potentialPitches);

        var toMove = false;
        //Reduces pitch to pitch class to test against scale
        var toCheck = (incomingPitch - root) % 12;

        //Adds chance for spontaneous movement
        var movementChance = Random.Range(0, 101);
        if (movementChance < movement)
        {
            toMove = true;
        }
        //if (movementChance<movement)
        //{
        //    //Debug.Log("Movement = " + movement + ", movementChance (lower for movement) = " + movementChance);
        //    var whichWay = Random.Range(0, 101);
        //    if (whichWay < 50)
        //    {
        //        toCheck -= 2;
        //    }
        //    else
        //    {
        //        toCheck += 2;
        //    }
        //}

        if (scale.Contains(toCheck)) {
            if (!toMove)
            {
                return incomingPitch;
            }
            else
            {
                scale.Remove(toCheck);
            }
        }
        
        //Finds nearest two neighbouring pitches - one above, one below.
        var neighbours = findNeighbours(toCheck, scale);

        //Checks to see if the pitch is above the average, used for converge and expand
        if (converge || expand)
        {

            var averageIncoming = 0;
            foreach (GenericVoiceAgent other in others)
            {
                averageIncoming += other.pitch;
            }
            averageIncoming /= others.Count;
            //If we're above the average, we converge on converge, and expand on expand
            if (averageIncoming > pitch)
            {
                if (expand) { distanceToNewPitch = 0 - neighbours[0]; }
                if (converge) { distanceToNewPitch = neighbours[1]; }
            }
            if (averageIncoming < pitch)
            {
                if (expand) { distanceToNewPitch = neighbours[1]; }
                if (converge) { distanceToNewPitch = 0 - neighbours[0]; }
            }

            newPitch = incomingPitch + distanceToNewPitch;
            return newPitch;
        }

        if (neighbours[0] == neighbours[1])
        {
            if (Random.value < 0.5) { newPitch = incomingPitch - neighbours[0]; }
            else { newPitch = incomingPitch + neighbours[1]; }
        }
        if (neighbours[0] < neighbours[1]) {
            newPitch = incomingPitch - neighbours[0];
            rejectedPitch = incomingPitch + neighbours[1];
        }
        if (neighbours[1] < neighbours[0]) {
            newPitch = incomingPitch + neighbours[1];
            rejectedPitch = incomingPitch - neighbours[0];
        }

        
        return newPitch;
    }

    //Returns distance to nearest neighbours. THESE DISTANCES ARE ALWAYS POSITIVE
    int[] findNeighbours(int incomingPitch, List<int> scale)
    {
        var upperDistance = 12;
        var lowerDistance = 12;
        foreach (int i in scale)
        {
            var up = i - incomingPitch;
            var down = incomingPitch - i;

            //if the pitch is lower, check how much lower
            if (i < incomingPitch)
            {

                //if it's lower than the current lowerDistance, it's distance becomes the new lowerDistance
                if (down < lowerDistance)
                {
                    lowerDistance = down;
                }
            }
            //if the pitch is higher, check how much higher
            if (i > incomingPitch)
            {
                if (up < upperDistance)
                {
                    upperDistance = up;
                }
            }
        }
        //In edge cases where a neighbour hasn't been found(e.g. scale degree 10, 11, 0), we need to return the disatnce to the other end
        if (lowerDistance == 12) { lowerDistance = incomingPitch - (scale[scale.Count - 1] - 12); }
        if (upperDistance == 12) { upperDistance = (scale[0] + 12) - incomingPitch; }
       //Debug.Log("Found two neighbours: +" + upperDistance + ", -" + lowerDistance);

        return new int[2] { lowerDistance, upperDistance };
    }

    
}
