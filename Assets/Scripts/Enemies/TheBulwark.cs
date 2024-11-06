using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBulwark : MonoBehaviour
{
    public float attackTimer;
    public float attackCountdown;
    public int bossHealth;
    public int bossCurrentHealth;
    public int bossPhase;
    public float phaseTransitionTimer;
    public float phaseTransitionCounter;
    public int phase1Threshold;
    private bool phase1Completed;
    public int phase2Threshold;
    private bool phase2Completed;
    public int phase3Threshold;
    private bool phase3Completed;
    public bool isActive;
    public bool isPhaseTransitioning;
    public bool isImmune;
    public BoxCollider rightArmCollider;
    public BulwarkArm rightArm;
    public BoxCollider leftArmCollider;
    public BulwarkArm leftArm;
    public List<GameObject> bridges = new List<GameObject>();
    public List<GameObject> roofBorders = new List<GameObject>();
    private int bridgeIndex;
    Animator animator;

    public Transform phase1Position;
    public Transform phase2Position;
    public List<Transform> phase2SneakAttacks = new List<Transform>();
    public Transform phase3Position;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bossCurrentHealth = bossHealth;
        bossPhase = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            attackCountdown += Time.deltaTime;
            if(attackCountdown >= attackTimer){
                switch(bossPhase){
                    case 0:
                        int randArm = Random.Range(0, 2);
                        switch(randArm){
                            case 0:
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackRight");
                            break;
                            case 1:
                            leftArmCollider.isTrigger = true;
                            leftArm.isActive = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackLeft");
                            break;
                            default:
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackRight");
                            break;
                        }
                        break;
                    case 1:
                        int randSneak = Random.Range(0, phase2SneakAttacks.Count);
                        switch(randSneak){
                            case 0:
                            transform.position = phase2SneakAttacks[0].position;
                            transform.rotation = phase2SneakAttacks[0].rotation;
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("SneakAttack");
                            break;
                            case 1:
                            transform.position = phase2SneakAttacks[1].position;
                            transform.rotation = phase2SneakAttacks[1].rotation;
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("SneakAttack");
                            break;
                            case 2:
                            transform.position = phase2SneakAttacks[2].position;
                            transform.rotation = phase2SneakAttacks[2].rotation;
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("SneakAttack");
                            break;
                            case 3:
                            transform.position = phase2SneakAttacks[3].position;
                            transform.rotation = phase2SneakAttacks[3].rotation;
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("SneakAttack");
                            break;
                            default:
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackRight");
                            break;
                        }
                    break;
                    default:
                        int randArmDef = Random.Range(0, 2);
                        switch(randArmDef){
                            case 0:
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackRight");
                            break;
                            case 1:
                            leftArmCollider.isTrigger = true;
                            leftArm.isActive = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackLeft");
                            break;
                            default:
                            rightArm.isActive = true;
                            rightArmCollider.isTrigger = true;
                            attackCountdown = 0;
                            animator.SetTrigger("AttackRight");
                            break;
                        }
                    break;
                }
            }
        }else if(isPhaseTransitioning){
            phaseTransitionCounter += Time.deltaTime;
            if(phaseTransitionCounter >= phaseTransitionTimer){
                phaseTransitionCounter = 0f;
                isImmune = false;
                isPhaseTransitioning = false;
                animator.SetTrigger("EndVortex");
            }
        }
    }

    public void SpawnBridge(){
        bridges[bridgeIndex].SetActive(true);
        roofBorders[bridgeIndex].SetActive(false);
    }

    public void ClearBridge(){
        bridges[bridgeIndex].SetActive(false);
        bridgeIndex++;
        roofBorders[bridgeIndex].SetActive(true);
    }

    public void ActivateBoss(){
        isActive = true;
    }

    public void DefeatBoss(){
        isActive = false;
        DisableArms();
        animator.SetTrigger("Death");
    }

    IEnumerator Retreat(){
        yield return new WaitForSeconds(7.5f);
        animator.SetTrigger("Retreat");
        yield return new WaitForSeconds(7.5f);
                bossPhase += 1;
                isActive = true;
    }

    public void HurtBoss(){
        if(!isImmune){
        bossCurrentHealth--;
        if(bossCurrentHealth <= 0){
            DefeatBoss();
        }else if(bossCurrentHealth <= phase1Threshold && !phase1Completed){
            phase1Completed = true;
            PhaseTransition();
        }
        else if(bossCurrentHealth <= phase2Threshold && !phase2Completed){
            PhaseTransition();
        }
        else{
        animator.SetTrigger("Hurt");
        DisableArms();
        }
        }
    }

    public void PhaseTransition(){
            isPhaseTransitioning = true;
            isActive = false;
            animator.SetTrigger("PhaseTransition");
            isImmune = true;
    }

    public void DisableArms(){
        rightArm.isActive = false;
        leftArm.isActive = false;
        leftArmCollider.isTrigger = false;
        rightArmCollider.isTrigger = false;
    }

    public void GameWin(){
        GameManager.Instance.WinLevel();
    }
}
