using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;

    public Transform[] destinations; //��������
    public int targetIdx; //���ߵǴ� �������� �ε���

    public bool arrived;

    public float waitTime = 2f;


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

        targetIdx = RandomNumber();
        
        MoveTo();



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
                    destinations[targetIdx].GetComponent<Destination>().Arrived();


                    //targetIdx = RandomNumber();
                    //MoveTo();

                    StartCoroutine(WaitAndMove(waitTime));


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
            if (randomNum != targetIdx)
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

    IEnumerator WaitAndMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        targetIdx = RandomNumber();
        MoveTo();
    }



}
