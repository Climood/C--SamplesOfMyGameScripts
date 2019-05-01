using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private float curTimeBetweenAttack;
    public float startTimeBetweenAttack;
    // Start is called before the first frame update
    void Start()
    {
        curTimeBetweenAttack = startTimeBetweenAttack;
    }
    
    void FixedUpdate()
    {
        if(curTimeBetweenAttack <= 0f)//атакуем
        {
            curTimeBetweenAttack = startTimeBetweenAttack;
        }
        else
        {
            curTimeBetweenAttack -= Time.deltaTime;
        }
    }
}
