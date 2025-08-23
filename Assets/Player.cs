using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;

    public Transform[] destinations;
    public int randomNum;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetDestination() �Լ��� ����ؼ� Ŭ���� ��ġ�� �÷��̾ ������.
        agent.SetDestination(new Vector3(0, 0, 0));

        RandomNumber();


        MoveTo();



    }

    // Update is called once per frame
    void Update()
    {
        if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0)
            {
                //Debug.Log("����");

            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Plane")))
            {
                Vector3 destination = hit.point;

                agent.SetDestination(destination);

                // hit.point �����ϸ� �α� ���
                float dis = Vector3.Distance(transform.position, hit.point);


            }


        }
    }
    //hit.point�� �����ϸ� "����"�α� ����ϱ�
    //�� �� ĳ���Ͱ� �̵��� �� �ִ� ���� ���ؼ� �����ϰ� �̵��� �� �ְ� ó�����ּ���.
    //ĳ���Ͱ� �׳� �̵� ���϶� ���� Ŭ���� �Ǹ� Ŭ���� ���� ���� �̵��ϰ� ó��

    public int RandomNumber()
    {
        int randomNum = Random.Range(0, destinations.Length-1);
        
        return randomNum;
    }

    public void MoveTo()
    {
        agent.SetDestination(destinations[randomNum].position);
    }


}
