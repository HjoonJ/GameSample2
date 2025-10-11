using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationManager : MonoBehaviour
{
    public static DestinationManager Instance;
    public Destination[] destinations; //갈수 있는 목적지들
    public int targetIdx; //가야되는 목적지의 인덱스

    private void Awake()
    {
        Instance = this;

        targetIdx = -1;
        destinations = FindObjectsOfType<Destination>();
        
        //유니티6용
        //destinations = FindObjectsByType<Destination>(FindObjectsSortMode.InstanceID);
    }

    List<Destination> canTakeDestinations = new List<Destination>();

    public Destination GetEmptyDestination()
    {

        canTakeDestinations.Clear();

        for (int i = 0; i < destinations.Length; i++)
        {
            if (destinations[i].CanTake() == true)
            {
                canTakeDestinations.Add(destinations[i]);
            }

        }

        if (canTakeDestinations.Count <= 0)
        {
            return null;
        }

        int randomNum = Random.Range(0, canTakeDestinations.Count);

        return canTakeDestinations[randomNum]; 


        //targetIdx = RandomNumber();
        //return destinations[targetIdx];
    }

}
