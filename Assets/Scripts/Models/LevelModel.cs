using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelModel : ScriptableObject
{
    public string levelName;
    public int levelNumber;
    public PlayerModel player;
    public EnemyModel[] enemies;
    public int enemyCount;
    public SlidingDoorModel[] lockedDoors;
    public bool hasKeyRed = false;
    public bool hasKeyBlue = false;

    public void EnemyKilled()
    {
        enemyCount--;
    }
}
