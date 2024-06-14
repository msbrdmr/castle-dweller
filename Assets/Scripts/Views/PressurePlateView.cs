using UnityEngine;

public class PressurePlateView : MonoBehaviour
{
    public SpriteRenderer iconRenderer;
    public Sprite lockedIcon;
    public Sprite unlockedIcon;

    private void Start()
    {
        iconRenderer.sprite = lockedIcon;
    }

    public void ShowLockedIcon()
    {
        iconRenderer.sprite = lockedIcon;
    }

    public void ShowUnlockedIcon()
    {
        iconRenderer.sprite = unlockedIcon;
    }
}
