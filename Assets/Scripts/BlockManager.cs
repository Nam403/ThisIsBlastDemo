using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private int numberOfColumns = 10;
    [SerializeField] private float distanceColumn = 0.4f;
    [SerializeField] private BlockColumn blockColumnPrefab;
    [SerializeField] private Vector3 rootPosition = new Vector3(0, 0, 0);

    // Use these two variables to search column with each color
    private Dictionary<Color32, int> searchIndexColumns = new Dictionary<Color32, int>();
    private Dictionary<Color32, int> missIndexComs = new Dictionary<Color32, int>();
    
    private BlockColumn[] blockColumns;
    private int numberOfBlocks;
    private bool levelCompleted = false;

    public static BlockManager Instance { get; private set; }
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

    private void Update()
    {
        if (numberOfBlocks == 0 && levelCompleted == false)
        {
            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        levelCompleted = true;
        Debug.Log("Level Completed!");
        // Implement level completion logic here
    }

    public void InitDictionaryForSearch(BlockDataColumn[] blockDataColumns)
    {
        searchIndexColumns.Clear();
        missIndexComs.Clear();
        foreach (BlockDataColumn column in blockDataColumns)
        {
            foreach(Color32 color in column.colors)
            if (!searchIndexColumns.ContainsKey(color))
            {
                searchIndexColumns[color] = 0;
                missIndexComs[color] = 0;
            }
        }
    }

    public void UpdateNumberOfBlocks(int num)
    {
        numberOfBlocks -= num;
    }

    public void SpawnBlockColumns(BlockDataColumn[] blockDataColumns)
    {
        levelCompleted = false;
        numberOfBlocks = 0;
        for(int i = 0; i < blockDataColumns.Length; i++)
        {
            numberOfBlocks += blockDataColumns[i].colors.Count;
        }
        this.numberOfColumns = blockDataColumns.Length;
        rootPosition.x = -((1f * numberOfColumns) / 2f - .5f) * distanceColumn;
        blockColumns = new BlockColumn[numberOfColumns];
        Vector3 step = new Vector3(distanceColumn, 0, 0);
        for (int i = 0; i < numberOfColumns; i++)
        {
            BlockColumn newBlockColumn = Instantiate(blockColumnPrefab, rootPosition + i * step, transform.rotation);
            blockColumns[i] = newBlockColumn;
            blockColumns[i].SpawnBlocks(blockDataColumns[i].colors);
            Debug.Log("Spawned Block Column");
        }
    }

    public int GetColumnIndexWithColor(Color32 color)
    {
        int answer = -1;
        while (missIndexComs[color] < numberOfColumns)
        {
            if (blockColumns[searchIndexColumns[color]].HeadColumnIsColor(color) == true)
            {
                missIndexComs[color] = 0;
                answer = searchIndexColumns[color];
                Debug.Log("Return column have index: " + searchIndexColumns[color]);
                searchIndexColumns[color] = (searchIndexColumns[color] + 1) % numberOfColumns;
                return answer;
            }
            else
            {
                Debug.Log("Missed column at index: " + searchIndexColumns[color]);
                searchIndexColumns[color] = (searchIndexColumns[color] + 1) % numberOfColumns;
                missIndexComs[color]++; 
            }
        }
        missIndexComs[color] = 0;
        searchIndexColumns[color] = 0;
        return answer;
    }

    public void CheckAllColumnSize()
    {
        for (int i = 0; i < numberOfColumns; i++)
        {
            Debug.Log("Column at index " + i + " have " + blockColumns[i].CheckColumnSize() + " blocks");
        }
    }

    public Vector3 GetColumnPositionWithId(int index)
    {
        if (index >= 0 && index < numberOfColumns)
        {
            return blockColumns[index].transform.position;
        }
        else
        {
            Debug.LogWarning("Index out of range in GetColumnPosition");
            return Vector3.zero;
        }
    }

    public void UpdateColumnWithId(int index)
    {
        blockColumns[index].UpdateColumn();
    }
}
