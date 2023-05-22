using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Rigidbody2D charRigid;
    private float standardStrength = 75;
    private float moveStrength;
    public float debugStrength = 3;
    public LogicManagerScript logic;
    public bool puzzleEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG
        if (logic.debug)
        {
            if (Input.GetKey(KeyCode.LeftShift) | (Input.GetKey(KeyCode.RightShift)))
            {
                moveStrength = standardStrength * debugStrength;
            }
            else
            {
                moveStrength = standardStrength;
            }
        }

        if (!puzzleEnabled)
        {
            charRigid.velocity = Vector2.zero;
            if (Input.GetKey(KeyCode.W) | (Input.GetKey(KeyCode.UpArrow)))
            {
                charRigid.velocity += Vector2.up * moveStrength;
            }

            if (Input.GetKey(KeyCode.S) | (Input.GetKey(KeyCode.DownArrow)))
            {
                charRigid.velocity += Vector2.down * moveStrength;
            }

            if (Input.GetKey(KeyCode.D) | (Input.GetKey(KeyCode.RightArrow)))
            {
                charRigid.velocity += Vector2.right * moveStrength;
            }

            if (Input.GetKey(KeyCode.A) | (Input.GetKey(KeyCode.LeftArrow)))
            {
                charRigid.velocity += Vector2.left * moveStrength;
            }
        }
    }
}
