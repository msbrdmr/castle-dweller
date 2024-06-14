using UnityEngine;
using System.Collections.Generic;

public class ExitDoorView : MonoBehaviour
{
    public GameObject enemyCountBarPrefab; // Prefab for the enemy count bar (cylinder)
    public Transform barsParent; // Parent transform for positioning bars
    public int maxBars = 8; // Maximum number of bars (cylinders) that can be displayed
    public float startYPosition = 0.35f; // Starting Y position for the first bar
    public float endYPosition = 2.85f; // Ending Y position for the last bar

    public HingeJoint rightHingeJoint; // Hinge joint for the right door
    public HingeJoint leftHingeJoint; // Hinge joint for the left door

    public float openAngle = 100f;
    public float closeAngle = 0f;

    private List<GameObject> enemyCountBars = new List<GameObject>();

    private void Start()
    {
        // Set initial limits to lock both doors
        rightHingeJoint.limits = new JointLimits { min = 0, max = 0 };
        leftHingeJoint.limits = new JointLimits { min = 0, max = 0 };
    }

    public void Initialize(int initialEnemyCount)
    {
        CreateEnemyCountBars(initialEnemyCount);
    }

    public void UpdateEnemyCount(int currentEnemyCount)
    {
        // Remove excess bars if enemy count decreases
        while (enemyCountBars.Count > currentEnemyCount)
        {
            GameObject barToRemove = enemyCountBars[enemyCountBars.Count - 1];
            enemyCountBars.Remove(barToRemove);
            Destroy(barToRemove);
        }

        // Add bars if enemy count increases
        while (enemyCountBars.Count < currentEnemyCount)
        {
            CreateEnemyCountBar();
        }

        // Update positions of existing bars
        UpdateBarPositions();

        // Adjust hinge joint limits based on enemy count bars
        AdjustHingeJointLimits();
    }

    private void CreateEnemyCountBars(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            CreateEnemyCountBar();
        }
    }

    private void CreateEnemyCountBar()
    {
        GameObject bar = Instantiate(enemyCountBarPrefab, barsParent);
        enemyCountBars.Add(bar);

        // Calculate the Y position based on the number of bars and distribute them within the specified range
        float totalHeight = endYPosition - startYPosition;
        float barHeight = totalHeight / maxBars;
        float yPos = startYPosition + enemyCountBars.Count * barHeight;

        // Set local position of the bar
        bar.transform.localPosition = new Vector3(0f, yPos, 0f);
    }

    private void UpdateBarPositions()
    {
        // Recalculate positions for all bars
        float totalHeight = endYPosition - startYPosition;
        float barHeight = totalHeight / maxBars;

        for (int i = 0; i < enemyCountBars.Count; i++)
        {
            GameObject bar = enemyCountBars[i];
            float yPos = startYPosition + i * barHeight;
            bar.transform.localPosition = new Vector3(0f, yPos, 0f);
        }
    }

    private void AdjustHingeJointLimits()
    {
        // Check if there are any enemy count bars left
        if (enemyCountBars.Count == 0)
        {
            // No bars left, unlock the door
            JointLimits limits = new JointLimits();
            limits.min = -openAngle / 2f;
            limits.max = openAngle / 2f;

            if (rightHingeJoint != null)
            {
                rightHingeJoint.limits = limits;
            }

            if (leftHingeJoint != null)
            {
                leftHingeJoint.limits = limits;
            }
        }
    }
}
