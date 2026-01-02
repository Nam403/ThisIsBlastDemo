using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private int bulletCount = 50;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float shootRate = 10f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private TextMeshPro bulletCountText;
    private bool isShooting = false;
    private float shootTimer = 0f;
    private ShooterColumn parentShooterColumn;

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
                if(parentShooterColumn != null)
                {
                    parentShooterColumn.UpdateColumn();
                }
                else 
                { 
                    Debug.LogWarning("Parent Shooter Column is null!"); 
                }
                StartCoroutine(CheckAfterShootAndDestroy());
            }
        }
    }

    public void SetParentShooterColumn(ShooterColumn shooterColumn)
    {
        parentShooterColumn = shooterColumn;
    }

    private void OnMouseDown()
    {
        //ChangeShootingState();
        Invoke("ChangeShootingState", 0.5f);
    }

    private void ChangeShootingState()
    {
        if (isShooting == false && parentShooterColumn.ShooterCanActived(transform.position.y) == true)
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
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().SeekColumnWithIndex(indexTarget);
            bullet.GetComponent<Bullet>().SetSpeed(1f / shootRate);
            bulletCount--;
            bulletCountText.text = bulletCount.ToString();
        }
        else
        {
            if(indexTarget < 0)
            {
                Debug.Log("No target found for color: " + color);
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

    public void SetColor(Color newColor)
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
