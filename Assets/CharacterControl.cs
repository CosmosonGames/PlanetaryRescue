using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Rigidbody2D charRigid;
    private float moveStrength = 25;
    public float debugStrength = 3;
    public LogicManagerScript logic;

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
                moveStrength = 25 * debugStrength;
            }
            else
            {
                moveStrength = 25;
            }
        }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
