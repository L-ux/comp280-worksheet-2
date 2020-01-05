using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush_Script : BasicGhostScript
{
    protected override void SetDestination()
    {
        if (playerTransform == null) // if player no longer exists (dies) . stop navmesh-ing
        {
            navMeshAgent.enabled = false;
        }
        else if (isScared) // if scared - run directly away from player
        {
            Vector3 scaredDestination = transform.position + (transform.position-playerTransform.transform.position);
            navMeshAgent.SetDestination(scaredDestination);
        }
        else if (!isScared) // if chasing - try to ambush player - aim for location in front of player
        {
            float distaceToPlayer = (transform.position - playerTransform.transform.position).magnitude;
            Vector3 destination = playerTransform.transform.position;

            // if more than 7 units away AND not within/just outside the cage.. do this
            if (distaceToPlayer > 7f && !(transform.position.x >= -5 && transform.position.x <=5 && transform.position.z >= -2 && transform.position.z <= 5)) // if more than 5 units away from the player...
            {
                destination += (playerTransform.transform.forward * 5); // ... target 5 units in front of the player

                // issues arise when this location is within the ghost cage, as the ghost wants to get back into the cage to get as close as possible.
                while (destination.x >= -4 && destination.x <=4 && destination.z >= -1 && destination.z <= 4)
                {
                    // this loop should reduce the distance of searching until the destination is no longer within the cage
                    destination -= playerTransform.transform.forward; 
                }
            }
            
            navMeshAgent.SetDestination(destination);
        }
    }
}
