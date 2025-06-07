using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] private Color deselectedColor;
    [SerializeField] private Color selectedColor;

    void Start() {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }
    
    // GameButton calls this when clicked
    public virtual void onButtonClick() {
        Debug.Log("Virtual button click function executed. Please check the code!");
    }

    public void select()
    {
        spriteRenderer.color = selectedColor;
    }

    public void deselect()
    {
        spriteRenderer.color = deselectedColor;
    }
}
