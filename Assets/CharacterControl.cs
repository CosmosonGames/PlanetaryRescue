using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Rigidbody2D charRigid;
    private float moveStrength = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
