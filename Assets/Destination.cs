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

        Debug.Log($"{desName} ����");
        //������ ���� �� 
        //������ �ð� ���� ��� �� �ٽ� �����ϰ� �̵��ϰ� ó��.

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


