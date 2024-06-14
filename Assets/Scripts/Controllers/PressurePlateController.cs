using UnityEngine;
using System.Collections;

public class PressurePlateController : MonoBehaviour
{
    public DoorId id;
    public PressurePlateView view;

    private bool isLocked = true;
    private Vector3 originalPosition;
    private Vector3 pressedPosition;

    private int pressCounter = 0;
    private Coroutine resetCounterCoroutine;

    private void Start()
    {
        originalPosition = transform.position;
        pressedPosition = originalPosition + new Vector3(0, -0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PressurePlateController: OnTriggerEnter");

        if (other.CompareTag("Player") && pressCounter == 0)
        {
            pressCounter++;
            if (isLocked)
            {
                FindDoorAndOpen();
            }
            transform.position = pressedPosition;
        }
    }

    private void FindDoorAndOpen()
    {
        SlidingDoorController[] doors = FindObjectsOfType<SlidingDoorController>();
        foreach (SlidingDoorController door in doors)
        {
            if (door.model.id == id)
            {
                if (!door.IsOpen())
                {
                    isLocked = false;
                    door.OpenDoor();
                    view.ShowUnlockedIcon();
                }
                break;
            }
        }
    }
}
