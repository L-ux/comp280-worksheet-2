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
    
    public bool freeToGo = false;
    public float initialWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = this.GetComponent<NavMeshAgent>();
        //SetDestination();
        StartCoroutine(waitScaredChange());
        spawnPos = transform.position;
        StartCoroutine(waitBeforeLeaving(initialWaitTime));
    }

    // Update is called once per frame
    protected virtual void SetDestination()
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
    
    protected virtual void Update()
    {
        if(freeToGo)
        {
            navMeshAgent.enabled = true;
            SetDestination();
        }
        // else
        // {
        //     navMeshAgent.enabled = false;
        // }
    }


    public void doDead()
    {
        freeToGo = false;
        navMeshAgent.enabled = false;
        transform.position = spawnPos; // move it back to its spawn point
        StartCoroutine(waitBeforeLeaving(5));
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

    IEnumerator waitBeforeLeaving(float time)
    {
        yield return new WaitForSeconds(time);
        freeToGo = true;        
    }
}
