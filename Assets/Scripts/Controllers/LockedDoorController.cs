using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    private new HingeJoint hingeJoint;
    private KeyType keyType;
    private Material material;
    private Sprite lockedSprite;
    private Sprite unlockedSprite;
    private SpriteRenderer[] spriteRenderers;
    public LockedDoorModel model;

    void Start()
    {

        lockedSprite = model.lockedSprite;
        unlockedSprite = model.unlockedSprite;
        hingeJoint = GetComponentInChildren<HingeJoint>();
        keyType = model.keyType;
        material = GetComponentInChildren<Renderer>().material;

        if (keyType == KeyType.Red)
        {
            material.color = Color.red;
        }
        else if (keyType == KeyType.Blue)
        {
            material.color = Color.blue;
        }

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = lockedSprite;
        }
        model.isLocked = true;
    }
    private void UpdateHingeJoint()
    {
        if (model.isLocked)
        {
            hingeJoint.limits = new JointLimits { min = 0, max = 0 };
        }
        else
        {
            hingeJoint.limits = new JointLimits { min = -100, max = 100 };
        }
    }
    public void UnlockDoor()
    {
        model.isLocked = false;
        UpdateHingeJoint();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].sprite = unlockedSprite;
        }
    }
}
