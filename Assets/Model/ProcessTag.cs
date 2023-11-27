using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessTag : MonoBehaviour
{
    private Color color;

    public Color Color
    {
        get 
        {             
            if(color == Color.clear)
                SetColor();
            return this.color; 
        }
        set { this.color = value; }
    }

    private void Start()
    {
        SetColor();
    }

    private void SetColor()
    {
        Color = this.GetComponent<SpriteRenderer>().color;
    }
}
