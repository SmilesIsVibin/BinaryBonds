using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public bool isActive;

    [Header("Players")]
    public PlayerController girlPlayer;
    public RobotController robotPlayer;
    public RobotFollow robotFollow;
    public GameObject girlIconActive;
    public GameObject girlIconDeactive;
    public GameObject robotIconActive;
    public GameObject robotIconDeactive;
    public GameObject girlTracker;
    public GameObject robotTracker;

    [Header("Distance")]
    public RawImage girlPic;
    public float minDistance;
    public float maxDistance;

    [Header("Player Cameras")]
    public CinemachineFreeLook girlCam;
    public CinemachineFreeLook robotCam;

    // Start is called before the first frame update
    void Start()
    {
        SwitchPlayerCameras(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (girlPlayer.isActive)
                {
                    SwitchPlayerCameras(1);
                }else if (robotPlayer.isActive)
                {
                    SwitchPlayerCameras(0);
                }
            }

            if(Vector3.Distance(girlPlayer.transform.position, robotPlayer.transform.position) <= minDistance)
            {
                girlPic.color = Color.green;
            }else if (Vector3.Distance(girlPlayer.transform.position, robotPlayer.transform.position) > minDistance && Vector3.Distance(girlPlayer.transform.position, robotPlayer.transform.position) < maxDistance)
            {
                girlPic.color = Color.yellow;
            }
            else
            {
                girlPic.color = Color.red;
            }
        }
    }

    public void SwitchPlayerCameras(int switcher)
    {
        switch (switcher)
        {
            case 0:
                girlPlayer.isActive = true;
                girlCam.Priority = 10;
                girlIconActive.SetActive(true);
                girlIconDeactive.SetActive(false);
                robotPlayer.isActive = false;
                robotCam.Priority = 0;
                robotIconActive.SetActive(false);
                robotIconDeactive.SetActive(true);

                girlTracker.SetActive(false);
                robotTracker.SetActive(true);
                break;
            case 1:
                girlPlayer.isActive = false;
                girlCam.Priority = 0;
                girlIconActive.SetActive(false);
                girlIconDeactive.SetActive(true);
                robotPlayer.isActive = true;
                robotCam.Priority = 10;
                if (robotFollow.isFollowing)
                {
                    robotFollow.isFollowing = false;
                    robotPlayer.GetComponent<NavMeshAgent>().enabled = false;
                    robotPlayer.isFollowing = false;
                }
                robotIconActive.SetActive(true);
                robotIconDeactive.SetActive(false);

                girlTracker.SetActive(true);
                robotTracker.SetActive(false);
                break;
            default:
                girlPlayer.isActive = true;
                girlCam.Priority = 10;
                girlIconActive.SetActive(true);
                girlIconDeactive.SetActive(false);
                robotPlayer.isActive = false;
                robotCam.Priority = 0;
                robotIconActive.SetActive(false);
                robotIconDeactive.SetActive(true);

                girlTracker.SetActive(false);
                robotTracker.SetActive(true);
                break;
        }
    }

    public void PlayerDeath()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
