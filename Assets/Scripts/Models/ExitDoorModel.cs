using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ExitDoorModel", menuName = "ScriptableObjects/ExitDoorModel", order = 1)]
public class ExitDoorModel : ScriptableObject
{
    private int remainingEnemyCount;
    private bool isLocked;
    [SerializeField] private GameObject RedBarPrefab;
    private GameObject[] RedBars;
    private Transform RedBarTransform;
    void Start()
    {
        for (int i = 0; i < remainingEnemyCount; i++)
        {
            GameObject obj = Instantiate(RedBarPrefab, new Vector3(RedBarTransform.position.x, RedBarTransform.position.y + i * 0.5f, RedBarTransform.position.z), RedBarTransform.rotation);

            RedBars.Append(obj);
        }

    }

    void showRedBarsOnExitDoor()
    {

    }


}
