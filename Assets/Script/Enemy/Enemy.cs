using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class Enemy : MonoBehaviour
{
    public EnemyState state;
    
    public float maxHp;
    public float curHp;
    public float moveSpeed;
    
    public float attackRange;
    public float attackPower;

    // 어택스피드가 올라갈수록 느려짐.
    public float attackSpeed;
    public float attackCoolTime;

    public Animator animator;
    public EnemyAnimEvent animEvent;

    [SerializeField] public NavMeshAgent agent;

    public IEnemyTarget target;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animEvent = GetComponentInChildren<EnemyAnimEvent>();
        
        // ()가 포함되고 안되고 에 대한 이해 필요! () 는 함수의 결과값을 넣는 것이기 때문. 지금은 그냥 주소값만 넣는것.
        animEvent.startAttackListener = StartAttack;
        animEvent.endAttackListener = EndAttack;
        animEvent.shootingTimingListener = ShootingTiming;
    }


    private void Start()
    {
        SetState(EnemyState.Idle);

        attackCoolTime = 0f;


    }

    public void SetState(EnemyState state)
    {
        this.state = state; 

        //
    }


    private void Update()
    {
        attackCoolTime += Time.deltaTime;

        if (state == EnemyState.Idle)
        {
            UpdateIdle();
        }
        else if (state == EnemyState.Approaching)
        {
            UpdateApproaching();
        }
        else if (state == EnemyState.Attack)
        {
            UpdateAttack();
        }
    }
    public void UpdateIdle()
    {
        target = CharacterManager.Instance.FindClosest(transform.position);


        if (target != null)
        {
            SetState(EnemyState.Approaching);
            //캐릭터한테 가라!
            animator.SetBool("Walking", true);

            agent.SetDestination(target.Transform.position);
            return;
        }
        agent.isStopped = true;
    }
    public void UpdateApproaching()
    {
        //적이 타겟(캐릭터)에 가까이 갔는지 확인
        float distance = Vector3.Distance(transform.position, target.Transform.position);

        if (distance <= attackRange)
        {
            animator.SetBool("Walking", false);
            SetState(EnemyState.Attack);
            
            return;
        }

        if (target == null || !target.Transform.gameObject.activeSelf)
        {
            SetState(EnemyState.Idle);
            return;
        }


        agent.isStopped = false;
        if (target != null)
        {
            agent.SetDestination(target.Transform.position);

        }

    }
    public void UpdateAttack()
    {
        // 공격 쿨타임 2초 (공격 관련 기능)
        if (attackCoolTime >= 2)
        {
            Attack();
            attackCoolTime = 0;

        }
        else
        {
            SetState(EnemyState.Idle);

            //타겟이 살아있으면 = EnemyStateApproaching
            //타겟이 죽었으면 = EnemyStateIdle 로 바꾸기

        }



    }
    // 2초가 지나야지 Attack() 함수를 호출할 수 있는 조건이 됨.
    public virtual void Attack() 
    {
        agent.isStopped = true;
        animator.SetTrigger("Attack");
        
        // 공격 방향
        transform.forward = (target.Transform.position - transform.position).normalized;
    }

    //공격 애니메이션 시작 시
    public virtual void StartAttack()
    {

    }

    //공격 애니메이션 끝날 때
    public virtual void EndAttack()
    {
        SetState(EnemyState.Idle);
    }

    //총알이 딱 발사되는 애니메이션 실행 시
    public virtual void ShootingTiming()
    {

    }

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy OnTriggerEnter" + other.name);
        if (other.CompareTag("Character"))
        {
            target = other.GetComponent<IEnemyTarget>();
            Attack();
        }
        else if (other.CompareTag("Building"))
        {
            target = other.GetComponent<IEnemyTarget>();
            Attack();
        }
    }

}
public enum EnemyState
{
    Idle,
    Approaching,
    Attack

}