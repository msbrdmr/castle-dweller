using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator animator;
    public float raycastHeight = 1.0f;
    public float raycastLength = 0.2f;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        RaycastForward();
    }

    public void UpdateMovement(float speed)
    {
        animator.SetBool("IsRunning", speed > 0.01f);
        animator.SetFloat("Speed", speed);
    }

    public void TriggerAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void TriggerDeathAnimation()
    {
        animator.SetTrigger("Death");
    }

    private void RaycastForward()
    {
        Vector3 raycastOrigin = new(transform.position.x, transform.position.y + 1.2f, transform.position.z);
        Vector3 raycastDirection = transform.forward;

        Debug.DrawRay(raycastOrigin, raycastDirection, Color.red);

        if (Physics.Raycast(raycastOrigin, raycastDirection, out RaycastHit hit, 0.5f))
        {
            GameObject door = hit.collider.transform.parent.gameObject;
            if (door.TryGetComponent(out LockedDoorController lockedDoorController))
            {
                LockedDoorModel lockedDoorModel = lockedDoorController.model;
                KeyType keyType = lockedDoorModel.keyType;
                LevelModel levelModel = FindObjectOfType<LevelManager>().levelModel;
                Debug.Log("Key Type: " + keyType + " Level Model: " + levelModel.hasKeyRed + " " + levelModel.hasKeyBlue);

                if (lockedDoorModel.isLocked)
                {
                    if (keyType == KeyType.Red && levelModel.hasKeyRed)
                    {
                        lockedDoorController.UnlockDoor();
                    }
                    else if (keyType == KeyType.Blue && levelModel.hasKeyBlue)
                    {
                        lockedDoorController.UnlockDoor();
                    }
                }
            }

        }
    }

    
}