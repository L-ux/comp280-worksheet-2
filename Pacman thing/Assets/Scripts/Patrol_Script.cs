using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Script : BasicGhostScript
{
    [SerializeField]
    Vector3 pointPos;
    bool chasingPlayer = false;

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
        else if (!isScared) // if chasing - choose a random point, only chase when Within x units of that point
        {
            float selfToPointDistance = (transform.position - pointPos).magnitude;
            float pointToPlayerDistance = (pointPos - playerTransform.transform.position).magnitude;
            float selfToPlayerDistance = (transform.position - playerTransform.transform.position).magnitude;

            if (selfToPointDistance <= 2f) // if reached waypoint, find new waypoint
            {
                setRandomPointPos();
            }

            if (pointToPlayerDistance <= 7f || selfToPlayerDistance <= 4f)
            {
                navMeshAgent.SetDestination(playerTransform.transform.position);
                chasingPlayer = true;
            }
            else
            {
                navMeshAgent.SetDestination(pointPos);
                chasingPlayer = false;
            }
        }
    }

    protected override void Start()
    {
        setRandomPointPos(); // create random waypoint at start
        base.Start();
    }

    void setRandomPointPos() // creates a random waypoint within inner 60/70% of map
    {
        float xPos = Random.Range(-8, 10) - 0.5f;   
        float yPos = Random.Range(-11,12);

        pointPos = new Vector3(xPos, 0.25f, yPos);
    }

    IEnumerator waitForPlayerGone() // coroutine that waits for the player to be out of this ghosts hunting range, than gives the ghost a new patrol point
    {
        yield return new WaitUntil(() => chasingPlayer == true); // wait for chase to start and then stop again
        yield return new WaitUntil(() => chasingPlayer == false);
        setRandomPointPos(); // after chase has stopped, get new waypoint
    }
}
