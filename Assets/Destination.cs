using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour, IChangedGameMode
{
    public string desName;

    public float waitTime;

    public Character character;
    public bool isUsing;

    public void Awake()
    {
        GameManager.changedGameModeListener.Add(this);
    }


    public virtual bool CanTake()
    {
        if (isUsing == true)
            return false;
        else
            return true;
 
    }

    public virtual void Taken(Character c)
    {
        // character 멤버변수를 채우기 위해서 만듦.
        character = c;
        isUsing = true;
    }

    public virtual void Arrived(Character c)
    {

        character = c;
        isUsing = true;


        Debug.Log($"Destination Arrived() {c.gameObject.name} {desName} 도착");
        //목적지 도착 시 
        //지정된 시간 동안 대기 후 다시 랜덤하게 이동하게 처리.

        StartCoroutine(CoAction(waitTime));


    }

    public virtual IEnumerator CoAction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);


        Leave(character);
    }


    //캐릭터가 해당 장소를 떠날 때.
    public virtual void Leave(Character c)
    {
        c.NextMove();
        character = null;
        isUsing = false;
        Debug.Log($"Destination Leave() {c.gameObject.name}");
    }

    public virtual void ChangedGameMode(GameMode gm)
    {

    }
}


