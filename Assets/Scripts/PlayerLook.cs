using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLook : MonoBehaviour
{
    // public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    public Camera playerEyes;

    private float xRotation = 0.0f;
    private float yRotation = 0.0f;

    // public Camera playerCamera; // Reference to the player's camera
    public LayerMask buttonLayerMask; // Layer mask to filter button objects

    // Start is called before the first frame update
    void Start()
    {
        //Lock cursor to game window
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GamestateManager.state)
        {
            case GamestateManager.Gamestate.firstPerson:
                mouseLook();
                break;
        }
    }

    void mouseLook()
    {
        //Get mouse movement
        xRotation += Input.GetAxis("Mouse X") * KeybindManager.instance.mouseSensitivity * Time.deltaTime;
        yRotation += Input.GetAxis("Mouse Y") * KeybindManager.instance.mouseSensitivity * Time.deltaTime;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);
        
        //Rotate camera and body
        playerEyes.transform.localRotation = Quaternion.Euler(-yRotation, 0, 0); //x-rot only affect camera
        transform.localRotation = Quaternion.Euler(0, xRotation, 0);            //y-rot affects whole body
    }

    public void interact()
    {
        // Debug.Log("Attempting raycast...");
        // Perform a raycast from the center of the screen
        Ray ray = playerEyes.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buttonLayerMask))
        {
            // Debug.Log("Collided with an object! Object: " + hit.collider);
            // Debug.Log("Looking at the screen at point: " + hit.point);

            // Check if the hit object has a GameButton component
            GameButton button = hit.collider.GetComponent<GameButton>();
            if (button != null)
            {
                // Simulate a button click
                button.onClick();
            }
        }
    }

    //Called when a key is used
    public bool itemInteract(LayerMask interactLayerMask)
    {
        // Debug.Log("Attempting raycast...");
        // Perform a raycast from the center of the screen
        Ray ray = playerEyes.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactLayerMask))
        {
            // Debug.Log("Collided with an object! Object: " + hit.collider);
            // Debug.Log("Looking at the screen at point: " + hit.point);

            // Check if the hit object has a GameButton component
            SecondaryInteractable interactable = hit.collider.GetComponent<SecondaryInteractable>();
            if (interactable != null)
            {
                // Simulate a button click
                interactable.secondaryInteract();
                return true;
            }
        }
        return false;
    }

    public bool isLookingAt(GameObject target, LayerMask layerMask, out Vector3 hitPos)
    {
        // Cast a ray from the center of the camera's viewport
        Ray ray = playerEyes.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        hitPos = new Vector3();

        // Check if the ray hits the screen object
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            hitPos = hit.point; // Get the coordinates of the point on the screen
            // Debug.Log("Looking at the screen at point: " + hit.point);
            if (hit.collider.gameObject == target)
            {
                // Player is looking at the screen
                // Debug.Log("Looking at the screen at point: " + hit.point);
                return true;
            }
        }
        return false;
    }
}
