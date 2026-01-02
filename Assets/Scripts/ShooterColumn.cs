using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShooterColumn : MonoBehaviour
{
    [SerializeField] private float distanceShooter = 1f;
    [SerializeField] private Shooter shooterPrefab;
    [SerializeField] private int numberOfShooters = 1;

    private List<Shooter> shooters = new List<Shooter>();

    public void SpawnShooters(int blockCount, Color[] color, int[] bulletCounts)
    {
        numberOfShooters = blockCount;
        shooters.Clear();
        Vector3 vectorDistance = new Vector3(0, distanceShooter, 0);
        for (int i = 0; i < numberOfShooters; i++)
        {
            Shooter newShooter = Instantiate(shooterPrefab, transform.position - i * vectorDistance, transform.rotation);
            if (color[i] == null)
            {
                Debug.LogWarning("Color of shooter at index " + i + " is null!");
            }
            else
            {
                newShooter.SetColor(color[i]);
            }
            newShooter.SetBulletCount(bulletCounts[i]);
            Debug.Log("Spawned Shooter at index: " + i + " with position " + newShooter.transform.position.x + "-" + newShooter.transform.position.y);
            newShooter.SetParentShooterColumn(this);
            shooters.Add(newShooter);
        }
    }

    public void UpdateColumn()
    {
        Destroy(shooters[0].gameObject);
        shooters.Remove(shooters[0]);
        Vector3 step = new Vector3(0, distanceShooter, 0);
        for (int i = 0; i < shooters.Count; i++)
        {
            shooters[i].transform.position += step;
        }
    }

    public bool ShooterCanActived(float yPosition)
    {
        if (shooters.Count == 0) return false;
        if (math.abs(shooters[0].transform.position.y - yPosition) <= .1f)
        {
            return true;
        }
        return false;
    }
}
