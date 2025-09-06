using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Character : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;

    public Transform[] destinations; //��������
    public int targetIdx; //���ߵǴ� �������� �ε���

    public bool arrived;

    //public float waitTime;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetIdx = -1;
        //SetDestination() �Լ��� ����ؼ� Ŭ���� ��ġ�� �÷��̾ ������.
        agent.SetDestination(new Vector3(0, 0, 0));

        NextMove();



    }

    // Update is called once per frame
    void Update()
    {
        
        if (arrived == false)
        {
            if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (agent.hasPath == false || agent.velocity.sqrMagnitude == 0)
                {

                    //Debug.Log("����");

                    //���� �� �ش� ��ġ�� Destination ������Ʈ Arrived �Լ� ȣ���ϱ�
                    //+ Ŭ������ �̵��� ���������� ó���ȵǰ� ó��
                    
                    //destinations �迭�� ���ִ� Ư�� ��ҿ� ���� -> destinations ������Ʈ �ҷ�����
                    //Arrived �Լ� ȣ��

                    arrived = true;
                    destinations[targetIdx].GetComponent<Destination>().Arrived(this);



                }
            }
        }
        


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("Plane")))
            {
                Vector3 destination = hit.point;


                arrived = false;
                agent.SetDestination(destination);

                // hit.point �����ϸ� �α� ���
                float dis = Vector3.Distance(transform.position, hit.point);


            }


        }
    }
    
    public int RandomNumber()
    {
        int randomNum = Random.Range(0, destinations.Length);

        while (true)
        {
            // ���� ��� ���°� �����ϴ� �ڵ� + ������ �ϴ� ������ character ������ �����Ǿ� ������ �ٽ� ���ڸ� �̵���
            if (randomNum != targetIdx && destinations[randomNum].GetComponent<Destination>().character== null)
                break;
            else 
            {
                randomNum = Random.Range(0, destinations.Length);
            }
        }
        


        return randomNum;
    }

    public void MoveTo()
    {
        arrived = false;
        agent.SetDestination(destinations[targetIdx].position);
    }

    public void NextMove()
    {
        targetIdx = RandomNumber();
        MoveTo();
    }


}
