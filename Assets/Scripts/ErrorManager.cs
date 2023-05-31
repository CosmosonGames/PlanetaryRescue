using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ErrorManager : MonoBehaviour
{
    public SpriteRenderer errorImage;
    public Sprite missingkeycardorchip1;
    public Sprite missingkeycardwithembed2;
    public Sprite missingvalid3;

    public AudioSource audioSource;
    public AudioClip soundEffect;

    public void PlaySoundEffect()
    {
        audioSource.PlayOneShot(soundEffect);
    }

    public void ShowError(int errorCode)
    {
        switch (errorCode)
        {
            case 1:
                errorImage.sprite = missingkeycardorchip1;
                break;
            case 2:
                errorImage.sprite = missingkeycardwithembed2;
                break;
            case 3:
                errorImage.sprite = missingvalid3;
                break;
            default:
                Debug.LogError("Invalid error code!");
                return;
        }
        PlaySoundEffect();  
        errorImage.gameObject.SetActive(true);

    }

    public void HideError()
    {
        errorImage.gameObject.SetActive(false);
    }

    private void Update() {
        if (errorImage.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            HideError();
        }
    }
}