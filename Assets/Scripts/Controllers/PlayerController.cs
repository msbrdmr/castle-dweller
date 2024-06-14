using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : BaseCharacterController
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private Rigidbody _rigidbody;
    private FloatingJoystick joystick;
    private GameManager gameManager;

    private int defaultLevel = 5;

    private bool isFinished = false;
    public void Initialize(PlayerModel model, PlayerView view)
    {
        playerModel = model;
        playerModel.level = defaultLevel;
        gameManager = FindObjectOfType<GameManager>();
        playerView = view;
        joystick = FindObjectOfType<FloatingJoystick>();
        _rigidbody = GetComponent<Rigidbody>();
        base.Initialize(playerModel);
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        HandleRotation();
        HandleAttack();

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Level Up!");
            playerModel.level++;
            SetLevelText(playerModel.level);
        }
        CheckFinish();
    }
    // check finish, if global Z is greater than 10.5, then finish the game show level finished text
    private void CheckFinish()
    {
        if (transform.position.z > 9.5f && !isFinished)
        {
            gameManager.FinishLevel();
            isFinished = true;
        }
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
            Vector3 moveDirection = new(joystick.Horizontal, 0, joystick.Vertical);
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

    public PlayerModel GetPlayerModel()
    {
        return playerModel;
    }

    public PlayerView GetPlayerView()
    {
        return playerView;
    }
}