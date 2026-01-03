using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSlot : MonoBehaviour
{
    private Shooter shooter;
    private bool isStucked = false;
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

    public bool IsStucked()
    {
        return isStucked;
    }

    public void Clear()
    {
        if (shooter != null)
        {
            Destroy(shooter.gameObject);
            shooter = null;
        }
        isStucked = false;
    }


    public void SetStuckedState(bool stucked)
    {
        if(stucked == true && isStucked == false)
        {
            ShooterManager.Instance.UpdateNumberStuckedSlot(1);
        }
        if(stucked == false && isStucked == true)
        {
            ShooterManager.Instance.UpdateNumberStuckedSlot(-1);
        }
        isStucked = stucked;
    }
}
