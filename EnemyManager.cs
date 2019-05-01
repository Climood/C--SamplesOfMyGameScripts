using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject Enemy1;
    public float randXLeftBorder = -390f;
    public float randXRightBorder= 330f;
    float randPositionX;
    Vector3 whereToSpawn;
    public float timeBeforeStartSpawn;
    public float spawnDelayTimer;
    public float nextSpawn;
    
    public void onClickSpawnButton()
    {
        randPositionX = Random.Range(randXLeftBorder, randXRightBorder); //Указываем [x1,x2] между которыми будут спавниться, x буду подставлять в спавн вектор, который проходит по верху дороги 
        whereToSpawn = new Vector3(randPositionX, transform.position.y, transform.position.z); //т.е. y z  будут браться от объекта, к которому привязан скрипт
        Instantiate(Enemy1, whereToSpawn, Quaternion.identity); // сам спавн процесс
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        while(timeBeforeStartSpawn > 0)
        {
            timeBeforeStartSpawn -= Time.deltaTime;
            return;
        }
        if(nextSpawn <= 0)
        {
            nextSpawn = spawnDelayTimer;
            randPositionX = Random.Range(-390,330f); //Указываем [x1,x2] между которыми будут спавниться, x буду подставлять в спавн вектор, который проходит по верху дороги 
            whereToSpawn = new Vector3(randPositionX, transform.position.y, transform.position.z); //т.е. y z  будут браться от объекта, к которому привязан скрипт
            Instantiate(Enemy1  , whereToSpawn, Quaternion.identity); // сам спавн процесс
        }
        else
        {
            nextSpawn -= Time.deltaTime;
        }
    }

    void setTargetAllEnemysToPlayer()
    {
    }
}
