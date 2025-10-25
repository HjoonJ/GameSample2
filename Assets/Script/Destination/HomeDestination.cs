using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeDestination : Destination
{
    //모든 애들이 자유롭게 들어올 수 있고
    //지정된 시간이 지나면 나감 + 나갈 수 있는 곳이 있는지 판별
    public List<Character> characterList = new List<Character>();

    //무조건 갈 수 있는 지역
    public override bool CanTake()
    {
       return true;
    }


    // 누가 들어왔는지 알 수 있는 데이터
    public override void Arrived(Character c)
    {
        characterList.Add(c);

        StartCoroutine(CoWaiting(c));

        Debug.Log($"{desName} 도착");


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
                //반복 = > 기다림
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
