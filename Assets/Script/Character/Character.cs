using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Diagnostics.Contracts;

public class Character : MonoBehaviour, IChangedGameMode, IEnemyTarget
{

    [SerializeField] public NavMeshAgent agent;

    public bool arrived;

    public Destination des; // 목적지 자체

    public Transform Transform => transform; // 프로퍼티

    public float maxHp;
    public float curHp;

    //정석
    //public Transform Transform
    //{
    //    get { return transform; }
    //}

    //public float waitTime;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        GameManager.changedGameModeListener.Add(this);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //targetIdx = -1;
        //SetDestination() 함수를 사용해서 클릭한 위치로 플레이어를 보내기.
        agent.SetDestination(new Vector3(0, 0, 0));

        NextMove();



    }


    public bool knockDown;
    void Update()
    {
        if (knockDown == true)
        {
            return;
        }
        
        if (arrived == false)
        {
            if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0)
                {

                    //Debug.Log("도착");

                    //도착 시 해당 위치의 Destination 컴포넌트 Arrived 함수 호출하기
                    //+ 클릭으로 이동한 지역에서는 처리안되게 처리
                    
                    //destinations 배열에 들어가있는 특정 장소에 도착 -> destinations 컨포넌트 불러오기
                    //Arrived 함수 호출

                    arrived = true;
                    des.Arrived(this);



                }
            }
        }
        

        // 화면에 클릭하는 곳으로 캐릭터 이동
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Plane")))
            {
                Vector3 destination = hit.point;


                arrived = false;
                agent.SetDestination(destination);

                // hit.point 도착하면 로그 출력
                float dis = Vector3.Distance(transform.position, hit.point);


            }


        }
    }
    

    public void MoveTo()
    {
        arrived = false;
        agent.SetDestination(des.transform.position);

        des.Taken(this);

        

    }

    // 캐릭터가 목적지를 정해야될 때!
    public void NextMove()
    {
        //DestinationManager로부터 목적지를 전달받기.
        des = DestinationManager.Instance.GetEmptyDestination();


        MoveTo();
    }

   public void ChangedGameMode(GameMode gm)
    {

    }

    public virtual void TakeDamage(float damage)
    {
        curHp = curHp - damage;

        if (curHp /maxHp <= 0.1f) // 10%보다 체력이 낮을 때 
        {
            //행동 불능 + 적이 공격할 수 없는 상황 + 일정 시간이 지나면 다시 부활
            agent.isStopped = true;
            knockDown = true;


        }
    }
}
