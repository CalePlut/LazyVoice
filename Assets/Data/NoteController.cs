using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{

    public Sprite half, quarter, whole;
    public GameObject ledger, sharp;
    public GameObject stem;

    protected float tempo = 120.0f;
    public float speed = 0.1f;
    // Start is called before the first frame update
    public void Setup(int pitch, int duration)
    {
        var spawnManager = GameObject.FindGameObjectWithTag("NoteSpawns");
        transform.position = spawnManager.GetComponent<NotePosition>().returnPosition(pitch);


        var ren = GetComponent<SpriteRenderer>();
        var spr = ren.sprite;

        if (duration == 4) { spr = whole; }
        if (duration == 2) { spr = half; }
        if (duration == 1) { spr = quarter; }

        ren.sprite = spr;

        //Sets stem if duration is under a whole note
        if (duration < 4) {
            var stemInst = Instantiate(stem, this.transform);
            if(pitch<50)
            {
                stem.transform.localPosition = new Vector3(-1, -2, 0);
            }
            else if (pitch>60 && pitch < 71)
            {
                stem.transform.localPosition = new Vector3(-1, -2, 0);
            }
            else
            {
                stem.transform.localPosition = new Vector3(1, 2, 0);
            }
        }

        //Sets sharp if pitch is sharp
        if (Accidentals.sharps.Contains(pitch))
        {
            var sharpInst = Instantiate(sharp, this.transform);
        }

        //Sets ledger line if pitch is a ledger-lined pitch
        if (pitch < 40 || pitch == 60 || pitch > 81)
        {
            var ledgerInst = Instantiate(ledger, this.transform);
            if (pitch == 83) { ledgerInst.transform.localPosition = new Vector3(0, -0.75f, 0); }
            if (pitch == 38) { ledgerInst.transform.localPosition = new Vector3(0, 0.75f, 0); }

            if (pitch == 84)
            {
                var secondLedger = Instantiate(ledger, this.transform);
                secondLedger.transform.localPosition = new Vector3(0, -2, 0);
            }
            if (pitch == 36)
            {
                var secondLedger = Instantiate(ledger, this.transform);
                secondLedger.transform.localPosition = new Vector3(0, 2, 0);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        var xPos = transform.position.x;
        xPos -= speed * tempo * Time.deltaTime;

        var newPos = new Vector2(xPos, transform.position.y);

        if (xPos <= -4.0f)
        {
            Destroy(this.gameObject);
        }

        transform.position = newPos;
    }
}