using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Block Colors")]
    [SerializeField] public BlockDataColumn[] blockDataColumns;

    [Header("Shooter Settings")]
    [SerializeField] public ShooterDataColumn[] shooterDataColumns;
    public int numberEnableShooter;
}

[Serializable]
public class BlockDataColumn
{
    public List<Color32> colors;
}
[Serializable]
public class ShooterDataColumn
{
    public List<ShooterData> shooterDatas;
}
[Serializable]
public struct ShooterData
{
    public Color32 color;
    public int bulletCount;
}