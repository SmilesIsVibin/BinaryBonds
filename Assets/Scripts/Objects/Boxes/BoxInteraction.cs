using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    private Rigidbody boxRigidbody;
    private RobotController playerController;
    private bool isInteracting = false;

    void Start()
    {
        boxRigidbody = GetComponent<Rigidbody>();
    }

    public void BeginInteraction(RobotController player)
    {
        playerController = player;
        isInteracting = true;
        boxRigidbody.isKinematic = true;
        playerController.BeginInteraction(transform, this);
    }

    public void EndInteraction()
    {
        isInteracting = false;
        boxRigidbody.isKinematic = false;
        playerController.EndInteraction();
        playerController = null;
    }

    public void MoveWithPlayer(Vector3 playerPosition)
    {
        // The box should always stay in front of the player, adjust its position accordingly
        Vector3 targetPosition = playerPosition + playerController.transform.forward * 1.5f;
        targetPosition.y = transform.position.y; // Ensure the box stays on the same level

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
    }

    void Update()
    {
        if (isInteracting && !IsGrounded())
        {
            EndInteraction(); // Stop interacting if the box is not grounded
        }
    }

    private bool IsGrounded()
    {
        // Simple check to see if the box is grounded (not falling)
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the box hits a wall or obstacle, stop interaction
        if (isInteracting && other.CompareTag("Obstacle"))
        {
            EndInteraction();
        }
    }
}
