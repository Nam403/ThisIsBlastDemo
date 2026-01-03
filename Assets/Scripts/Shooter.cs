using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Shooter : MonoBehaviour
{
    [SerializeField] private int bulletCount = 50;
    [SerializeField] private Color32 color;
    [SerializeField] private float shootRate = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TextMeshPro bulletCountText;
    private bool isShooting = false;
    private float shootTimer = 0f;
    private ShooterColumn parentShooterColumn;
    private ShooterSlot parentShooterSlot;
    

    // Start is called before the first frame update
    void Start()
    {
        bulletCountText.text = bulletCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShooting == true)
        {
            if (shootTimer >= 1f / shootRate)
            {
                Shoot();
                shootTimer = 0f;
            }
            else
            {
                shootTimer += Time.deltaTime;
            }
            
            if (bulletCount <= 0)
            {
                parentShooterSlot.SetShooter(null);
                Destroy(gameObject);
                StartCoroutine(CheckAfterShootAndDestroy());
            }
        }
    }

    public void SetParentShooterColumn(ShooterColumn shooterColumn)
    {
        parentShooterColumn = shooterColumn;
    }

    public void SetParentShooterSlot(ShooterSlot shooterSlot)
    {
        parentShooterSlot = shooterSlot;
    }

    private void OnMouseDown()
    {
        int index = ShooterManager.Instance.GetIndexAvailableShooterSlot();
        if (parentShooterColumn != null && parentShooterColumn.IsHeadOfColumn(this) && index != -1)
        {
            ShooterManager.Instance.AddShooterIntoEnableRow(this, index);
            parentShooterColumn.UpdateColumn();
        }
        Invoke("ChangeShootingState", 0.5f);
    }

    private void ChangeShootingState()
    {
        if (isShooting == false && ShooterManager.Instance.ShooterCanActived(transform.position.y) == true)
        {
            Debug.Log("Enabling shooting");
            isShooting = true;
        }
    }

    private void Shoot()
    {
        int indexTarget = BlockManager.Instance.GetColumnIndexWithColor(color);
        if (indexTarget >= 0 && bulletCount > 0)
        {
            Debug.Log("Shooting");
            parentShooterSlot.SetStuckedState(false);
            // Rotate towards target
            Vector3 dir = BlockManager.Instance.GetColumnPositionWithId(indexTarget) - transform.position;
            dir.z = 0;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
            // Instantiate bullet
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SeekColumnWithIndex(indexTarget);
            bullet.GetComponent<Bullet>().SetSpeed(1f / shootRate);
            bulletCount--;
            bulletCountText.text = bulletCount.ToString();
        }
        else
        {
            transform.rotation = Quaternion.identity;
            if (indexTarget < 0)
            {
                transform.rotation = Quaternion.identity;
                parentShooterSlot.SetStuckedState(true);
                Debug.Log("Slot is Stucked. No target found for color: " + color);
            }
            else
            {
                Debug.Log("No bullets left to shoot.");
            }
        }
    }

    private IEnumerator CheckAfterShootAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        BlockManager.Instance.CheckAllColumnSize();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetColor(Color32 newColor)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Debug.Log("Setting shooter color to: " + newColor);
            sr.color = new Color(newColor.r, newColor.g, newColor.b, 1f); //Make sure alpha is 1
        }
        else
        {
            Debug.LogWarning("SpriteRenderer of shooter is null!");
        }
        this.color = newColor;
    }

    public void SetBulletCount(int count)
    {
        this.bulletCount = count;
        bulletCountText.text = bulletCount.ToString();
    }
}
