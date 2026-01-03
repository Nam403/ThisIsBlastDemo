using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour { 
    [SerializeField] private LevelData levelData;
    private int level = 1;
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
    }

    private void OnDisable()
    {
        BlockManager.OnLevelCompleted -= completeLevelUI.Show;
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
        completeLevelUI.Hide();
        level++;
        mainUI.UpdateScore();
        mainUI.ResetProcessBar();
        SetUpLevel(level);
    }

    public void RetryLevel()
    {
        completeLevelUI.Hide();
        mainUI.ResetProcessBar();
        SetUpLevel(level);
    }

    private void SetUpLevel(int level)
    {
        this.level = level;
        levelData = Resources.Load<LevelData>($"LevelDatas/LevelData{level}");
        if (levelData != null) {
            blockManager.SpawnBlockColumns(levelData.blockDataColumns);
            blockManager.InitDictionaryForSearch(levelData.blockDataColumns);
            shooterManager.SpawnShooterColumns(levelData.shooterDataColumns); 
            shooterManager.InitEnableShooterRow(levelData.numberEnableShooter);
        }
        else
        {
            Debug.LogError($"LevelData{level} not found in Resources/ScriptableObjects/");
        }
    }
}
