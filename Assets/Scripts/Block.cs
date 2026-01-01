using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Block : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;
    [SerializeField] private TextMeshPro indexText;
    // Start is called before the first frame update
    void Start()
    {}

    // Update is called once per frame
    void Update()
    {}

    public bool isColor(Color targetColor)
    {
        return color == targetColor;
    }

    public void SetIndexText(int index)
    {
        if (indexText != null)
        {
            indexText.text = index.ToString();
        }
    }
}
