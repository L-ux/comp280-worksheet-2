using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicGhostScript : MonoBehaviour
{
    public Transform playerTransform; // player object
    protected NavMeshAgent navMeshAgent; // protected so inherited classes can use

    public Material angryMat;   // materials for when chasing
    public Material scaredMat;  // materials for when scared

    public bool isScared = false;   // bool for when is scared

    public Vector3 spawnPos;    // stores starting position upon game start
    
    public bool freeToGo = false; // freeToGo is false at the start of the game and after dying, so they stay in the cage a little first before entering the arena
    public float initialWaitTime; // how long this ghost sits around for at the start of the game

    // Start is called before the first frame update
    protected virtual void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();   // grabs own nav agent
        StartCoroutine(waitScaredChange()); // start coroutine that waits for PowerPill pickup
        spawnPos = transform.position;  // note the staing position
        StartCoroutine(waitBeforeLeaving(initialWaitTime)); // 
    }

    // Update is called once per frame
    protected virtual void SetDestination()
    {
        if (playerTransform == null)    // if player no longer exists (dies) . stop navmesh-ing
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
    
    void Update()
    {
        if(freeToGo) // if ghosts has got the all clear to leave the cage...
        {
            navMeshAgent.enabled = true; // ... make sure to enable its navmesh again
            SetDestination();
        }
    }


    public void doDead()
    {
        freeToGo = false;
        navMeshAgent.enabled = false; // disable navmesh before moving its location
        transform.position = spawnPos; // move it back to its spawn point
        StartCoroutine(waitBeforeLeaving(2));
    }

    IEnumerator waitScaredChange() // coroutine to keep an eye out for Power Pill state
    {
        yield return new WaitUntil(() => Player_Script.hasPowerPill != isScared); // wait for the players Power Pill state to change
        isScared = Player_Script.hasPowerPill; // update this ghosts scared status to reflect the Power Pill status
        if(isScared)
            gameObject.GetComponent<Renderer>().material = scaredMat; // if scared, look scared
        else
            gameObject.GetComponent<Renderer>().material = angryMat; // if angry, look angry
        StartCoroutine(waitScaredChange());
    }

    IEnumerator waitBeforeLeaving(float time) // waits for given amount of time, then tells ghost it is free to move n stuff
    {
        yield return new WaitForSeconds(time);
        freeToGo = true;        
    }
}
