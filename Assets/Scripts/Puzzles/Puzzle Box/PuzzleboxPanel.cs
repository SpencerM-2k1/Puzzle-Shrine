using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PuzzleboxPanel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    PuzzleboxManager manager = null;
    int panelIndex = -1;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] float speed = 1.0f;
    
    [SerializeField] bool activated = false;

    [SerializeField] float cursorEnterTime = 0f;
    private bool cursorHover;

    // Update is called once per frame
    private void Update()
    {
        Transform targetPoint = activated ? endPoint : startPoint;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (cursorHover && ((Time.time - cursorEnterTime) > 3f))
        {
            // Debug.Log("Cursor hover detected!");
        }
    }

    public void setPuzzleboxManager(PuzzleboxManager newManager, int newIndex)
    {
        if (manager == null)
        {
            manager = newManager;
            panelIndex = newIndex;
            Debug.Log("\"" + gameObject.name + "\" : PuzzleboxManager set! Index: " + panelIndex);
        }
        else
        {
            Debug.LogError("\"" + gameObject.name + "\" : This PuzzleboxPanel is already included in a PuzzleboxManager!");
        }
    }

    //==================================================
    //  IPointerClickHandler Implementation
    //==================================================
    public void OnPointerClick(PointerEventData eventData)
    {
        // Debug.Log("Clicked: " + gameObject.name);
        bool interactSuccess = manager.panelInteract(panelIndex);
        if (interactSuccess)
        {
            activated = !activated;
        }
    }

    //==================================================
    //  IPointerClickHandler Implementation
    //==================================================
    public void OnPointerEnter(PointerEventData eventData)
    {
        cursorEnterTime = Time.time;
        cursorHover = true;
    }

    //==================================================
    //  IPointerClickHandler Implementation
    //==================================================
    public void OnPointerExit(PointerEventData eventData)
    {
        cursorHover = false;
    }
}
