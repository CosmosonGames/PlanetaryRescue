using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CentralControlScript : MonoBehaviour
{
    public Camera cam;
    public int strength;
    public float smoothTime = 0.0F;
    private Vector3 velocity = Vector3.zero;

    public void adjustCanvas(Transform rectTransform, SpriteRenderer spriteRenderer, int x, int y)
    {
        rectTransform.position = cam.transform.position;
        
        // Get the screen dimensions
        float screenHeight = Screen.height;
        float screenWidth = Screen.width;

        // Get the sprite dimensions
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
          
        Debug.Log(screenHeight);
        Debug.Log(screenWidth);
        
        // Calculate the new scale for the sprite
        float newScaleX = screenWidth / spriteWidth;
        float newScaleY = screenHeight / spriteHeight;

        Debug.Log(cam.fieldOfView);

        //// Get the distance between the camera and the sprite
        //float distance = Vector3.Distance(cam.transform.position, spriteRenderer.transform.position);

        //// Calculate the new scale for the sprite based on the distance from the camera
        //float newScale = Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad) * distance * 2;

        //// Set the new scale for the sprite
        //spriteRenderer.transform.localScale = new Vector3(newScaleX * newScale, newScaleY * newScale, 1);
    }

    private void Update()
    {
    }
}
