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
    public bool soloMode;
    public SoloPlayerController playerController;
    public GirlController girlPlayer;
    public RobotController robotPlayer;
    public RobotFollow robotFollow;
    public GameObject girlIconActive;
    public GameObject girlIconDeactive;
    public GameObject robotIconActive;
    public GameObject robotIconDeactive;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        if(!soloMode){
            SwitchPlayerCameras(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (!soloMode && Input.GetKeyDown(KeyCode.R))
            {
                if (girlPlayer.isActive)
                {
                    SwitchPlayerCameras(1);
                }else if (robotPlayer.isActive)
                {
                    SwitchPlayerCameras(0);
                }
            }
        }
    }

    public void SwitchPlayerCameras(int switcher)
    {
        switch (switcher)
        {
            case 0:
                girlPlayer.isActive = true;
                girlPlayer.currentCam.Priority = 10;
                girlIconActive.SetActive(true);
                girlIconDeactive.SetActive(false);
                robotPlayer.isActive = false;
                robotPlayer.currentCam.Priority = 0;
                robotIconActive.SetActive(false);
                robotIconDeactive.SetActive(true);
                break;
            case 1:
                girlPlayer.isActive = false;
                girlPlayer.currentCam.Priority = 0;
                girlIconActive.SetActive(false);
                girlIconDeactive.SetActive(true);
                robotPlayer.isActive = true;
                robotPlayer.currentCam.Priority = 10;
                if (robotFollow.isFollowing)
                {
                    robotFollow.isFollowing = false;
                    robotPlayer.GetComponent<NavMeshAgent>().enabled = false;
                    robotPlayer.isFollowing = false;
                }
                robotIconActive.SetActive(true);
                robotIconDeactive.SetActive(false);
                break;
            default:
                girlPlayer.isActive = true;
                girlPlayer.currentCam.Priority = 10;
                girlIconActive.SetActive(true);
                girlIconDeactive.SetActive(false);
                robotPlayer.isActive = false;
                robotPlayer.currentCam.Priority = 0;
                robotIconActive.SetActive(false);
                robotIconDeactive.SetActive(true);
                break;
        }
    }

    public void PlayerGameOver()
    {
        isActive = false;
        if(soloMode){
            playerController.isActive = false;
            playerController.gameObject.SetActive(false);
        }else{
            girlPlayer.isActive = false;
            robotPlayer.isActive = false;
            girlPlayer.gameObject.SetActive(false);
            robotPlayer.gameObject.SetActive(false);
        }
        GameManager.Instance.GameOverLevel();
    }
}
