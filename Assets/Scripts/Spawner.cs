using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private int numberOfColumns = 10;
    [SerializeField] private float distanceColumn = 0.4f;
    [SerializeField] private BlockColumn blockColumnPrefab;
    [SerializeField] private Vector3 rootPosition = new Vector3(-1.8f, 0, 0);

    private int searchIndexColumn = 0;
    private int missIndexCom = 0;
    private BlockColumn[] blockColumns;

    public static Spawner Instance { get; private set; }
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

    // Start is called before the first frame update
    void Start()
    {
        SpawnBlockColumns();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBlockColumns()
    {
        blockColumns = new BlockColumn[numberOfColumns];
        Vector3 step = new Vector3(distanceColumn, 0, 0);
        for (int i = 0; i < numberOfColumns; i++)
        {
            BlockColumn newBlockColumn = Instantiate(blockColumnPrefab, rootPosition + i * step, transform.rotation);
            blockColumns[i] = newBlockColumn;
            Debug.Log("Spawned Block Column");
        }
    }

    public int GetColumnIndexWithColor(Color color)
    {
        int answer = -1;
        while (missIndexCom < numberOfColumns)
        {
            if (blockColumns[searchIndexColumn].HeadColumnIsColor(color) == true)
            {
                missIndexCom = 0;
                answer = searchIndexColumn;
                Debug.Log("Return column have index: " + searchIndexColumn);
                searchIndexColumn = (searchIndexColumn + 1) % numberOfColumns;
                return answer;
            }
            else
            {
                Debug.Log("Missed column at index: " + searchIndexColumn);
                searchIndexColumn = (searchIndexColumn + 1) % numberOfColumns;
                missIndexCom++; 
            }
        }
        missIndexCom = 0;
        searchIndexColumn = 0;
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
