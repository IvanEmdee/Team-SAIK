using System;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // Public variables
    public float speed = 5f; // The speed at which the player moves
    public bool canMoveDiagonally = true; // Controls whether the player can move diagonally

    // Private variables 
    public Rigidbody2D rb { get; private set; }// Reference to the Rigidbody2D component attached to the player
    private Vector2 movement; // Stores the direction of player movement
    private bool isMovingHorizontally = true; // Flag to track if the player is moving horizontally

    public bool disableMovement = false;
    //variable for animator
    private CharacterAnimator animator;

    //Audio Manager is here (Steven)
    [SerializeField] playerAudio audio;
    [SerializeField] float stepInterval = 0.35f;
    private float stepTimer = 0f;

    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        // Prevent the player from rotating
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //(Steven) Animation setup to grab animator componenet
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    void Update()
    {
        Debug.Log("Update is being called");
        // Get player input from keyboard or controller
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // Check if diagonal movement is allowed
        if (canMoveDiagonally)
        {
            //audio logic so it doesn't spam the audio
            if (movement != Vector2.zero)
                {
                    stepTimer -= Time.deltaTime;

                    if (stepTimer <= 0f)
                    {
                        audio.playStep();
                        stepTimer = stepInterval;
                    }
                }
                else
                {
                    stepTimer = 0f;
                }
            // Set movement direction based on input
            movement = new Vector2(horizontalInput, verticalInput);
            // Optionally rotate the player based on movement direction
            RotatePlayer(horizontalInput, verticalInput);
        }
        else
        {
            // Determine the priority of movement based on input
            if (horizontalInput != 0)
            {
                isMovingHorizontally = true;
            }
            else if (verticalInput != 0)
            {
                isMovingHorizontally = false;
            }

            // Set movement direction and optionally rotate the player
            if (isMovingHorizontally)
            {
                movement = new Vector2(horizontalInput, 0);
                //RotatePlayer(horizontalInput, 0);

            }
            else
            {
                movement = new Vector2(0, verticalInput);
                //RotatePlayer(0, verticalInput);

            }
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the player in FixedUpdate for physics consistency
        if (!disableMovement)
            rb.linearVelocity = movement * speed;

        
        //(Steven) This is the animation movement update so that animation can be played base on the x and y value
        animator.IsMoving = movement != Vector2.zero;
        animator.MoveX = Mathf.RoundToInt(movement.x);
        animator.MoveY = Mathf.RoundToInt(movement.y);
    }

    void RotatePlayer(float x, float y)
    {
        // If there is no input, do not rotate the player
        //if (x == 0 && y == 0) return;

        // Calculate the rotation angle based on input direction
        //float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        // Apply the rotation to the player
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    public Vector2 GetMovementDirection()
    {
        return movement;
    }
}
