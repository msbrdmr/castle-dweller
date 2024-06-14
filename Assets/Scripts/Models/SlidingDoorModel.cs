using UnityEngine;

[CreateAssetMenu(fileName = "SlidingDoorModel", menuName = "ScriptableObjects/SlidingDoorModel", order = 1)]
public class SlidingDoorModel : ScriptableObject
{
    public DoorId id; //TODO, make this DoorId an integer instead of an enum.
    public float duration = 2.0f;
    public float originalScale = 3.0f;
    public float targetScale = 0.1f;
}
