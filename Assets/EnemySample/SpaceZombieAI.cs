using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    enum State { Patrol, Roar, Chase, Attack }
    State currentState = State.Patrol;

    [Header("References")]
    public Transform startPoint;        
    public Transform endPoint;          
    public Transform eyePoint;

    [Header("Detection")]
    public float sightRange;
    public float sightAngle;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 3f;
    public int attackDamage = 10;

    [Header("Patrol")]
    public float idleTimeMin = 1f;
    public float idleTimeMax = 4f;

    NavMeshAgent agent;
    Animator animator;
    Vector3 currentPatrolTarget;
    Transform playerTarget;
    float idleTimer;
    float currentIdleDuration;
    float attackTimer;
    bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        PickNewPatrolTarget();
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Patrol: PatrolUpdate(); break;
            case State.Roar: RoarUpdate(); break;
            case State.Chase: ChaseUpdate(); break;
            case State.Attack: AttackUpdate(); break;
        }
    }

    // ───── Patrol ─────
    void PatrolUpdate()
    {
        if (DetectPlayer())
        {
            EnterRoar();
            return;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            animator.SetInteger("State", 0);
            idleTimer += Time.deltaTime;

            if (idleTimer >= currentIdleDuration)
            {
                idleTimer = 0f;
                PickNewPatrolTarget();
            }
        }
        else
        {
            animator.SetInteger("State", 1);
        }
    }

    // ★ StartPoint와 EndPoint 사이 랜덤 지점 선택
    void PickNewPatrolTarget()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("StartPoint 또는 EndPoint가 지정되지 않았어!");
            return;
        }

        // 두 점의 중심과 반경 계산
        Vector3 center = (startPoint.position + endPoint.position) * 0.5f;
        float radius = Vector3.Distance(startPoint.position, endPoint.position) * 0.5f;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDir = Random.insideUnitSphere * radius;
            randomDir += center;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDir, out hit, radius, NavMesh.AllAreas))
            {
                currentPatrolTarget = hit.position;
                agent.SetDestination(currentPatrolTarget);
                currentIdleDuration = Random.Range(idleTimeMin, idleTimeMax);
                return;
            }
        }

        currentIdleDuration = Random.Range(idleTimeMin, idleTimeMax);
    }

    // ───── Roar ─────
    void EnterRoar()
    {
        currentState = State.Roar;
        agent.isStopped = true;
        animator.SetInteger("State", 2);
        Invoke(nameof(FinishRoar), 2f);
    }

    void RoarUpdate()
    {
        if (playerTarget == null) return;
        transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z));
    }

    void FinishRoar()
    {
        if (playerTarget == null)
        {
            ReturnToPatrol();
            return;
        }
        currentState = State.Chase;
        agent.isStopped = false;
    }

    // ───── Chase ─────
    void ChaseUpdate()
    {
        if (isAttacking) return;

        if (playerTarget == null || !IsTargetAlive())
        {
            ReturnToPatrol();
            return;
        }

        agent.SetDestination(playerTarget.position);
        animator.SetInteger("State", 1);

        if (Vector3.Distance(transform.position, playerTarget.position) <= attackRange)
        {
            currentState = State.Attack;
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            attackTimer = attackCooldown;
        }
    }

    // ───── Attack ─────
    void AttackUpdate()
    {
        if (playerTarget == null || !IsTargetAlive())
        {
            ReturnToPatrol();
            return;
        }

        transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z));

        if (isAttacking) return;

        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            float dist = Vector3.Distance(transform.position, playerTarget.position);

            if (dist > attackRange + 0.3f)
            {
                currentState = State.Chase;
                agent.isStopped = false;
                return;
            }

            attackTimer = 0f;
            isAttacking = true;

            animator.SetInteger("State", 3);
            animator.Play("Attack", 0, 0f);

            var hp = playerTarget.GetComponent<PlayerHP>();
            if (hp != null) hp.TakeDamage(attackDamage);

            Invoke(nameof(FinishAttack), attackCooldown);
        }
    }

    void FinishAttack()
    {
        isAttacking = false;
    }

    // ───── Patrol 복귀 ─────
    void ReturnToPatrol()
    {
        currentState = State.Patrol;
        playerTarget = null;
        isAttacking = false;
        CancelInvoke(nameof(FinishAttack));
        agent.isStopped = false;
        PickNewPatrolTarget();
    }

    // ───── 시야 판정 ─────
    bool DetectPlayer()
    {
        Collider[] hits = Physics.OverlapSphere(eyePoint.position, sightRange, playerMask);
        if (hits.Length == 0) return false;

        foreach (var hit in hits)
        {
            Vector3 dirToTarget = (hit.transform.position - eyePoint.position).normalized;
            float angle = Vector3.Angle(eyePoint.forward, dirToTarget);
            if (angle > sightAngle * 0.5f) continue;

            float distToTarget = Vector3.Distance(eyePoint.position, hit.transform.position);
            if (Physics.Raycast(eyePoint.position, dirToTarget, distToTarget, obstacleMask))
                continue;

            playerTarget = hit.transform;
            return true;
        }
        return false;
    }

    bool IsTargetAlive()
    {
        var hp = playerTarget.GetComponent<PlayerHP>();
        return hp != null && hp.currentHP > 0;
    }

    // ───── Gizmo ─────
    void OnDrawGizmosSelected()
    {
        if (eyePoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(eyePoint.position, sightRange);

            Vector3 leftDir = Quaternion.Euler(0, -sightAngle * 0.5f, 0) * eyePoint.forward;
            Vector3 rightDir = Quaternion.Euler(0, sightAngle * 0.5f, 0) * eyePoint.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(eyePoint.position, leftDir * sightRange);
            Gizmos.DrawRay(eyePoint.position, rightDir * sightRange);
        }

        // ★ 순찰 범위 표시 (StartPoint ~ EndPoint)
        if (startPoint != null && endPoint != null)
        {
            Vector3 center = (startPoint.position + endPoint.position) * 0.5f;
            float radius = Vector3.Distance(startPoint.position, endPoint.position) * 0.5f;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(center, radius);
            Gizmos.DrawLine(startPoint.position, endPoint.position);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPoint.position, 0.5f);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(endPoint.position, 0.5f);
        }
    }
}