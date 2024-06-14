using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    public ExitDoorView exitDoorView;

    private LevelModel levelModel;

    public void Start()
    { // Get the LevelModel from the hierarchy
        LevelManager lvlmng = GameObject.FindObjectOfType<LevelManager>();

        if (lvlmng != null)
        {
            levelModel = lvlmng.levelModel;
        }
        else
        {
            Debug.LogError("LevelManager not found in the scene");
        }

        int initialEnemyCount = levelModel.enemyCount;
        exitDoorView.Initialize(initialEnemyCount);
    }

    void Update()
    {
        int currentEnemyCount = levelModel.enemyCount;
        exitDoorView.UpdateEnemyCount(currentEnemyCount);
    }
}