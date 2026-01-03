using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class ShooterManager : MonoBehaviour
{
    [SerializeField] private int numberOfColumns = 2;
    [SerializeField] private float distanceColumn = .8f;
    [SerializeField] private ShooterColumn shooterColumnPrefab;
    [SerializeField] private ShooterSlot shooterSlotPrefab; 
    [SerializeField] private Vector3 shooterRowPosition = new Vector3(0, -1.5f, 0);
    [SerializeField] private Vector3 rootPosition = new Vector3(0, -2.5f, 0);

    private List<ShooterColumn> shooterColumns = new List<ShooterColumn>();
    private List<ShooterSlot> enableShooterSlots = new List<ShooterSlot>();

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

    public void InitEnableShooterRow(int numberEnableShooter)
    {
        shooterRowPosition.x = -((1f * numberEnableShooter) / 2f - .5f) * distanceColumn;
        if(enableShooterSlots.Count > 0)
        {
            foreach (ShooterSlot slot in enableShooterSlots)
            {
                Destroy(slot.gameObject);
            }
        }
        enableShooterSlots.Clear();
        for (int i = 0; i < numberEnableShooter; i++)
        {
            ShooterSlot newShooterSlot = Instantiate(shooterSlotPrefab, shooterRowPosition + new Vector3(i * distanceColumn, 0, 0), transform.rotation);
            enableShooterSlots.Add(newShooterSlot);
        }
    }

    public void SpawnShooterColumns(ShooterDataColumn[] shooterDataColumns)
    {
        shooterColumns.Clear();
        this.numberOfColumns = shooterDataColumns.Length;
        rootPosition.x = -((1f * numberOfColumns) / 2f - .5f) * distanceColumn;
        Vector3 step = new Vector3(distanceColumn, 0, 0);
        for (int i = 0; i < numberOfColumns; i++)
        {
            ShooterColumn newShooterColumn = Instantiate(shooterColumnPrefab, rootPosition + i * step, transform.rotation);
            newShooterColumn.SpawnShooters(shooterDataColumns[i].shooterDatas);
            shooterColumns.Add(newShooterColumn);
            Debug.Log("Spawned Shooter Column");
        }
    }

    public bool ShooterCanActived(float yPosition)
    {
        if (math.abs(shooterRowPosition.y - yPosition) <= .1f)
        {
            return true;
        }
        return false;
    }

    public int GetIndexAvailableShooterSlot()
    {
        for (int i = 0; i < enableShooterSlots.Count; i++)
        {
            if (enableShooterSlots[i].HaveShooter() == false)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddShooterIntoEnableRow(Shooter shooter, int index)
    {
        enableShooterSlots[index].SetShooter(shooter);
    }
}