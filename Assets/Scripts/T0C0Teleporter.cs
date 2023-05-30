using System.Collections;
using UnityEngine;

public class T0C0Teleporter : MonoBehaviour
{
    Collider2D contactCollider;
    private bool canTeleport = true;
    public GameObject logic;
    private LogicManagerScript logicManager;

    private float centerY;
    private float topY;
    private float bottomY;
    private bool roomComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        contactCollider = GetComponent<Collider2D>();
        logicManager = logic.GetComponent<LogicManagerScript>();

        Vector2 center = contactCollider.bounds.center;
        Vector2 size = contactCollider.bounds.size;
        centerY = center.y;
        bottomY = center.y - size.y / 2;
        topY = center.y + size.y / 2;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && canTeleport)
        {
            SpriteRenderer spriteRenderer = other.gameObject.GetComponent<SpriteRenderer>();
            float height = spriteRenderer.sprite.texture.height / spriteRenderer.sprite.pixelsPerUnit;
            height = 0;
            Vector3 targetPosition;
            if (other.gameObject.transform.position.y > centerY)
            {
                targetPosition = new Vector3(other.gameObject.transform.position.x, bottomY - 0.1f*height, other.gameObject.transform.position.z);
                if (!roomComplete) {
                    logicManager.EndRoomTime(0);
                }
                roomComplete = true;
            } else {
                targetPosition = new Vector3(other.gameObject.transform.position.x, topY + 0.1f*height, other.gameObject.transform.position.z);
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
