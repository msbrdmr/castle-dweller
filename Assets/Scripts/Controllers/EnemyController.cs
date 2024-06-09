using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private EnemyModel enemyModel;
    private EnemyView enemyView;
    private Rigidbody _rigidbody;

    private Transform spawnTransform;
    private Transform endTransform;

    private bool movingToEnd = true;

    public void Initialize(EnemyModel model, EnemyView view, Transform spawnTransform, Transform endTransform)
    {
        enemyModel = model;
        enemyView = view;
        this.spawnTransform = spawnTransform;
        this.endTransform = endTransform;

        _rigidbody = GetComponent<Rigidbody>();

        enemyView.Initialize(model);

        if (!model.isStopped)
        {
            StartCoroutine(MoveBetweenPoints());
        }
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
                float waitPercentage = waitTimePassed / waitTime;
                transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, waitPercentage);
                waitTimePassed += Time.deltaTime;
                yield return null;
            }
            // transform.rotation = targetRotation;
        }
    }

}
