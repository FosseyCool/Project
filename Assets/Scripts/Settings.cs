using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings")]
public class Settings : ScriptableObject
{
    public float AppleSpawnChance;
    public float appleSpawnChance
    {
        get { return AppleSpawnChance; }
    }
}
