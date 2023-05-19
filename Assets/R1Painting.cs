using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R1Painting : MonoBehaviour
{
    public CentralControlScript canvasControlScript;
    public RectTransform canvasRectTransform;
    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
    }

    private void OnMouseUp()
    {
        Debug.Log("Triggered mouse up");
        if (!canvas.enabled)
        {
            canvas.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canvas.enabled)
        {
            canvasControlScript.adjustCanvas(canvasRectTransform);
        }
    }
}
