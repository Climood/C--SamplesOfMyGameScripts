using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackA : MonoBehaviour
{
    private float curTimeBetweenAttack;
    public float startTimeBetweenAttack; // в секундах по ходу
    private Animator animatorController;
    public Transform attackZonePosition;
    public LayerMask layerMaskOfPlayer; //Указываем слой layer mask врагов
    public float attackRangeX;//размеры хитбокса удара 
    public float attackRangeY;
    public float attackRangeZ;
    public float damageOfAttack;
    private bool canAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        curTimeBetweenAttack = startTimeBetweenAttack;
        animatorController = GetComponent<Animator>();
    }
  
    public void canIattack(bool can)
    {
        canAttack = can;
    }
    void FixedUpdate()
    {
        if (curTimeBetweenAttack <= 0f)//атакуем
        {
            if (canAttack)
            {
                animatorController.SetTrigger("enemyAttack");
                Collider[] playerToDamage = Physics.OverlapBox(attackZonePosition.position, new Vector3(attackRangeX, attackRangeY, attackRangeZ), Quaternion.EulerRotation(0, 0, 0), layerMaskOfPlayer); //возвращает список коллайдеров, которые зашли в сферу (зону) атаки
                for (int i = 0; i < playerToDamage.Length; i++)
                {
                    playerToDamage[i].GetComponent<PlayerController3D>().playerTakeDamage(damageOfAttack); //наносим сам урон каждому врагу, чей коллайдер был в зоне поражения
                }
            }
            curTimeBetweenAttack = startTimeBetweenAttack;
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
