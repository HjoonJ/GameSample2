using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public string desName;

    public float waitTime;

    public Character character;
    public bool isUsing;
    public virtual void Arrived(Character c)
    {
        character = c;
        isUsing = true;

        Debug.Log($"{desName} 도착");
        //목적지 도착 시 
        //지정된 시간 동안 대기 후 다시 랜덤하게 이동하게 처리.

        StartCoroutine(WaitAndMove(waitTime));


    }

    IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        character.NextMove();
        character = null;
        isUsing = false;
        
    }


}


