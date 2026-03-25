using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Miron : Character
{
    public List<Building> buildings = new List<Building>();

    public float healRange;
    public Building targetBuilding;
    
    public float healCoolTime; // 타이머
    public float healSpeed; // 공격속도 == 1
    public bool healing;
    public float healGauge;

    public override void UpdateBattle()
    {

        //회복시킬 건물이 없을 때- 적 찾기 (처음부터 건물의 체력들이 다 차있을 때 // 건물들을 모두 회복시켰을 때)

        if (targetBuilding == null)
        {

            for (int i = 0; i < buildings.Count; i++)
            {

                if (buildings[i].curHp < buildings[i].maxHp)
                {
                    //buildings[i] 수리해야될 가장 가까운 건물(대상)찾기 
                    //접근 -> 체력 회복

                    
                    agent.isStopped = false;
                    targetBuilding = buildings[i];

                    return;
                }
                

            }
            //모든 건물의 체력이 꽉차있을 때! - 적을 찾기. 



        }
        else
        {
            agent.SetDestination(targetBuilding.transform.position);

            float buildingDistance = Vector3.Distance(targetBuilding.transform.position, transform.position);

            //공격범위 안에 들어간 상황
            if (buildingDistance <= healRange + targetBuilding.areaRange)
            {
                // 공격시작. 한번 공격 후 공격 쿨타임 지나고 다시 한번 공격. 
                LookAt();
                agent.isStopped = true;

                if (healCoolTime >= healSpeed)
                {
                    healCoolTime = 0;
                    Heal();
                    healing = true;
                }

            }

        }

        healCoolTime += Time.deltaTime;

    }

    public override void CharacterBattleMode()
    {
        base.CharacterBattleMode();
        des = null;
        buildings = FindObjectsByType<Building>(FindObjectsSortMode.None).ToList();


    }

    public override void LookAt()
    {
        
        Vector3 vec = (targetBuilding.transform.position - transform.position);
        Vector3 dir = vec.normalized;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);


    }

    public void Heal()
    {
        targetBuilding.AddHeal(healGauge);

        if (targetBuilding.curHp >= targetBuilding.maxHp)
        {
            targetBuilding = null;
        }
        
    }
}
