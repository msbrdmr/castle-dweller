using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Enemy Data")]
public class EnemyModel : CharacterModel
{
    public float waitDuration;
    public float sightRadius;
    public float sightAngle;
    public bool isStopped;
}
