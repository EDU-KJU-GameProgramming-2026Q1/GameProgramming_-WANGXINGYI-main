using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EX_CharacterController_InputManager_Parts_Move : MonoBehaviour
{
    CharacterController character;

    [Header("Move")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float friction = 0.9f;

    [Header("Jump / Gravity")]
    public float jumpHeight = 0.5f;
    public float gravity = -9.81f;

    [Header("Climb")]
    public float climbSpeed = 1.2f;

    Vector3 velocity;

    Vector2 moveInput;

    bool jumpPressed;
    bool isSprinting;

    enum PlayerState
    {
        Ground,
        Air,
        Climb
    }

    PlayerState state = PlayerState.Ground;

    enum ClimbType
    {
        None,
        Ladder,
        Cliff
    }

    ClimbType climbType = ClimbType.None;

    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    void Update()
    {
        ReadInput();

        StateMachine();

        character.Move(velocity * Time.deltaTime);

        jumpPressed = false;
    }

    void ReadInput()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;
    }

    void StateMachine()
    {
        switch (state)
        {
            case PlayerState.Ground:
                GroundMove();
                break;

            case PlayerState.Air:
                AirMove();
                break;

            case PlayerState.Climb:
                ClimbMove();
                break;
        }
    }

    void GroundMove()
    {
        Vector3 inputDir =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        if (inputDir.sqrMagnitude > 1)
            inputDir.Normalize();

        // Sprint Àû¿ë
        float speed = isSprinting ? runSpeed : walkSpeed;

        if (inputDir.magnitude > 0)
        {
            velocity.x = inputDir.x * speed;
            velocity.z = inputDir.z * speed;
        }
        else
        {
            velocity.x *= friction;
            velocity.z *= friction;
        }

        velocity.y = -2f;

        if (jumpPressed)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            state = PlayerState.Air;
            return;
        }

        if (!character.isGrounded)
        {
            state = PlayerState.Air;
        }
    }

    void AirMove()
    {
        Vector3 inputDir =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        float speed = isSprinting ? runSpeed : walkSpeed;

        velocity.x = inputDir.x * speed;
        velocity.z = inputDir.z * speed;

        velocity.y += gravity * Time.deltaTime;

        if (character.isGrounded)
        {
            state = PlayerState.Ground;
        }
    }

    void ClimbMove()
    {
        velocity = Vector3.zero;

        if (climbType == ClimbType.Ladder)
        {
            velocity.y = moveInput.y * climbSpeed;
        }
        else if (climbType == ClimbType.Cliff)
        {
            velocity =
                transform.right * moveInput.x * climbSpeed +
                Vector3.up * moveInput.y * climbSpeed;

            velocity += -transform.forward * 0.1f;
        }

        if (jumpPressed)
        {
            Vector3 jumpDir = -transform.forward + Vector3.up;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);

            velocity = jumpDir.normalized * jumpForce;

            state = PlayerState.Air;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClimbableLadder"))
        {
            climbType = ClimbType.Ladder;
            state = PlayerState.Climb;
            velocity = Vector3.zero;
        }

        if (other.CompareTag("ClimbableCliff"))
        {
            climbType = ClimbType.Cliff;
            state = PlayerState.Climb;
            velocity = Vector3.zero;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ClimbableLadder") ||
            other.CompareTag("ClimbableCliff"))
        {
            state = PlayerState.Air;
        }
    }
}