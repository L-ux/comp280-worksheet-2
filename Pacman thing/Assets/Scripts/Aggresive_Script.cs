using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggresive_Script : BasicGhostScript
{

    // this happens to be the same as the basic ghost script, as it just goes straight for the player
    protected override void SetDestination()
    {
        if (playerTransform == null) // if player no longer exists (dies) . stop navmesh-ing
        {
            navMeshAgent.enabled = false;
        }
        else if (!isScared) // if chasing - go straight for player
        {
            navMeshAgent.SetDestination(playerTransform.transform.position);
        }
        else if (isScared) // if scared - run directly away from player
        {
            Vector3 scaredDestination = transform.position + (transform.position-playerTransform.transform.position);
            navMeshAgent.SetDestination(scaredDestination);
        }
    }
}
