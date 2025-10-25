using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingDestination : Destination
{
    public Collection[] collections;

    public void Start()
    {
        collections = GetComponentsInChildren<Collection>();

        for (int i = 0; i < collections.Length; i++)
        {
            collections[i].gameObject.SetActive(true);
        }

    }

    public void Update()
    {
        //if (character.agent.remainingDistance < 0.1)
        //{

        //}


    }

    public IEnumerator CoActive(Collection collection)
    {
        yield return new WaitForSeconds(5);
        collection.gameObject.SetActive(true);
    }

    public override bool CanTake()
    {
        for (int i = 0; i < collections.Length; i++)
        { 
            if (collections[i].gameObject.activeSelf && isUsing == false)
            {
                return true;
            }

        }
        

        return false;

        //collections 중에 수집할 수 있는게 있는지 확인;

    }


    public override void Arrived(Character c)
    {
        base.Arrived(c);



    }

    public override IEnumerator CoAction(float waitTime)
    {
        while (true)
        {
            //랜덤하게 collections 중 하나 뽑기 + 캐릭터 이동시키기
            Collection collection = RandomCollection();
            if (collection == null)
                break;

            character.agent.SetDestination(collection.transform.position);

            while (true)
            {
                yield return null;

                if (character.agent.pathPending == false && character.agent.remainingDistance < 0.1)
                {
                    break;
                }

            }

            yield return new WaitForSeconds(1f); //수집 애니메이션 + 수집! 대용

            collection.gameObject.SetActive(false);

            StartCoroutine(CoActive(collection));

        }
        


        //더 이상 수집할 수 있는 대상이 없으면
        //다음 장소로 이동




        yield return null;

        Leave(character);
        

    }

    List<Collection> activeCollections = new List<Collection>();
    public Collection RandomCollection()
    {
        //collections = GetComponentsInChildren<Collection>();
        
        //return collections[Random.Range(0, collections.Length)];

        activeCollections.Clear();

        for (int i = 0;i < collections.Length; i++)
        {
            if (collections[i].gameObject.activeSelf)
            {
                activeCollections.Add(collections[i]);
            }
        }

        if (activeCollections.Count <= 0)
            return null;

        return activeCollections[Random.Range(0, activeCollections.Count)];




    }
}
