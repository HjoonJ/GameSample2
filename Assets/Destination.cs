using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public string desName;

    public virtual void Arrived()
    {
        Debug.Log($"{desName} 도착");
        //목적지 도착 시 
        //지정된 시간 동안 대기 후 다시 랜덤하게 이동하게 처리.

    }



}


