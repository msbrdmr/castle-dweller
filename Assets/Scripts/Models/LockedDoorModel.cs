using UnityEngine;

[CreateAssetMenu(fileName = "LockedDoorModel", menuName = "ScriptableObjects/LockedDoorModel", order = 1)]
public class LockedDoorModel : ScriptableObject
{
    public KeyType keyType;
    public bool isLocked = true;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

}
