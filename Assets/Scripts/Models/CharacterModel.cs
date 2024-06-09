using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data")]
public class CharacterModel : ScriptableObject
{
    public int level;
    public string characterName;
    public CharacterType type;
    // rotation speed of the character
    public float movementSpeed;

    public float rotationSpeed;
}

public enum CharacterType
{
    Player,
    Enemy
}
