using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterManager : MonoBehaviour
{
    [SerializeField] private int numberOfColumns = 2;
    [SerializeField] private float distanceColumn = 1f;
    [SerializeField] private ShooterColumn shooterColumnPrefab; 
    [SerializeField] private Vector3 rootPosition = new Vector3(0, -2, 0);

    private List<ShooterColumn> shooterColumns = new List<ShooterColumn>();

    public static ShooterManager Instance { get; private set; }
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

    public void SpawnShooterColumns(int numberOfColumns, Color[] shooterColors, int[] shooterCount)
    {
        this.numberOfColumns = numberOfColumns;
        int numberOfShooterPerColumn = shooterColors.Length / numberOfColumns;
        Color[] tempColor = new Color[numberOfShooterPerColumn];
        int[] tempCount = new int[numberOfShooterPerColumn];
        rootPosition.x = -((1f * numberOfColumns) / 2f - .5f) * distanceColumn;
        Vector3 step = new Vector3(distanceColumn, 0, 0);
        for (int i = 0; i < numberOfColumns; i++)
        {
            ShooterColumn newShooterColumn = Instantiate(shooterColumnPrefab, rootPosition + i * step, transform.rotation);
            for (int j = 0; j < numberOfShooterPerColumn; j++)
            {
                tempColor[j] = shooterColors[i * numberOfShooterPerColumn + j];
                tempCount[j] = shooterCount[i * numberOfShooterPerColumn + j];
            }
            newShooterColumn.SpawnShooters(numberOfShooterPerColumn, tempColor, tempCount);
            shooterColumns.Add(newShooterColumn);
            Debug.Log("Spawned Shooter Column");
        }
    }
}
