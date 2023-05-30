using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ErrorManager : MonoBehaviour
{
    public Image errorImage;
    public Sprite errorSprite1;
    public Sprite errorSprite2;
    public Sprite errorSprite3;

    public void ShowError(int errorCode)
    {
        switch (errorCode)
        {
            case 1:
                errorImage.sprite = errorSprite1;
                break;
            case 2:
                errorImage.sprite = errorSprite2;
                break;
            case 3:
                errorImage.sprite = errorSprite3;
                break;
            default:
                Debug.LogError("Invalid error code!");
                break;
        }
        errorImage.gameObject.SetActive(true);
        AddHideErrorOnClick();
    }

    public void HideError()
    {
        errorImage.gameObject.SetActive(false);
        RemoveHideErrorOnClick();
    }

    private void AddHideErrorOnClick()
    {
        EventTrigger trigger = errorImage.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { HideError(); });
        trigger.triggers.Add(entry);
    }

    private void RemoveHideErrorOnClick()
    {
        EventTrigger trigger = errorImage.gameObject.GetComponent<EventTrigger>();
        Destroy(trigger);
    }
}