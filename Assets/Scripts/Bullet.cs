using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int indexColumnTarget = -1;
    [SerializeField] private float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (indexColumnTarget < 0)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = BlockManager.Instance.GetColumnPositionWithId(indexColumnTarget) - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    public void SeekColumnWithIndex(int targetIndex)
    {
        this.indexColumnTarget = targetIndex;
    }

    public void SetSpeed(float time)
    {
        Vector3 dir = BlockManager.Instance.GetColumnPositionWithId(indexColumnTarget) - transform.position;
        speed = dir.magnitude / time + 1f;
    }

    private void HitTarget()
    {
        Destroy(gameObject);
        // Implement what happens when the bullet hits the target
        BlockManager.Instance.UpdateColumnWithId(indexColumnTarget);
    }
}
