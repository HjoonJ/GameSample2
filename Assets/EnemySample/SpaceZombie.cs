using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpaceZombie : MonoBehaviour
{
    public ZombieStateType stateType;

    [SerializeField] public NavMeshAgent agent;

    //애니메이션 실행시간
    public float animTime;

    public GameObject startPoint;
    public GameObject endPoint;

    void Start()
    {
        

        
        stateType = ZombieStateType.Idle;

        agent.isStopped = true;


        StartCoroutine(CoWaiting());

        

    }

    
    void Update()
    {
        // 확률적으로 Idle(2초), Walk(5초), LookAround(2초) 가 랜덤하게 등장하게끔   
        int randomNum = Random.Range(0, 3);
        stateType = (ZombieStateType)randomNum;
        //애니메이션 실행
        
    }

    public IEnumerator CoWaiting()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            stateType = ZombieStateType.Walk;

            agent.isStopped = false;

            agent.SetDestination(startPoint.transform.position);

            yield return new WaitForSeconds(5);

            stateType = ZombieStateType.LookAround;

            agent.isStopped = true;

            stateType = ZombieStateType.Idle;

            yield return new WaitForSeconds(1);

            stateType = ZombieStateType.Walk;

            agent.isStopped = false;

            agent.SetDestination(endPoint.transform.position);

        }
        
       

    }


}

public enum ZombieStateType
{
    Idle,
    Walk,
    LookAround,
    Roar,
    Approach,
    Attack


}
