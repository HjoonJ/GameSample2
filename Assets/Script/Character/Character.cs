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

    //풀어썼을 때
    //public Transform Transform
    //{
    //    get
    //    {
    //        return transform;
    //    }
    //}

    public float maxHp;
    public float curHp;

    public float attackPower;

    public float attackCoolTime; // 타이머
    
    public float attackSpeed; // 공격속도 == 1

    public Transform attackPoint;

    public bool attacking; //공격 중인지 유무

    //타격범위
    public float attackLength;
    //적 탐지범위
    public float attackRange;

    public CharacterState state;

    public Enemy closestEnemy;
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
        state = CharacterState.Normal;

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
        

        if (state == CharacterState.Normal)
        {
            UpdateNormal();
        }
        // 배틀 모드로 전환 시
        else
        {
            UpdateBattle();   

        }

        // 자주 FindClosestEnemy 함수를 부르는 것 or 공격 후 현재 closestEnemy 와의 거리를 다시 한번 측정하여 attackRange > distance 가 되면 FindClosestEnemy 함수 부르기.
        

        // 화면에 클릭하는 곳으로 캐릭터 이동
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Plane")))
        //    {
        //        Vector3 destination = hit.point;


        //        arrived = false;
        //        agent.SetDestination(destination);

        //        // hit.point 도착하면 로그 출력
        //        float dis = Vector3.Distance(transform.position, hit.point);


        //    }


        //}
    }
    
    public virtual void UpdateNormal()
    {
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


    }

    public virtual void UpdateBattle()
    {
        // 등장 여부와 관계없이 대상 적이 없는 경우
        if (closestEnemy == null || closestEnemy.gameObject.activeSelf == false)
        {
            
            agent.isStopped = true;

            closestEnemy = EnemyManager.Instance.FindClosestEnemy(transform.position);

            if (closestEnemy == null)
            {
                return;
            }
            agent.isStopped = false;
            //agent.SetDestination(closestEnemy.transform.position);


        }



        // 대상 적이 어택 레인지에 들어온 경우 


        // 캐릭터 이동 중

        // 적과 캐릭터의 거리 측정 , attackRange 범위 안에 들어오면 공격시작 + 바라보기
        //LookAt();
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
        if (gm == GameMode.Normal)
        {
            //모든 코루틴이나 행동을 종료하고 Idle 상태로 만듦.
            state = CharacterState.Normal;

            //평상시 행동을 적용.
            NextMove();
        }

        if (gm == GameMode.Battle)
        {

            state = CharacterState.Battle;

            attackCoolTime = attackSpeed;

            CharacterBattleMode();


            //전투 모드 시
            //캐릭터가 [계속 싸울 수 있도록] 만들기
            //1. agent 멈춤 2. 가까운 적을 찾기 3. 가까운 적으로 이동  4. 사정거리만큼 가까이 가서 멈추고 때리기 5. 가까운 적을 처치 시 다음 적 찾기 


        }
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

    public virtual void CharacterBattleMode()
    {
        //agent.isStopped = true;

        // 적을 못찾았을 때를 구현!!해야함 + 적closestEnemy가 null 이 아니고 비활성화 상태인 경우! 

        closestEnemy = null;

        //closestEnemy = EnemyManager.Instance.FindClosestEnemy(this.transform.position);

        //agent.SetDestination(closestEnemy.transform.position);// 가까운 적으로 이동



    }

    public virtual void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackLength, LayerMask.GetMask("Enemy"));
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<Enemy>().TakeDamage(attackPower);
        
        }

    }

    public virtual void LookAt(Transform tr)
    {
        //Debug.Log("Character1 LookAt()");
        Vector3 vec = (tr.position - transform.position);
        Vector3 dir = vec.normalized;
        dir.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);


    }


}

public enum CharacterState
{
    Normal,
    Battle

}




