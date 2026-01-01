using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockColumn : MonoBehaviour
{
    [SerializeField] private float distanceBlock = 0.4f;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private int numberOfBlocks = 10;

    private List<Block> blocks = new List<Block>();

    // Start is called before the first frame update
    void Start()
    {
        SpawnBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBlocks()
    {
        Vector3 vectorDistance = new Vector3(0, distanceBlock, 0);
        for(int i = 0; i < numberOfBlocks; i++)
        {
            Block newBlock = Instantiate(blockPrefab, transform.position + i * vectorDistance, transform.rotation);
            newBlock.SetIndexText(i);
            Debug.Log("Spawned Block at index: " + i + " with position " + newBlock.transform.position.x + "-" + newBlock.transform.position.y);
            blocks.Add(newBlock);
        }
    }

    public void UpdateColumn()
    {
        Destroy(blocks[0].gameObject);
        blocks.Remove(blocks[0]);
        Vector3 step = new Vector3(0, distanceBlock, 0);
        for (int i = 0; i < blocks.Count; i++)
        {
            blocks[i].transform.position -= step;
            blocks[i].SetIndexText(i);
        }    
    }

    public bool HeadColumnIsColor(Color color)
    {
        if (blocks.Count == 0) return false;
        if (blocks[0].isColor(color))
        {
            return true;
        }
        return false;
    }

    public int CheckColumnSize()
    {
        return blocks.Count;
    }
}
