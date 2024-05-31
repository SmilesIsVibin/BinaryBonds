using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public bool disabled = false;

    public Transform playerCamera;
    public CinemachineFreeLook thirdPersonCam;
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float gravity;
    public float defaultHeight;
    public float crouchHeight;
    public float crouchSpeed;

    float turnSmoothVelocity;
    float turnSmoothTime = 0.1f;
    private CharacterController characterController;

    private bool canMove = true;

    void OnEnable()
    {
        CameraManager.Register(thirdPersonCam);
    }
    private void OnDisable()
    {
        CameraManager.UnRegister(thirdPersonCam);
    }

    void Start()
    {
        CameraManager.SwitchCamera(thirdPersonCam);
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!disabled)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float movementDirectionY = direction.y;

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                direction.y = jumpPower;
            }
            else
            {
                direction.y = movementDirectionY;
            }

            if (!characterController.isGrounded)
            {
                direction.y -= gravity * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.LeftControl) && canMove)
            {
                characterController.height = crouchHeight;
                walkSpeed = crouchSpeed;
                runSpeed = crouchSpeed;

            }
            else
            {
                characterController.height = defaultHeight;
                walkSpeed = 6f;
                runSpeed = 12f;
            }

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0);

                Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                characterController.Move(moveDirection.normalized * walkSpeed * Time.deltaTime);
            }
        }
    }
}

