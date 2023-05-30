using UnityEngine;
using TMPro;
using System.Linq;

public class ShootingPuzzle : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public GameObject asteroidPrefab;
    public float asteroidSpeed = 20f;
    public float asteroidSpawnInterval = 2f;
    public float asteroidSpawnDelay = 1f;
    public float asteroidSpawnRadius = 5f;
    public float asteroidMinSize = 0.5f;
    public float asteroidMaxSize = 1f;
    public float bulletSpeed = 500f;
    public float bulletLifetime = 1.5f;
    public float bulletCooldown = 0.5f;
    public Transform gunTransform;
    public GameObject bulletPrefab;

    private float asteroidSpawnTimer;
    private float bulletCooldownTimer;

    public int asteroidsHit = 0;

    public TextMeshPro text;
    public GameObject player;
    private CharacterControl characterControl;

    public GameObject parent;
    private ParkourScript parkourScript;

    public GameObject logicObject;
    private LogicManagerScript logic;

    void Start()
    {
        asteroidSpawnTimer = asteroidSpawnDelay;
        spriteRenderer = GetComponent<SpriteRenderer>();
        characterControl = player.GetComponent<CharacterControl>();
        parkourScript = parent.GetComponent<ParkourScript>();
        logic = logicObject.GetComponent<LogicManagerScript>();
        ToggleSpriteAndChildren(false);
    }

    void Update()
    {
        if (spriteRenderer.enabled) {
            if (!HandleMouse()) {
                return;
            }

            ToggleSpriteAndChildren(true);
            // Spawn asteroids
            asteroidSpawnTimer -= Time.deltaTime;
            if (asteroidSpawnTimer <= 0f)
            {
                SpawnAsteroid();
                asteroidSpawnTimer = asteroidSpawnInterval;
            }

            // Shoot bullets
            if (Input.GetMouseButton(0) && bulletCooldownTimer <= 0f)
            {
                ShootBullet();
                bulletCooldownTimer = bulletCooldown;
            }
            bulletCooldownTimer -= Time.deltaTime;

            text.text = (asteroidsHit/2).ToString();

            if (asteroidsHit/2 >= 15)
            {
                ToggleSpriteAndChildren(false);
                parkourScript.puzzleComplete = true;
                
                if (logic.debug) {
                    Debug.Log("Room 2, Puzzle 1 complete!");
                }
            }
        }
    }

    void SpawnAsteroid()
    {
        //FIX ISSUE WITH SPAWNING TOO CLOSE!
        Bounds bounds = GetComponent<SpriteRenderer>().bounds;
        Vector2 spawnPosition = Vector2.zero;
        float minDistanceFromGun = 10f;
        do
        {
            spawnPosition = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
        } while (Vector2.Distance(spawnPosition, gunTransform.position) < minDistanceFromGun);
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.transform.parent = transform;
        float size = Random.Range(asteroidMinSize, asteroidMaxSize);
        asteroid.transform.localScale = new Vector3(size, size, 1f);
        Rigidbody2D asteroidRigidbody = asteroid.GetComponent<Rigidbody2D>();
        asteroidRigidbody.velocity = Random.insideUnitCircle.normalized * asteroidSpeed;
        asteroid.AddComponent<Asteroid>();
    }

    void ShootBullet()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)gunTransform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, gunTransform.position, Quaternion.identity);
        bullet.transform.parent = transform;
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        bulletRigidbody.velocity = direction * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }

    void ToggleSpriteAndChildren(bool enabled)
    {
        spriteRenderer.gameObject.SetActive(enabled);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enabled);
        }
        characterControl.puzzleEnabled = enabled;
    }

    private bool HandleMouse() {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(mousePosition);

            Collider2D backgroundCollider = gameObject.GetComponent<Collider2D>();
            if (!colliders.Contains(backgroundCollider))
            {
                ToggleSpriteAndChildren(false);
                return false;
            } else {
                return true;
            }
        } else {
            return true;
        }
    }
}