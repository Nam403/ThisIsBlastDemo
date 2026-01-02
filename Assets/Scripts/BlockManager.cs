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

    public void InitDictionaryForSearch(Color[] colors)
    {
        searchIndexColumns.Clear();
        missIndexComs.Clear();
        foreach (Color color in colors)
        {
            if (!searchIndexColumns.ContainsKey((Color32)color))
            {
                searchIndexColumns[(Color32)color] = 0;
                missIndexComs[(Color32)color] = 0;
            }
        }
    }

    public void SpawnBlockColumns(int numberOfColumns, Color[] blockColors)
    {
        this.numberOfColumns = numberOfColumns;
        int numberOfBlocksPerColumn = blockColors.Length / numberOfColumns;
        Color[] tempColor = new Color[numberOfBlocksPerColumn];
        rootPosition.x = -((1f * numberOfColumns) / 2f - .5f) * distanceColumn;
        blockColumns = new BlockColumn[numberOfColumns];
        Vector3 step = new Vector3(distanceColumn, 0, 0);
        for (int i = 0; i < numberOfColumns; i++)
        {
            BlockColumn newBlockColumn = Instantiate(blockColumnPrefab, rootPosition + i * step, transform.rotation);
            blockColumns[i] = newBlockColumn;
            for(int j = 0; j < numberOfBlocksPerColumn; j++)
            {
                tempColor[j] = blockColors[i * numberOfBlocksPerColumn + j];
            }
            blockColumns[i].SpawnBlocks(numberOfBlocksPerColumn, tempColor);
            Debug.Log("Spawned Block Column");
        }
    }

    public int GetColumnIndexWithColor(Color color)
    {
        int answer = -1;
        Color32 color32 = (Color32)color;
        while (missIndexComs[color32] < numberOfColumns)
        {
            if (blockColumns[searchIndexColumns[color32]].HeadColumnIsColor(color) == true)
            {
                missIndexComs[(Color32)color] = 0;
                answer = searchIndexColumns[color32];
                Debug.Log("Return column have index: " + searchIndexColumns[color32]);
                searchIndexColumns[color32] = (searchIndexColumns[color32] + 1) % numberOfColumns;
                return answer;
            }
            else
            {
                Debug.Log("Missed column at index: " + searchIndexColumns[color32]);
                searchIndexColumns[color32] = (searchIndexColumns[color32] + 1) % numberOfColumns;
                missIndexComs[color32]++; 
            }
        }
        missIndexComs[color32] = 0;
        searchIndexColumns[color32] = 0;
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
