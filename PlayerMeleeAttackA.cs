using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttackA : MonoBehaviour
{
    private float curTimeBetweenAttack;
    public float startTimeBetweenAttack; // в секундах по ходу
    private Animator animatorController;
    private bool buttonClicked = false;
    public Transform attackZonePosition;
    public LayerMask layerMaskOfEnemy; //Указываем слой layer mask врагов
    public float attackRangeX;//размеры хитбокса удара 
    public float attackRangeY;
    public float attackRangeZ;
    public float damageOfAttack;
    // Start is called before the first frame update
    void Start()
    {
        curTimeBetweenAttack = startTimeBetweenAttack;
        animatorController = GetComponent<Animator>();
    }

    public void onClickFireButton() 
    {
        animatorController.SetTrigger("playerAttack");
        buttonClicked = true;
    }

    void FixedUpdate()
    {
        if (curTimeBetweenAttack <= 0f)//атакуем
        {
            if (buttonClicked)
            {
                Collider[] enemyToDamage = Physics.OverlapBox(attackZonePosition.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ),Quaternion.EulerRotation(0,0,0), layerMaskOfEnemy); //возвращает список коллайдеров, которые зашли в сферу (зону) атаки
                for(int i = 0; i < enemyToDamage.Length; i++)
                {
                    enemyToDamage[i].GetComponent<EnemyController3D>().enemyTakeDamage(damageOfAttack); //наносим сам урон каждому врагу, чей коллайдер был в зоне поражения
                }
                buttonClicked = false;
                curTimeBetweenAttack = startTimeBetweenAttack;
            }
        }
        else
        {
            curTimeBetweenAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos() //чтобы визуализировать область атаки
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(attackZonePosition.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ));
    }
}
