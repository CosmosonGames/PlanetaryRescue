using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C0R1Regulator : MonoBehaviour
{
    Collider2D contactCollider;
    private bool canTeleport = true;

    private float centerX;
    private float rightX;
    private float leftX;
    private bool roomStarted = false;

    public GameObject logic;
    private LogicManagerScript logicManager;

    // Start is called before the first frame update
    void Start()
    {
        contactCollider = GetComponent<Collider2D>();
        logicManager = logic.GetComponent<LogicManagerScript>();

        Vector2 center = contactCollider.bounds.center;
        Vector2 size = contactCollider.bounds.size;
        centerX = center.x;
        leftX = center.x - size.x / 2;
        rightX = center.x + size.x / 2;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && canTeleport)
        {
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            float width = spriteRenderer.sprite.texture.width / spriteRenderer.sprite.pixelsPerUnit;
            Vector3 targetPosition;
            if (other.gameObject.transform.position.x > centerX)
            {
                targetPosition = new Vector3(leftX - 0.1f*width, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
            } else {
                targetPosition = new Vector3(rightX + 0.1f*width, other.gameObject.transform.position.y, other.gameObject.transform.position.z);
                if (!roomStarted) {
                    logicManager.StartRoomTime(1);
                }
                roomStarted = true;
            }
            StartCoroutine(TeleportPlayer(other.gameObject, targetPosition));
            canTeleport = false;
            StartCoroutine(TeleportCooldown());
        }
    }

    IEnumerator TeleportPlayer(GameObject player, Vector3 targetPosition)
    {
        float duration = 0.2f;
        float elapsedTime = 0.0f;
        Vector3 startingPosition = player.transform.position;
        Collider2D[] colliders = player.GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        Physics2D.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Default"), true);
        while (elapsedTime < duration)
        {
            player.transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.transform.position = targetPosition;
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }
        Physics2D.IgnoreLayerCollision(player.layer, LayerMask.NameToLayer("Default"), false);
    }

    private IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canTeleport = true;
    }
}
