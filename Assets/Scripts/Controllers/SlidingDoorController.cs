using System.Collections;
using UnityEngine;

public class SlidingDoorController : MonoBehaviour
{
    private bool isOpen = false;
    private bool isAnimating = false;
    private float targetEndScale;
    private float startScale;
    private float timeElapsed;

    [SerializeField] public SlidingDoorModel model;

    void FixedUpdate()
    {
        if (isAnimating)
        {
            timeElapsed += Time.deltaTime;
            float scale = Mathf.Lerp(startScale, targetEndScale, timeElapsed / model.duration);
            transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);

            if (timeElapsed >= model.duration)
            {
                transform.localScale = new Vector3(targetEndScale, transform.localScale.y, transform.localScale.z);
                isOpen = (targetEndScale == model.targetScale);
                isAnimating = false;
            }
        }
    }

    public void OpenDoor()
    {
        if (!isAnimating && !isOpen)
        {
            StartAnimation(model.targetScale);
        }
    }

    public void CloseDoor()
    {
        if (!isAnimating && isOpen)
        {
            StartAnimation(model.originalScale);
        }
    }

    private void StartAnimation(float endScale)
    {
        isAnimating = true;
        timeElapsed = 0f;
        startScale = transform.localScale.x;
        targetEndScale = endScale;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}
