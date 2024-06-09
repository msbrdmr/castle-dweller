using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
}
