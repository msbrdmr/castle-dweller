using System;
using TMPro;
using UnityEngine;

public class BaseCharacterController : MonoBehaviour
{
    public CharacterModel characterModel;
    public GameObject LevelTextPrefab;
    private TMP_Text levelTextTMP;

    public void Initialize(CharacterModel model)
    {
        characterModel = model;
    }
    protected virtual void Start()
    {
        GameObject levelTextObject = Instantiate(LevelTextPrefab, transform.position, Quaternion.identity);
        levelTextObject.transform.SetParent(transform);
        levelTextObject.transform.position += new Vector3(0, 2.5f, 0);
        levelTextTMP = levelTextObject.GetComponent<TMP_Text>();
        int charLevel = characterModel.level;
        levelTextTMP.text = $"{charLevel} Lv.";
    }
    protected virtual void Update()
    {
    }
    private void LateUpdate()
    {
        levelTextTMP.transform.rotation = Quaternion.Euler(30, 0, 0);
    }

    public void SetLevelText(int level)
    {
        levelTextTMP.text = $"{level} Lv.";
    }
}
