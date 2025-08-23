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
        //SetDestination() 함수를 사용해서 클릭한 위치로 플레이어를 보내기.
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

                // hit.point 도착하면 로그 출력
                float dis = Vector3.Distance(transform.position, hit.point);


            }


        }
    }
    //hit.point에 도착하면 "도착"로그 출력하기
    //맵 상에 캐릭터가 이동할 수 있는 곳을 정해서 랜덤하게 이동할 수 있게 처리해주세요.
    //캐릭터가 그냥 이동 중일때 만약 클릭이 되면 클릭된 곳을 먼저 이동하게 처리


}
