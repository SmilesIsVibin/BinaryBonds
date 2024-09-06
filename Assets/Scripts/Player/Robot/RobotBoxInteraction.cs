using UnityEngine;
using Cinemachine;

public class RobotBoxInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public LayerMask interactableLayer;
    public GameObject interactionPopup;

    public bool isInteracting = false;
    private BoxPullPush currentBox;
    private RobotController robotController;

    void Start()
    {
        robotController = GetComponent<RobotController>();
        interactionPopup.SetActive(false); // Ensure the pop-up is initially hidden
    }

    void Update()
    {
        RaycastHit hit;
        bool isLookingAtBox = Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, interactableLayer);

        if (isLookingAtBox && hit.collider.CompareTag("PushableBox"))
        {
            if (!isInteracting)
            {
                currentBox = hit.collider.GetComponent<BoxPullPush>();
                interactionPopup.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isInteracting)
                {
                    StartInteraction(currentBox);
                }
                else
                {
                    StopInteraction();
                }
            }
        }
        else
        {
            interactionPopup.SetActive(false);
            if (isInteracting && Input.GetKeyDown(KeyCode.E))
            {
                StopInteraction();
            }else if (isInteracting && isLookingAtBox == false){
                StopInteraction();
            }
        }
    }

    private void StartInteraction(BoxPullPush box)
    {
        isInteracting = true;
        robotController.isInteracting = true;
        box.CreateFixedJoint();
        box.AttachToPlayer();

        robotController.ChangeCameraPriority(1);
        interactionPopup.SetActive(false);
    }

    private void StopInteraction()
    {
        isInteracting = false;
        robotController.isInteracting = false;
        if (currentBox != null)
        {
            currentBox.DetachFromPlayer();
            currentBox.RemoveFixedJoint();
        }
        robotController.ChangeCameraPriority(0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.forward * interactionDistance;
        Gizmos.DrawRay(transform.position, direction);
    }
}
