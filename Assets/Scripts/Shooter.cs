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
                StartCoroutine(CheckAfterShootAndDestroy());
            }
        }
    }

    private void OnMouseDown()
    {
        //ChangeShootingState();
        Invoke("ChangeShootingState", 0.5f);
    }

    private void ChangeShootingState()
    {
        if (isShooting == false)
        {
            Debug.Log("Enabling shooting");
            isShooting = true;
        }
    }

    private void Shoot()
    {
        int indexTarget = Spawner.Instance.GetColumnIndexWithColor(color);
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
            Debug.Log("Can not shoot!");
        }
    }

    private IEnumerator CheckAfterShootAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Spawner.Instance.CheckAllColumnSize();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
