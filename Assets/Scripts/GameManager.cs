using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour { 
    [SerializeField] LevelData levelData;
    [SerializeField] int level = 1;
    [SerializeField] BlockManager blockManager;
    [SerializeField] ShooterManager shooterManager;
    
    // Start is called before the first frame update
    void Start()
    {
        SetUpLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpLevel(int level)
    {
        this.level = level;
        //levelData = Resources.Load<LevelData>($"ScriptableObjects/LevelData{level}");

        blockManager.SpawnBlockColumns(levelData.blockColumnCount, levelData.blockColors);
        blockManager.InitDictionaryForSearch(levelData.shooterColors);
        shooterManager.SpawnShooterColumns(levelData.shooterColumnCount, levelData.shooterColors, levelData.shooterBulletCounts);
    }
}
