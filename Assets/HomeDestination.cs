using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDestination : Destination
{
    //��� �ֵ��� �����Ӱ� ���� �� �ְ�
    //������ �ð��� ������ ���� + ���� �� �ִ� ���� �ִ��� �Ǻ�
    public List<Character> characterList = new List<Character>();

    //������ �� �� �ִ� ����
    public override bool CanTake()
    {
       return true;
    }


    // ���� ���Դ��� �� �� �ִ� ������
    public override void Arrived(Character c)
    {
        characterList.Add(c);

        StartCoroutine(CoWaiting(c));

        Debug.Log($"{desName} ����");


        //StartCoroutine(CoAction(waitTime));


    }


    public IEnumerator CoWaiting(Character c)
    {
        while (true) 
        
        {
            yield return new WaitForSeconds(3);
            Destination des = DestinationManager.Instance.GetEmptyDestination();

            if (des == null && des == this)
            {
                //�ݺ� = > ��ٸ�
                continue;
            }

            break;


        }
        

        Leave(c);


    }

    public override void Leave(Character c)
    {

        c.NextMove();
        characterList.Remove(c);
    }

}
