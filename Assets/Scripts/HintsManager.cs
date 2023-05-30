using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsManager : MonoBehaviour
{
    public GameObject r1p1;
    private R1TablePuzzleScript r1p1Script;
    public GameObject r1p2;
    private R1PhotoDevelopingScript r1p2Script;
    public GameObject r1p3;
    private R1Anagrams r1p3Script;
    public GameObject r2p1;
    private ShootingPuzzle r2p1Script;
    public GameObject r2p2;
    private WeaponsQuiz r2p2Script;
    public GameObject r2p3;
    public GameObject r3p1;
    public GameObject r3p2;
    public GameObject r3p3;

    public GameObject slowTypeObject;
    private SlowType slowType;

    private int currentRoom = 0;
    private int currentPuzzle = 0;
    private int currentHint = 0;

    private List<string> r1p1Hints = new List<string> {"Those paintings look interesting", "I wonder why the paintings are colored that way", "Don’t the object colors look similar to the painting colors?"};
    private List<string> r1p2Hints = new List<string> {"Try using your chip and keycard",  "The chip looks like it fits somewhere in the keycard, but how can you fuse them together?", "The machine in this room looks useful for the chip and keycard you have."};
    private List<string> r1p3Hints = new List<string> {"There must be a password you can find around the room to input into the computer", "Perhaps an object that you can write on holds the password?", "Look at the easel"};
    private List<string> r2p1Hints = new List<string> {"The asteroids are a part of your combat training", "Dodging the asteroids doesn't seem to be successful", "Destroying the asteroids seems to be the only option to succeed"};
    private List<string> r2p2Hints = new List<string> {"It looks like a matching game", "The words look like they're names to the pictures of weapons."};
    private List<string> r2p3Hints = new List<string> {"What can you only see when the sun is out?", "What is cast on the floor when the sun shines in your direction?", "What follows you everywhere you go?"};
    private List<string> r3p1Hints = new List<string> {};
    private List<string> r3p2Hints = new List<string> {};
    private List<string> r3p3Hints = new List<string> {};

    void Start() {
        slowType = slowTypeObject.GetComponent<SlowType>();
        r1p1Script = r1p1.GetComponent<R1TablePuzzleScript>();
        r1p2Script = r1p2.GetComponent<R1PhotoDevelopingScript>();
        r1p3Script = r1p3.GetComponent<R1Anagrams>();

    }

    void Update() {
        if (currentRoom >= 2) {
            currentRoom++;
            currentPuzzle = 0;
        } else {
            currentPuzzle++;
        }
    }

    private void OnMouseUp() {
        Debug.Log("Clicked");
        
    }
}
