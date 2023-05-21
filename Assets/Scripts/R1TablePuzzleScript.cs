using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class R1TablePuzzleScript : MonoBehaviour
{
    GameObject selectedObject;
    Vector3 offset;

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

    List<GameObject> BS;
    List<GameObject> S;

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
            Vector3 newPosition = BS[i].transform.position;
            newPosition.x = -4.06f + (i * 2.723f);
            newPosition.y = 1.1219f;
            BS[i].transform.position = newPosition;

            Vector3 newPosition2 = S[i].transform.position;
            newPosition2.x = -4.06f + (i * 2.723f);
            newPosition2.y = -1.2781f;
            S[i].transform.position = newPosition2;
        }

    }

    // Update is called once per frame
    void Update()
    {
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
                if (collider.gameObject.name == "Background")
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

        if (bsGreenCollider.bounds.Intersects(sRedCollider.bounds) && bsYellowCollider.bounds.Intersects(sPurpleCollider.bounds) && bsOrangeCollider.bounds.Intersects(sBlueCollider.bounds) && bsBlackCollider.bounds.Intersects(sPinkCollider.bounds) && selectedObject == null)
        {
            SceneManager.LoadScene("MainScene");
            // Handle collision here
        }

    }
}
