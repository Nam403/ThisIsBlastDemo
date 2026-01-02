using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Block Colors")] 
    public Color[] blockColors;
    public int blockColumnCount;

    [Header("Shooter Settings")]
    public int shooterColumnCount;
    public Color[] shooterColors;
    public int[] shooterBulletCounts;
}
