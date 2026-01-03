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
    private int numberEmptyColumn = 0;
    private int numberStuckedSlot = 0;
    private int maxNumberStuckedSlot = 0;

    public static event System.Action OnAllShootersStucked;
    public static event System.Action OnShootingCannotContinue;

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

    public void Clear()
    {
        foreach (ShooterColumn column in shooterColumns)
        {
            column.Clear();
            Destroy(column.gameObject);
        }
        shooterColumns.Clear();
        foreach (ShooterSlot slot in enableShooterSlots)
        {
            slot.Clear();
            Destroy(slot.gameObject);
        }
        enableShooterSlots.Clear();
    }

    public void InitEnableShooterRow(int numberEnableShooter)
    {
        numberStuckedSlot = 0;
        maxNumberStuckedSlot = numberEnableShooter;
        shooterRowPosition.x = -((1f * numberEnableShooter) / 2f - .5f) * distanceColumn;
        for (int i = 0; i < numberEnableShooter; i++)
        {
            ShooterSlot newShooterSlot = Instantiate(shooterSlotPrefab, shooterRowPosition + new Vector3(i * distanceColumn, 0, 0), transform.rotation);
            enableShooterSlots.Add(newShooterSlot);
        }
    }

    public void SpawnShooterColumns(ShooterDataColumn[] shooterDataColumns)
    {
        numberEmptyColumn = 0;
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

    public void UpdateNumberStuckedSlot(int num)
    {
        numberStuckedSlot += num;
        Debug.Log("Number of stucked slots increased to " + numberStuckedSlot);
        if (numberStuckedSlot >= maxNumberStuckedSlot)
        {
            Debug.Log("All shooters are stucked! Game Over!");
            OnAllShootersStucked?.Invoke();
        }
    }

    public void UpdateNumberEmptyColumn()
    {
        numberEmptyColumn++;
        Debug.Log("Number of empty shooter columns: " + numberEmptyColumn);
        if (numberEmptyColumn == shooterColumns.Count)
        {
            Debug.Log("All shooter columns are empty!");
            InvokeRepeating("CheckShootingIsCompleted", .5f, .5f);
        }
    }

    private void CheckShootingIsCompleted()
    {
        int emptySlotCount = 0;
        for(int i = 0; i < enableShooterSlots.Count; i++)
        {
            if (enableShooterSlots[i].HaveShooter() == false)
            {
                emptySlotCount++;
            }
        }
        Debug.Log("Checking shooting status: empty slots = " + emptySlotCount + ", stucked slots = " + numberStuckedSlot);
        if (emptySlotCount == maxNumberStuckedSlot)
        {
            CancelInvoke("CheckShootingIsCompleted");
        }
        if (emptySlotCount < maxNumberStuckedSlot && emptySlotCount + numberStuckedSlot == maxNumberStuckedSlot)
        {
            CancelInvoke("CheckShootingIsCompleted");
            OnShootingCannotContinue?.Invoke();
        }
    }
}