using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    public string desName;

    public virtual void Arrived()
    {
        Debug.Log($"{desName} ����");
        //������ ���� �� 
        //������ �ð� ���� ��� �� �ٽ� �����ϰ� �̵��ϰ� ó��.

    }



}


