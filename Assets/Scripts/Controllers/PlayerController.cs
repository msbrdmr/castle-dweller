using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private Rigidbody _rigidbody;
    private FixedJoystick joystick;
    public void Initialize(PlayerModel model, PlayerView view)
    {
        joystick = FindObjectOfType<FixedJoystick>();
        playerModel = model;
        playerView = view;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleRotation();
        HandleAttack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        Vector3 velocity = direction * playerModel.movementSpeed;

        _rigidbody.MovePosition(_rigidbody.position + velocity * Time.fixedDeltaTime);
        playerView.UpdateMovement(velocity.magnitude);
    }

    private void HandleRotation()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            Quaternion newRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, playerModel.rotationSpeed * Time.deltaTime);
        }
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerView.TriggerAttackAnimation();
        }
    }


}
