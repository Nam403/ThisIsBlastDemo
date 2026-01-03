using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour { 
    [SerializeField] private LevelData levelData;
    private int level = 2;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private ShooterManager shooterManager;
    [SerializeField] private CompleteLevelUI completeLevelUI;
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private MainUI mainUI;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        BlockManager.OnLevelCompleted += completeLevelUI.Show;
        ShooterManager.OnAllShootersStucked += gameOverUI.Show;
        ShooterManager.OnShootingCannotContinue += gameOverUI.Show;
    }

    private void OnDisable()
    {
        BlockManager.OnLevelCompleted -= completeLevelUI.Show;
        ShooterManager.OnAllShootersStucked -= gameOverUI.Show;
        ShooterManager.OnShootingCannotContinue -= gameOverUI.Show;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpLevel(level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel()
    {
        blockManager.Clear();
        shooterManager.Clear();
        completeLevelUI.Hide();
        level++;
        mainUI.UpdateLevelText(level);
        mainUI.UpdateScore();
        mainUI.ResetProcessBar();
        SetUpLevel(level);
    }

    public void RetryLevel()
    {
        blockManager.Clear();
        shooterManager.Clear();
        gameOverUI.Hide();
        mainUI.ResetProcessBar();
        SetUpLevel(level);
    }

    private void SetUpLevel(int level)
    {
        this.level = level;
        mainUI.UpdateLevelText(level);
        levelData = Resources.Load<LevelData>($"LevelDatas/LevelData{level}");
        if (levelData != null) {
            blockManager.SpawnBlockColumns(levelData.blockDataColumns);
            blockManager.InitDictionaryForSearch(levelData.blockDataColumns);
            shooterManager.SpawnShooterColumns(levelData.shooterDataColumns); 
            shooterManager.InitEnableShooterRow(levelData.numberEnableShooter);
        }
        else
        {
            Debug.LogError($"LevelData{level} not found!");
        }
    }
}
