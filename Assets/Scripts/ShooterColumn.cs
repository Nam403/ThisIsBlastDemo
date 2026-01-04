using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShooterColumn : MonoBehaviour
{
    [SerializeField] private float distanceShooter = 1f;
    [SerializeField] private Shooter shooterPrefab;

    private List<Shooter> shooters = new List<Shooter>();

    public void Clear()
    {
        foreach (Shooter shooter in shooters)
        {
            Destroy(shooter.gameObject);
        }
        shooters.Clear();
    }

    public void SpawnShooters(List<ShooterData> shooterDatas)
    {
        int numberOfShooters = shooterDatas.Count;
        Vector3 vectorDistance = new Vector3(0, distanceShooter, 0);
        for (int i = 0; i < numberOfShooters; i++)
        {
            Shooter newShooter = Instantiate(shooterPrefab, transform.position - i * vectorDistance, transform.rotation);
            if (shooterDatas[i].color.Equals(null))
            {
                Debug.LogWarning("Color of shooter at index " + i + " is null!");
            }
            else
            {
                newShooter.SetColor(shooterDatas[i].color);
            }
            newShooter.SetBulletCount(shooterDatas[i].bulletCount);
            Debug.Log("Spawned Shooter at index: " + i + " with position " + newShooter.transform.position.x + "-" + newShooter.transform.position.y);
            newShooter.SetParentShooterColumn(this);
            shooters.Add(newShooter);
        }
    }

    public void UpdateColumn()
    {
        shooters.Remove(shooters[0]);
        if (shooters.Count == 0)
        {
            Clear();
            ShooterManager.Instance.UpdateListColumn(this);
            Destroy(gameObject);
        }
        else 
        {
            Vector3 step = new Vector3(0, distanceShooter, 0);
            for (int i = 0; i < shooters.Count; i++)
            {
                shooters[i].transform.position += step;
            }
        }      
    }

    public bool IsHeadOfColumn(Shooter shooter)
    {
        if (shooters.Count > 0 && shooters[0] == shooter)
        {
            return true;
        }
        return false;
    }
}
