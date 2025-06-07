using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameButton : MonoBehaviour
{
    [SerializeField] private ButtonManager buttonManager = null;

    // Start is called before the first frame update
    void Start()
    {
        //Quick and dirty fix due to changes in how I handle terminal buttons
        if (buttonManager == null)
        {
            buttonManager = transform.parent.gameObject.GetComponent<ButtonManager>();
        }

        if (buttonManager == null)
        {
            Debug.LogWarning("Error! GameButton must be a child of a ButtonManager!");
        }
    }

    public virtual void onClick()
    {
        // Debug.Log("Button clicked!");
        buttonManager.onButtonClick();
    }
}
