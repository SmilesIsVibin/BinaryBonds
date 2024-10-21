using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLevelSaveData : MonoBehaviour
{
    public List<int> levelSaves = new List<int>();

    void Start(){
        if(SceneManager.GetActiveScene().buildIndex == 0){
            SetupLevelSaveData();
        }
    }
    public void SetupLevelSaveData(){
        if(PlayerPrefs.HasKey("Level1SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level1SaveData"));
        }else{
            PlayerPrefs.SetInt("Level1SaveData", 1);
            levelSaves.Add(PlayerPrefs.GetInt("Level1SaveData"));
        }

        if(PlayerPrefs.HasKey("Level2SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level2SaveData"));
        }else{
            PlayerPrefs.SetInt("Level2SaveData", 0);
            levelSaves.Add(PlayerPrefs.GetInt("Level2SaveData"));
        }

        if(PlayerPrefs.HasKey("Level3SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level3SaveData"));
        }else{
            PlayerPrefs.SetInt("Level3SaveData", 0);
            levelSaves.Add(PlayerPrefs.GetInt("Level3SaveData"));
        }

        if(PlayerPrefs.HasKey("Level4SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level4SaveData"));
        }else{
            PlayerPrefs.SetInt("Level4SaveData", 0);
            levelSaves.Add(PlayerPrefs.GetInt("Level4SaveData"));
        }

        if(PlayerPrefs.HasKey("Level5SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level5SaveData"));
        }else{
            PlayerPrefs.SetInt("Level5SaveData", 0);
            levelSaves.Add(PlayerPrefs.GetInt("Level5SaveData"));
        }

        if(PlayerPrefs.HasKey("Level6SaveData")){
            levelSaves.Add(PlayerPrefs.GetInt("Level6SaveData"));
        }else{
            PlayerPrefs.SetInt("Level6SaveData", 0);
            levelSaves.Add(PlayerPrefs.GetInt("Level6SaveData"));
        }
    }

    public void SaveLevelCompletion(int levelIndex){
        switch(levelIndex){
            case 1:
                PlayerPrefs.SetInt("Level1SaveData", 1);
                break;
            case 2:
                PlayerPrefs.SetInt("Level2SaveData", 1);
                break;
            case 3:
                PlayerPrefs.SetInt("Level3SaveData", 1);
                break;
            case 4:
                PlayerPrefs.SetInt("Level4SaveData", 1);
                break;
            case 5:
                PlayerPrefs.SetInt("Level5SaveData", 1);
                break;
            case 6:
                PlayerPrefs.SetInt("Level6SaveData", 1);
                break;
            default:
                PlayerPrefs.SetInt("Level1SaveData", 1);
                break;
        }
    }

    public void UnlockAllLevels(){
        levelSaves.Clear();
        PlayerPrefs.SetInt("Level1SaveData", 1);
        PlayerPrefs.SetInt("Level2SaveData", 1);
        PlayerPrefs.SetInt("Level3SaveData", 1);
        PlayerPrefs.SetInt("Level4SaveData", 1);
        PlayerPrefs.SetInt("Level5SaveData", 1);
        PlayerPrefs.SetInt("Level6SaveData", 1);
        for(int i = 0; i < 6; i++){
            levelSaves.Add(1);
        }
    }
    public void ClearAllLevels(){
        levelSaves.Clear();
        PlayerPrefs.SetInt("Level1SaveData", 1);
        PlayerPrefs.SetInt("Level2SaveData", 0);
        PlayerPrefs.SetInt("Level3SaveData", 0);
        PlayerPrefs.SetInt("Level4SaveData", 0);
        PlayerPrefs.SetInt("Level5SaveData", 0);
        PlayerPrefs.SetInt("Level6SaveData", 0);
        for(int i = 0; i < 6; i++){
            levelSaves.Add(1);
        }
    }
}
