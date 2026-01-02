using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockColumn : MonoBehaviour
{
    [SerializeField] private float distanceBlock = 0.4f;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private int numberOfBlocks = 10;

    private List<Block> blocks = new List<Block>();
    private bool isEmpty = false;

    private void Update()
    {
        if(blocks.Count == 0 && isEmpty == false)
        {
            isEmpty = true;
            BlockManager.Instance.UpdateNumberOfBlocks(numberOfBlocks);
            Debug.Log("Block Column is empty, notifying Block Manager.");
        }
    }

    public void SpawnBlocks(List<Color32> colors)
    {
        isEmpty = false;
        numberOfBlocks = colors.Count;
        blocks.Clear();
        Vector3 vectorDistance = new Vector3(0, distanceBlock, 0);
        for(int i = 0; i < numberOfBlocks; i++)
        {
            Block newBlock = Instantiate(blockPrefab, transform.position + i * vectorDistance, transform.rotation);
            //newBlock.SetIndexText(i);
            if (colors[i].Equals(null))
            {
                Debug.LogWarning("Color of block at index " + i + " is null!");
            }
            else
            {
                newBlock.SetColor(colors[i]);
            }   
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
            //blocks[i].SetIndexText(i);
        }    
    }

    public bool HeadColumnIsColor(Color32 color)
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

    public void SetColorForBlock(int index, Color32 color)
    {
        if (index < 0 || index >= blocks.Count) return;
        blocks[index].SetColor(color);
    }
}
