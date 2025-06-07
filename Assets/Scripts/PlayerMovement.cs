using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;
    private CharacterController controller;

    private Vector3 walkMovement = new Vector3();
    private Vector3 velocity = new Vector3();
    private bool isGrounded;

    void Start()
    {
        // Automatically assigns the CharacterController component
        controller = GetComponent<CharacterController>();
        
        // Check if the CharacterController component is assigned successfully
        // NOTE: This should NOT fail due to the RequireComponent attribute. I WILL cry if it ever does.
        if (controller == null)
        {
            Debug.LogError("CharacterController component is missing!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Grounded check
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        //Get player movement input

        //Translate input into movement vector
        controller.Move(walkMovement * speed * Time.deltaTime);


        //Move player (gravity and inputs)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void setMovement(Vector2 movement)
    {
        walkMovement = transform.right * movement.x + transform.forward * movement.y;
    }

    public void jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    //Stop all ongoing movement; used when entering a menu
    public void haltMovement()
    {
        this.setMovement(new Vector2(0f, 0f));
    }
}
