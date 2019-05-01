using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameMove : MonoBehaviour
{
    public GameObject player; //ссылка на игрока, к которому цыпляем
    // Start is called before the first frame update
    //public GameObject Camera; //Ссылка на саму камеру (нужна, чтобы не изменять y z вручную, а брать настроенные из юнити) ПОМЕНЯЛ НА THIS
        
    void FixedUpdate()
    {
        //transform.position = new Vector3 (player.transform.position.x, Camera.transform.position.y, Camera.transform.position.z);    //решил менять только по оси Х
        transform.position = new Vector3 (player.transform.position.x, this.transform.position.y, this.transform.position.z);  //this заменяет ссылку на саму камеру, т.к. скрипт создается в объекте камеры  //решил менять только по оси Х чтобы камера шла вправо\влево
    }
}
