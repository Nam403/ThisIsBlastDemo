using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;
    [SerializeField] private TextMeshPro indexText;

    public bool isColor(Color targetColor)
    {
        float eps = 0.01f;
        return Mathf.Abs(color.r - targetColor.r) <= eps
        && Mathf.Abs(color.g - targetColor.g) <= eps
        && Mathf.Abs(color.b - targetColor.b) <= eps;

        /*Color32 c1 = (Color32)color;
        Color32 c2 = (Color32)targetColor;
        return c1.Equals(c2);*/
    }

    public void SetIndexText(int index)
    {
        if (indexText != null)
        {
            indexText.text = index.ToString();
        }
    }

    public void SetColor(Color newColor)
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Debug.Log("Setting block color to: " + newColor);
            sr.color = new Color(newColor.r, newColor.g, newColor.b, 1f); //Make sure alpha is 1
        }
        else
        {
            Debug.LogWarning("SpriteRenderer of block is null!");
        }
            this.color = newColor;
    }
}
