using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    public PlayableDirector gameCutscene;

    public void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") || other.CompareTag("Player__Elara") || other.CompareTag("Player_Happy")){
            gameCutscene.Play();
            Destroy(this.gameObject);
        }
    }
}
