using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Bounds parentBounds;

    void Start()
    {
        parentBounds = transform.parent.GetComponent<SpriteRenderer>().bounds;
    }

    void Update()
    {
        if (!parentBounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("triggered");
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(gameObject);
        }
    }
}