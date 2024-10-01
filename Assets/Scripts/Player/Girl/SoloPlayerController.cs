using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class SoloPlayerController : MonoBehaviour
{
    [Header("Player Movement Info")]
    public CharacterController characterController;
    public float maxSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public bool isSprinting;

    [Header("Player Jump Info")]
    public float jumpSpeed;
    public float ySpeed;
    public float originalStepOffset;
    public float coyoteTime;
    public float fallTime;
    private float? lastGroundTime;
    private float? jumpPressedTime;

    [Header("Player Camera Info")]
    public Transform cameraTransform;
    public float rotationSpeed;
    public Animator animator;
    public bool isGrounded;
    public bool isJumping;
    public CinemachineFreeLook[] cameraList;
    public CinemachineFreeLook currentCam;

    [Header("Player Misc Info")]
    public bool isActive;
    
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        Cursor.lockState = CursorLockMode.Locked;
        maxSpeed = walkSpeed;
        ChangeCameraPriority(0);
    }

    void Update()
    {
        if (isActive) 
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

                if ((isJumping && ySpeed < 0) || ySpeed < fallTime)
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

                if (ySpeed < fallTime)
                {
                    animator.SetBool("isFalling", true);
                }
            }

            Vector3 velocity = new Vector3(0f, ySpeed, 0f);
            characterController.Move(velocity * Time.deltaTime);
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
}
