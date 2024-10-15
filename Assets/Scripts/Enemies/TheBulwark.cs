using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBulwark : MonoBehaviour
{
    public float attackTimer;
    public float attackCountdown;
    public int bossHealth;
    public int bossCurrentHealth;
    public bool isActive;
    public BoxCollider rightArmCollider;
    
    public BulwarkArm rightArm;
    public BoxCollider leftArmCollider;
    public BulwarkArm leftArm;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bossCurrentHealth = bossHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive){
            attackCountdown += Time.deltaTime;
            if(attackCountdown >= attackTimer){
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
            }
        }
    }

    public void ActivateBoss(){
        isActive = true;
    }

    public void DefeatBoss(){
        isActive = false;
        DisableArms();
        animator.SetTrigger("Death");
    }

    public void HurtBoss(){
        bossCurrentHealth--;
        if(bossCurrentHealth <= 0){
            DefeatBoss();
        }else{
        animator.SetTrigger("Hurt");
        DisableArms();
        }
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
