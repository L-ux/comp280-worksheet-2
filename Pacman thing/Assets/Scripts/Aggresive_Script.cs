using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggresive_Script : BasicGhostScript
{
    protected override void SetDestination()
    {
        if (playerTransform == null)
        {
            navMeshAgent.enabled = false;
        }
        else if (!isScared)
        {
            navMeshAgent.SetDestination(playerTransform.transform.position);
        }
        else if (isScared)
        {
            Vector3 scaredDestination = transform.position + (transform.position-playerTransform.transform.position);
            navMeshAgent.SetDestination(scaredDestination);
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
