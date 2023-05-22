using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class R1TablePuzzleScript : MonoBehaviour
{

    GameObject selectedObject;
    Vector3 offset;

    public GameObject player;

    public GameObject parentObject;

    public GameObject BSGreen;
    public GameObject BSOrange;
    public GameObject BSYellow;
    public GameObject BSBlack;

    public GameObject SPurple;
    public GameObject SRed;
    public GameObject SPink;
    public GameObject SBlue;

    private BoxCollider2D bsGreenCollider;
    private BoxCollider2D bsOrangeCollider;
    private BoxCollider2D bsYellowCollider;
    private BoxCollider2D bsBlackCollider;

    private BoxCollider2D sPurpleCollider;
    private BoxCollider2D sRedCollider;
    private BoxCollider2D sPinkCollider;
    private BoxCollider2D sBlueCollider;

    private Vector3 parentPosition;
    private Quaternion parentRotation;

    private SpriteRenderer parentSprite;

    private float scaleFactor = 0.05f;

    List<GameObject> BS;
    List<GameObject> S;

    public float checkInterval = 0.1f;
    private bool isVisible = true;

    public CharacterControl characterControl;

    private IEnumerator CheckVisibility()
    {
        while (true)
        {
            bool currentVisibility = parentSprite.isVisible;

            if (currentVisibility != isVisible)
            {
                isVisible = currentVisibility;

                if (isVisible)
                {
                    characterControl.puzzleEnabled = true;
                    OnSpriteRendererEnabled();
                }
                else
                {
                    OnSpriteRendererDisabled();
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    private void OnSpriteRendererEnabled()
    {
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = true;
        }

        AdjustScale();
        AdjustLocation();
        Debug.Log("SpriteRenderer enabled");
    }

    private void OnSpriteRendererDisabled()
    {
        foreach (SpriteRenderer child in parentSprite.GetComponentsInChildren<SpriteRenderer>())
        {
            child.enabled = false;
        }
        characterControl.puzzleEnabled = false;
        Debug.Log("SpriteRenderer disabled");
    }

    // Start is called before the first frame update
    void Start()
    {
        BS = new List<GameObject> { BSGreen, BSOrange, BSYellow, BSBlack};
        S = new List<GameObject> { SPurple, SRed, SPink, SBlue};

        bsGreenCollider = BSGreen.GetComponent<BoxCollider2D>();
        bsOrangeCollider = BSOrange.GetComponent<BoxCollider2D>();
        bsYellowCollider = BSYellow.GetComponent<BoxCollider2D>();
        bsBlackCollider = BSBlack.GetComponent<BoxCollider2D>();

        sPurpleCollider = SPurple.GetComponent<BoxCollider2D>();
        sRedCollider = SRed.GetComponent<BoxCollider2D>();
        sPinkCollider = SPink.GetComponent<BoxCollider2D>();
        sBlueCollider = SBlue.GetComponent<BoxCollider2D>();

        parentPosition = parentObject.transform.position;
        parentRotation = parentObject.transform.rotation;

        parentSprite = parentObject.GetComponent<SpriteRenderer>();

        characterControl = player.GetComponent<CharacterControl>();

        AdjustScale();
        PositionObjects();
        StartCoroutine(CheckVisibility());
    }

    // Update is called once per frame
    void Update()
    {
        if (parentSprite.enabled)
        {
            HandleMouse();
            CheckIfCorrect();
        }
    }

    void PositionObjects(){

        System.Random random = new System.Random();

        // Shuffle the list using the Fisher-Yates shuffle algorithm
        for (int i = BS.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            GameObject temp = BS[i];
            BS[i] = BS[j];
            BS[j] = temp;
        }

        for (int i = S.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            GameObject temp = S[i];
            S[i] = S[j];
            S[j] = temp;
        }

        // Set the position of each sprite based on its index in the shuffled list
        for (int i = 0; i < BS.Count; i++)
        {
            Vector3 newPosition = parentPosition;
            newPosition += parentRotation * new Vector3(-4.06f + (i * 2.723f), 1.1219f, 0f);
            BS[i].transform.position = newPosition;

            Vector3 newPosition2 = parentPosition;
            newPosition2 += parentRotation * new Vector3(-4.06f + (i * 2.723f), -1.2781f, 0f);
            S[i].transform.position = newPosition2;
        }
    }

    void HandleMouse(){
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);
            foreach (Collider2D collider in colliders)
            {
                GameObject targetObject = collider.gameObject;
                if (S.Contains(targetObject)){
                    selectedObject = targetObject.transform.gameObject;
                    offset = selectedObject.transform.position - mousePosition;
                    break;
                }
            }
        }
        if (selectedObject)
        {
            // Check if the mouse position is over a collider
            Collider2D[] colliders = Physics2D.OverlapCircleAll(mousePosition, 0.1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.name == parentObject.name)
                {
                    // Adjust the position of the object to stay within the collider
                    Vector3 newPosition = mousePosition + offset;
                    Vector3 colliderPosition = collider.transform.position;
                    Vector3 colliderSize = collider.bounds.size;
                    float xMin = colliderPosition.x - colliderSize.x / 2 + selectedObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    float xMax = colliderPosition.x + colliderSize.x / 2 - selectedObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
                    float yMin = colliderPosition.y - colliderSize.y / 2 + selectedObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    float yMax = colliderPosition.y + colliderSize.y / 2 - selectedObject.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                    newPosition.x = Mathf.Clamp(newPosition.x, xMin, xMax);
                    newPosition.y = Mathf.Clamp(newPosition.y, yMin, yMax);
                    selectedObject.transform.position = newPosition;
                    break;
                }
            }
        }
        if (Input.GetMouseButtonUp(0) && selectedObject)
        {
            selectedObject = null;
        }

    }

    void CheckIfCorrect(){
        // Check if the sprites are in the correct positions & mouse is not dragging a sprite --> if so, load the next scene
        if (bsGreenCollider.bounds.Intersects(sRedCollider.bounds) && bsYellowCollider.bounds.Intersects(sPurpleCollider.bounds) && bsOrangeCollider.bounds.Intersects(sBlueCollider.bounds) && bsBlackCollider.bounds.Intersects(sPinkCollider.bounds) && selectedObject == null)
        {
            //SceneManager.LoadScene("MainScene");
            // Handle collision here
        }

    }

    void AdjustScale()
    {
        float cameraSize = Camera.main.orthographicSize;
        float spriteSize = parentSprite.bounds.size.y;

        Debug.Log(cameraSize);
        Debug.Log(spriteSize);

        float scale = (cameraSize * 2f) / spriteSize * scaleFactor;
        parentSprite.transform.localScale = new Vector3(scale, scale, 1f);

    }

    void AdjustLocation()
    {
        parentObject.transform.position = player.transform.position;
    }
}