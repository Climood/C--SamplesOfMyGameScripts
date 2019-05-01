using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBordersManager : MonoBehaviour
{
    public float upBorderZ = 14.9f;
    public float downBorderZ = -30f;
    public float rightBorderX = 347f;
    public float leftBorderX = -374f;
    public GameObject upBorder;
    public GameObject downBorder;
    public GameObject rightBorder;
    public GameObject leftBorder;
    // Start is called before the first frame update
    void Start() //Расставляет границы мира по нужным позициям, чтобы не руками
    {
        Vector3 tmpPosition = new Vector3();
        tmpPosition.x = upBorder.transform.position.x;
        tmpPosition.y = upBorder.transform.position.y;
        tmpPosition.z = upBorderZ;
        upBorder.transform.position = tmpPosition;
        tmpPosition.x = downBorder.transform.position.x;
        tmpPosition.y = downBorder.transform.position.y;
        tmpPosition.z = downBorderZ;
        downBorder.transform.position = tmpPosition;
        tmpPosition.x = rightBorderX;
        tmpPosition.y = rightBorder.transform.position.y;
        tmpPosition.z = rightBorder.transform.position.z;
        rightBorder.transform.position = tmpPosition;
        tmpPosition.x = leftBorderX;
        tmpPosition.y = leftBorder.transform.position.y;
        tmpPosition.z = leftBorder.transform.position.z;
        leftBorder.transform.position = tmpPosition;
    }
    
}
