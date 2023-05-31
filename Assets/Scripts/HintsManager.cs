using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintsManager : MonoBehaviour
{
    public GameObject character;
    private CharacterControl characterControl;
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

    public GameObject hintsButton;

    private SpriteRenderer hintsButtonSprite;

    public GameObject logicObject;
    private LogicManagerScript logic;

    Dictionary<string, List<string>> hints = new Dictionary<string, List<string>>()
    {{ "r0p0", new List<string> {"Those paintings look interesting", "I wonder why the paintings are colored that way", "Don't the object colors look similar to the painting colors?"}},
    { "r0p1", new List<string> { "Try using your chip and keycard", "The chip looks like it fits somewhere in the keycard, but how can you fuse them together?", "The machine in this room looks useful for the chip and keycard you have." } },
    { "r0p2", new List<string> { "There must be a password you can find around the room to input into the computer", "Perhaps an object that you can write on holds the password?", "Look at the easel" } },
    { "r1p0", new List<string> { "The asteroids are a part of your combat training", "Dodging the asteroids doesn't seem to be successful", "Destroying the asteroids seems to be the only option to succeed" } },
    { "r1p1", new List<string> { "It looks like a matching game", "The words look like they're names to the pictures of weapons." } },
    { "r1p2", new List<string> { "What can you only see when the sun is out?", "What is cast on the floor when the sun shines in your direction?", "What follows you everywhere you go?" } }};

    void Start() {
        slowType = slowTypeObject.GetComponent<SlowType>();
        r1p1Script = r1p1.GetComponent<R1TablePuzzleScript>();
        r1p2Script = r1p2.GetComponent<R1PhotoDevelopingScript>();
        r1p3Script = r1p3.GetComponent<R1Anagrams>();
        r2p1Script = r2p1.GetComponent<ShootingPuzzle>();
        r2p2Script = r2p2.GetComponent<WeaponsQuiz>();
        hintsButtonSprite = hintsButton.GetComponent<SpriteRenderer>();
        characterControl = character.GetComponent<CharacterControl>();
        logic = logicObject.GetComponent<LogicManagerScript>();

    }

    void Update() {
        if (characterControl.puzzleEnabled && !characterControl.examineEnabled) {
            // Place hints button at the top right of the player's screen
            hintsButton.GetComponent<SpriteRenderer>().enabled = true;
            hintsButton.transform.position = new Vector3(character.transform.position.x + 77.7466f, character.transform.position.y + 71.6542f, character.transform.position.z);
        } else {
            hintsButton.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnMouseUp() {
        if (characterControl.puzzleEnabled && !characterControl.examineEnabled) {
            ShowHint();
        } 
    }

    private void ShowHint() {
        CheckCurrentPuzzle();
        string hint = hints["r" + currentRoom + "p" + currentPuzzle][currentHint];
        logic.currentTime += 180;
        slowType.WriteText(hint);

        if (currentRoom >= 2) {
            currentRoom++;
            currentPuzzle = 0;
            currentHint = 0;
        } else {
            currentPuzzle++;
        }

        if (currentHint >= 2) {
            currentHint = 0;
        } else {
            currentHint++;  
        }
    }

    private void CheckCurrentPuzzle() {
        if (r1p1Script.puzzleComplete) {
            currentRoom = 1;
            currentPuzzle = 2;
        } else if (r1p2Script.puzzleComplete) {
            currentRoom = 1;
            currentPuzzle = 3;
        } else if (r1p3Script.puzzleComplete) {
            currentRoom = 2;
            currentPuzzle = 1;
        } else if (r2p1Script.puzzleComplete) {
            currentRoom = 2;
            currentPuzzle = 2;
        } else if (r2p2Script.puzzleComplete) {
            currentRoom = 2;
            currentPuzzle = 3;
        } 
    }
}
