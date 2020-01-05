using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_Script : BasicGhostScript
{
    [SerializeField]
    Vector3 pointPos;

    protected override void SetDestination()
    {
        if (playerTransform == null) // if player no longer exists (dies) . stop navmesh-ing
        {
            navMeshAgent.enabled = false;
        }
        else
        {
            float selfToPointDistance = (transform.position - pointPos).magnitude;
            float selfToPlayerDistance = (transform.position - playerTransform.transform.position).magnitude;

            if (isScared || selfToPlayerDistance < 6f) // if scared OR too close to player - run directly away from player
            {
                Vector3 scaredDestination = transform.position + (transform.position-playerTransform.transform.position);
                navMeshAgent.SetDestination(scaredDestination);
                if (selfToPlayerDistance < 6f) // if too close to player, find a new point to go to 
                {
                    setRandomPointPos();
                }
            }
            else  // if not scared and not too close to player - move towards random point, when reaching point, choose a new one at random
            {
                if (selfToPointDistance <= 2f)  // if reached the target waypoint, set new one
                {
                    setRandomPointPos();
                }
                navMeshAgent.SetDestination(pointPos);
            }
        }
    }

    protected override void Start()
    {
        setRandomPointPos(); // set random start point at beginning
        base.Start();
    }

    void setRandomPointPos()
    {
        float xPos = Random.Range(-8, 10) - 0.5f;  // X range of inner 60/70% of map
        float yPos = Random.Range(-11,12); // technically this is Z position, inner range of inner 60/70% of map

        pointPos = new Vector3(xPos, 0.25f, yPos);
    }

}
