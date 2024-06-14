using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class EnemyController : BaseCharacterController
{
    private EnemyModel enemyModel;
    private EnemyView enemyView;
    private Rigidbody _rigidbody;
    private Transform spawnTransform;
    private Transform endTransform;
    private bool movingToEnd = true;
    private bool playerDetected = false; // Flag to check if the player has been detected

    private LevelManager levelManager;
    private LevelModel levelModel;

    public void Initialize(EnemyModel model, EnemyView view, Transform spawnTransform, Transform endTransform)
    {
        levelManager = FindObjectOfType<LevelManager>();
        enemyModel = model;
        enemyView = view;
        this.spawnTransform = spawnTransform;
        this.endTransform = endTransform;
        _rigidbody = GetComponent<Rigidbody>();

        enemyView.Initialize(model, this);

        if (!model.isStopped)
        {
            StartCoroutine(MoveBetweenPoints());
        }
        base.Initialize(enemyModel);
    }

    protected override void Start()
    {
        base.Start();
        levelModel = FindObjectOfType<LevelManager>().levelModel;
    }

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            Vector3 startPoint = movingToEnd ? spawnTransform.position : endTransform.position;
            Vector3 targetPoint = movingToEnd ? endTransform.position : spawnTransform.position;
            transform.LookAt(targetPoint);

            float distance = Vector3.Distance(startPoint, targetPoint);
            float walkingTime = distance / enemyModel.movementSpeed;
            float walkingTimePassed = 0;

            while (walkingTimePassed < walkingTime)
            {
                float progressPercentage = walkingTimePassed / walkingTime;
                _rigidbody.MovePosition(Vector3.Lerp(startPoint, targetPoint, progressPercentage));
                walkingTimePassed += Time.deltaTime;
                enemyView.UpdateMovement(enemyModel.movementSpeed);
                yield return null;
            }

            movingToEnd = !movingToEnd;
            enemyView.UpdateMovement(0);
            Quaternion initialRotation = transform.rotation;
            Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 180, 0);
            float waitTime = enemyModel.waitDuration;
            float waitTimePassed = 0;
            while (waitTimePassed < waitTime)
            {
                float waitPercentage = (waitTimePassed / waitTime) * 4;
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, waitPercentage);
                waitTimePassed += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CollisionWithPlayer(collision.collider);
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void CollisionWithPlayer(Collider playerCollider)
    {
        if (playerDetected) return; // If player already detected, do nothing
        playerDetected = true; // Mark player as detected

        StopAllCoroutines();
        enemyView.UpdateMovement(0);

        // Attack the player or die. Check levels.
        // get the level of player and compare it with the level of the enemy
        PlayerController playerController = playerCollider.GetComponent<PlayerController>();
        int playerLevel = playerController.GetPlayerModel().level;
        PlayerView playerView = playerController.GetPlayerView();
        Rigidbody playerRB = playerCollider.GetComponent<Rigidbody>();

        // GameObject particle = //find the gameObject that has the particle system component in the hierarchy
        GameObject particle = GameObject.Find("particle");

        StartCoroutine(RotateEnemyTowarrdsPlayer());

        if (playerLevel > enemyModel.level)
        {
            // player wins
            playerView.TriggerAttackAnimation();
            enemyView.UpdateMovement(0);

            //rotate the enemy, then play its death particle effect
            StartCoroutine(LayEnemy());
            StartCoroutine(ParticlePlay(particle, transform));
            Destroy(gameObject, 1f);
            Debug.Log("Player wins.");
            levelModel.EnemyKilled();
        }
        else
        {
            // enemy wins
            // Prevent player from moving
            enemyView.TriggerAttackAnimation();
            playerView.UpdateMovement(0);
            playerController.enabled = false;
            playerView.enabled = false;

            //rotate the player, then play its death particle effect
            StartCoroutine(LayPlayer(playerController));
            StartCoroutine(ParticlePlay(particle, playerController.transform));
            StartCoroutine(LayPlayer(playerController));

            //get the game manager
            GameManager gameManager = FindObjectOfType<GameManager>();
            Debug.Log("Player loses. Restarting the game.");

            StartCoroutine(RestartGame(gameManager));
        }
    }

    private IEnumerator RestartGame(GameManager gameManager)
    {
        yield return new WaitForSeconds(1f);
        gameManager.LoadCurrentLevel();
    }

    private IEnumerator ParticlePlay(GameObject particle, Transform transform)
    {
        particle.transform.position = transform.position;
        particle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(2f);
    }
    private IEnumerator LayEnemy()
    {
        //rotate its X axis by 90 degrees in 1 second
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-90, 0, 0);
        float waitTime = 1f;
        float waitTimePassed = 0;
        while (waitTimePassed < waitTime)
        {
            float waitPercentage = waitTimePassed / waitTime;
            transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, waitPercentage);
            waitTimePassed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator LayPlayer(PlayerController playerController)
    {
        Quaternion initialRotation = playerController.transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(-90, 0, 0);
        float waitTime = 1f;
        float waitTimePassed = 0;
        while (waitTimePassed < waitTime)
        {
            float waitPercentage = waitTimePassed / waitTime;
            playerController.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, waitPercentage);
            waitTimePassed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateEnemyTowarrdsPlayer()
    {
        Vector3 direction = FindObjectOfType<PlayerController>().transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float waitTime = 0.5f;
        float waitTimePassed = 0;
        while (waitTimePassed < waitTime)
        {
            float waitPercentage = waitTimePassed / waitTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, waitPercentage);
            waitTimePassed += Time.deltaTime;
            yield return null;
        }
    }

}
