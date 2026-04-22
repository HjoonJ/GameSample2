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
        // 1. 게임 시작 처음부터 건물 체력이 꽉 차있는 경우
        // 2. 게임 시작 이후 미론이 건물의 모든 체력을 회복시켰을 경우 
        attackCoolTime += Time.deltaTime;
        healCoolTime += Time.deltaTime;

        if (closestEnemy != null)
        {
            //적이 이미 설정된 상황
            agent.SetDestination(closestEnemy.transform.position);

            float distance = Vector3.Distance(this.transform.position, closestEnemy.transform.position);
            
            if (distance <= attackRange)
            {
                // 공격시작. 한번 공격 후 공격 쿨타임 지나고 다시 한번 공격. 
                LookAt(closestEnemy.transform);

                if (attackCoolTime >= attackSpeed)
                {
                    Debug.Log($"Attack() {closestEnemy.gameObject.name}");
                    attackCoolTime = 0;
                    agent.isStopped = true;
                    Attack();
                    attacking = true;
                    
                }

            }

            else // 대상은 있는데 거리가 먼 경우 (공격 범위 밖) 
            {

                // 대상 적(이 어택 레인지에 들어왔다가 다시 멀어진 경우
                if (attacking == true)
                {
                    Debug.Log($"다시 멀어진 경우");

                    closestEnemy = null;
                    attacking = false;
                    return;
                }

            }

        }

        if (targetBuilding == null)
        {
            if (closestEnemy != null)
            {
                if (closestEnemy.gameObject.activeSelf == false)
                {
                    closestEnemy = null;
                }
                return;
            }


            for (int i = 0; i < buildings.Count; i++)
            {
                //체력이 감소한 빌딩이 있을 경우
                if (buildings[i].curHp < buildings[i].maxHp)
                {
                    //buildings[i] 수리해야될 가장 가까운 건물(대상)찾기 
                    //접근 -> 체력 회복

                    Debug.Log($"타겟 빌딩 설정됨");
                    agent.isStopped = false;
                    targetBuilding = buildings[i];

                    return;
                }
            }

            // 여기까지 왔다는 것은 모든 건물들의 체력이 꽉차있음 - > 적을 찾으러 가야 함.
            if (closestEnemy == null || closestEnemy.gameObject.activeSelf == false)
            {
                agent.isStopped = true;

                closestEnemy = EnemyManager.Instance.FindClosestEnemy(transform.position);

                if (closestEnemy == null)
                {
                    return;
                }
                agent.isStopped = false;
            }

            //모든 건물의 체력이 꽉차있을 때! - 적을 찾기. 
            // 1. buildings 리스트에 있는 모든 건물들의 체력을 검사 
            // 2. 모든 건물들의 현재 체력이 맥스 체력과 같음
            // 3. 적 찾기.

        
        }
        
        //건물 힐하는 곳(targetBuilding != null)
        else
        {
            agent.SetDestination(targetBuilding.transform.position);

            float buildingDistance = Vector3.Distance(targetBuilding.transform.position, transform.position);

            //공격범위 안에 들어간 상황
            if (buildingDistance <= healRange + targetBuilding.areaRange)
            {
                // 공격시작. 한번 공격 후 공격 쿨타임 지나고 다시 한번 공격. 
                LookAt(targetBuilding.transform);
                agent.isStopped = true;

                if (healCoolTime >= healSpeed)
                {
                    healCoolTime = 0;

                    Debug.Log($"Heal() {targetBuilding.gameObject.name}");
                    Heal();
                    healing = true;
                }

            }

        }

    }

    public override void CharacterBattleMode()
    {
        base.CharacterBattleMode();
        des = null;
        buildings = FindObjectsByType<Building>(FindObjectsSortMode.None).ToList();

        targetBuilding = null;

    }

    public void Heal()
    {
        targetBuilding.AddHeal(healGauge);

        if (targetBuilding.curHp >= targetBuilding.maxHp)
        {
            targetBuilding = null;
        }
        
    }

    public void HealAfterBattle()
    {
        if (closestEnemy == null || closestEnemy.gameObject.activeSelf == false)
        {

            targetBuilding = null;
            
                
            agent.isStopped = true;

            closestEnemy = EnemyManager.Instance.FindClosestEnemy(transform.position);

            if (closestEnemy == null)
            {
                return;
            }
            agent.isStopped = false;
        }


        float distance = Vector3.Distance(closestEnemy.transform.position, transform.position);

        //공격범위 안에 들어간 상황
        if (distance <= attackRange)
        {
            // 공격시작. 한번 공격 후 공격 쿨타임 지나고 다시 한번 공격. 
            LookAt(closestEnemy.transform);

            if (attackCoolTime >= attackSpeed)
            {

                attackCoolTime = 0;
                agent.isStopped = true;
                Attack();
                attacking = true;
            }

        }


        else // 대상은 있는데 거리가 먼 경우 (공격 범위 밖) 
        {

            // 대상 적(이 어택 레인지에 들어왔다가 다시 멀어진 경우
            if (attacking == true)
            {
                closestEnemy = null;
                attacking = false;
                return;
            }

            // 대상 적이 있지만 아직 마주친 적도 없는 경우 (다가가고 있어야 함)
            agent.SetDestination(closestEnemy.transform.position);


        }

        attackCoolTime += Time.deltaTime;



    }


}
