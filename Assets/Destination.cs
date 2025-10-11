using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public string desName;

    public float waitTime;

    public Character character;
    public bool isUsing;

    public virtual bool CanTake()
    {
        if (isUsing == true)
            return false;
        else
            return true;
 
    }

    public virtual void Taken(Character c)
    {
        // character ��������� ä��� ���ؼ� ����.
        character = c;
        isUsing = true;
    }

    public virtual void Arrived(Character c)
    {
        character = c;
        isUsing = true;

        Debug.Log($"Destination Arrived() {c.gameObject.name} {desName} ����");
        //������ ���� �� 
        //������ �ð� ���� ��� �� �ٽ� �����ϰ� �̵��ϰ� ó��.

        StartCoroutine(CoAction(waitTime));


    }

    public virtual IEnumerator CoAction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        Leave(character);
    }


    //ĳ���Ͱ� �ش� ��Ҹ� ���� ��.
    public virtual void Leave(Character c)
    {
        c.NextMove();
        character = null;
        isUsing = false;
        Debug.Log($"Destination Leave() {c.gameObject.name}");
    }


}


