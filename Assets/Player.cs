using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetDestination() �Լ��� ����ؼ� Ŭ���� ��ġ�� �÷��̾ ������.
        agent.SetDestination(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
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


}
