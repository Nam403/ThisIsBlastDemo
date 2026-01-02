using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSlot : MonoBehaviour
{
    private Shooter shooter;
    public void SetShooter(Shooter shooter)
    {
        this.shooter = shooter;
        if (shooter != null)
        {
            shooter.transform.position = transform.position;
            shooter.SetParentShooterSlot(this);
        }
    }

    public bool HaveShooter()
    {
        return shooter != null;
    }
}
