using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class RobotController : MonoBehaviour
{
    public CharacterController characterController;
    public float maxSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public bool isSprinting;
    public float jumpSpeed;
    public float ySpeed;
    public float originalStepOffset;
    public float coyoteTime;
    private float? lastGroundTime;
    private float? jumpPressedTime;
    public Transform cameraTransform;
    public float rotationSpeed;
    public Animator animator;
    public bool isActive;
    public bool isGrounded;
    public bool isJumping;
    public bool isFollowing;
    bool cursorLocked;
    public CinemachineFreeLook[] cameraList;
    public CinemachineFreeLook currentCam;

    // Interaction variables
    public bool isInteractingWithBox = false;
    private Transform interactingBox;
    private BoxInteraction currentBoxInteraction;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        Cursor.lockState = CursorLockMode.Locked;
        cursorLocked = true;
        maxSpeed = walkSpeed;
        ChangeCameraPriority(0);
    }

    void Update()
    {
        if (isActive)
        {
            if (isInteractingWithBox)
            {
                HandleBoxInteractionMovement();
            }
            else
            {
                HandleRegularMovement();
            }
        }

        HandleCursorLock();
    }

    private void HandleRegularMovement()
    {
        if (!isFollowing)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                maxSpeed = sprintSpeed;
            }
            else
            {
                maxSpeed = walkSpeed;
            }

            Vector3 movementDirection = new Vector3(horizontal, 0f, vertical);
            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
            float speed = inputMagnitude * maxSpeed;

            animator.SetFloat("InputMagnitude", speed, 0.05f, Time.deltaTime);
            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
            movementDirection.Normalize();

            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (characterController.isGrounded)
            {
                lastGroundTime = Time.time;
            }

            if (Input.GetButtonDown("Jump"))
            {
                jumpPressedTime = Time.time;
            }

            if (Time.time - lastGroundTime <= coyoteTime)
            {
                characterController.stepOffset = originalStepOffset;
                ySpeed = -0.5f;
                animator.SetBool("isGrounded", true);
                isGrounded = true;
                animator.SetBool("isJumping", false);
                isJumping = false;
                animator.SetBool("isFalling", false);

                if (Time.time - jumpPressedTime <= coyoteTime)
                {
                    animator.SetBool("isJumping", true);
                    isJumping = true;
                    ySpeed = jumpSpeed;
                    jumpPressedTime = null;
                    lastGroundTime = null;
                }
            }
            else
            {
                characterController.stepOffset = 0f;
                animator.SetBool("isGrounded", false);
                isGrounded = false;

                if ((isJumping && ySpeed < 0) || ySpeed < -2.5f)
                {
                    animator.SetBool("isFalling", true);
                }
            }

            Vector3 velocity = movementDirection * speed;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);

            if (movementDirection != Vector3.zero)
            {
                Quaternion toRotate = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (isFollowing)
            {
                ySpeed = -0.5f;
            }
            else if (!isFollowing)
            {
                ySpeed += Physics.gravity.y * Time.deltaTime;

                if (characterController.isGrounded)
                {
                    lastGroundTime = Time.time;
                }

                if (Time.time - lastGroundTime <= coyoteTime)
                {
                    characterController.stepOffset = originalStepOffset;
                    ySpeed = -0.5f;
                    animator.SetBool("isGrounded", true);
                    isGrounded = true;
                    animator.SetBool("isFalling", false);
                }
                else
                {
                    characterController.stepOffset = 0f;
                    animator.SetBool("isGrounded", false);
                    isGrounded = false;
                    if ((ySpeed < 0) || ySpeed < -2.5f)
                    {
                        animator.SetBool("isFalling", true);
                    }
                }

                Vector3 velocity = new Vector3(0f, ySpeed, 0f);
                characterController.Move(velocity * Time.deltaTime);
            }
            else
            {
                ySpeed += Physics.gravity.y * Time.deltaTime;

                if (characterController.isGrounded)
                {
                    lastGroundTime = Time.time;
                }

                if (Time.time - lastGroundTime <= coyoteTime)
                {
                    characterController.stepOffset = originalStepOffset;
                    ySpeed = -0.5f;
                    animator.SetBool("isGrounded", true);
                    isGrounded = true;
                    animator.SetBool("isJumping", false);
                    isJumping = false;
                    animator.SetBool("isFalling", false);
                }
                else
                {
                    characterController.stepOffset = 0f;
                    animator.SetBool("isGrounded", false);
                    isGrounded = false;

                    if ((ySpeed < 0) || ySpeed < -2.5f)
                    {
                        animator.SetBool("isFalling", true);
                    }

                    Vector3 velocity = new Vector3(0f, ySpeed, 0f);
                    characterController.Move(velocity * Time.deltaTime);
                }
            }
        }
    }

    private void HandleBoxInteractionMovement()
    {
        // Restrict movement to forward, backward, left, and right
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0f, vertical);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        float speed = inputMagnitude * walkSpeed;

        // Movement direction is relative to the box's forward direction
        movementDirection = interactingBox.TransformDirection(movementDirection);
        movementDirection.y = 0f;
        movementDirection.Normalize();

        // Move the player and the box together
        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        currentBoxInteraction.MoveWithPlayer(transform.position);

        // Prevent rotation and jumping while interacting with the box
        animator.SetFloat("InputMagnitude", speed, 0.05f, Time.deltaTime);
        animator.SetBool("isJumping", false);
        animator.SetBool("isGrounded", characterController.isGrounded);
        ySpeed = characterController.isGrounded ? -0.5f : ySpeed + Physics.gravity.y * Time.deltaTime;
    }

    private void HandleCursorLock()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursorLocked = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    public void ChangeCameraPriority(int camIndex)
    {
        for (int i = 0; i < cameraList.Length; i++)
        {
            if (i != camIndex)
            {
                cameraList[i].Priority = 0;
            }
            else
            {
                cameraList[i].Priority = 10;
                currentCam = cameraList[i];
            }
        }
    }

    public void BeginInteraction(Transform box, BoxInteraction boxInteraction)
    {
        isInteractingWithBox = true;
        interactingBox = box;
        currentBoxInteraction = boxInteraction;

        // Lock the camera rotation
        currentCam.m_XAxis.m_MaxSpeed = 0f;
        currentCam.m_YAxis.m_MaxSpeed = 0f;

        // Snap the player to the box side
        PositionPlayerAtBox();
    }

    public void EndInteraction()
    {
        isInteractingWithBox = false;
        interactingBox = null;
        currentBoxInteraction = null;

        // Unlock the camera rotation
        currentCam.m_XAxis.m_MaxSpeed = 300f; // Restore the original speed
        currentCam.m_YAxis.m_MaxSpeed = 2f;   // Restore the original speed
    }

    private void PositionPlayerAtBox()
    {
        // Snap the player to the center of the closest side of the box
        Vector3 boxCenter = interactingBox.position;
        Vector3 playerPosition = transform.position;
        Vector3 offset = playerPosition - boxCenter;

        float absX = Mathf.Abs(offset.x);
        float absZ = Mathf.Abs(offset.z);

        if (absX > absZ)
        {
            // Snap to the left or right side
            playerPosition = new Vector3(boxCenter.x + Mathf.Sign(offset.x) * 1.5f, playerPosition.y, boxCenter.z);
        }
        else
        {
            // Snap to the front or back side
            playerPosition = new Vector3(boxCenter.x, playerPosition.y, boxCenter.z + Mathf.Sign(offset.z) * 1.5f);
        }

        characterController.enabled = false;
        transform.position = playerPosition;
        transform.rotation = Quaternion.LookRotation(-offset.normalized);
        characterController.enabled = true;
    }
}
