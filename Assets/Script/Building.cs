using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IEnemyTarget
{
    public Transform Transform => transform;

    public float maxHp;
    public float curHp;
    public float areaRange;
    

    public void Awake()
    {
        //curHp = maxHp;
        // 하위 게임 오브젝트 중 AreaRange 이름을 가진 게임 오브젝트의 Transform 컴포넌트 반환
        areaRange = transform.Find("AreaRange").GetComponent<CapsuleCollider>().radius;
    }


    public void TakeDamage(float damage)
    {
        curHp -= damage;
        if (curHp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void AddHeal(float h)
    {
        curHp += h;
        if (curHp > maxHp)
        {
            curHp = maxHp;
        }

        

    }
}
