using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private Bounds parentBounds;
    private ShootingPuzzle shootingPuzzle;

    void Start()
    {
        shootingPuzzle = transform.parent.GetComponent<ShootingPuzzle>();
        Physics2D.gravity = new Vector2(0f, -50f);
        parentBounds = transform.parent.GetComponent<SpriteRenderer>().bounds;
    }

    void Update()
    {
        if (!parentBounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
            shootingPuzzle.asteroidsHit ++;
        }
    }
}