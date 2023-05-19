using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CentralControlScript : MonoBehaviour
{
    public Camera cam;
    public Vector3 camLocation;
    public Vector2 size;

    public void adjustCanvas(RectTransform rectTransform)
    {
        rectTransform.position = camLocation;
        rectTransform.sizeDelta = size;
    }

    private void Update()
    {
        camLocation = cam.WorldToScreenPoint(Vector3.zero);
        size = new Vector2(Screen.width * 0.2f, Screen.height * 0.2f);
    }
}
