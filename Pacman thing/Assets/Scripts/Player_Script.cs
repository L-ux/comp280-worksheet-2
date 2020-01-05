using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Script : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody rb;
    public CharacterController charcont;
    float ypos;

    public Text scoreTex;
    public Text timeTex;
    float PillPickupTime;
    public float PowerPillDuration = 10f;

    public static bool hasPowerPill = false;
    static int totalScore = 0;
    int pelletScore = 0;
    int killScore = 0;
    int killMulti = 1;

    void Start()
    {
        PillPickupTime = -PowerPillDuration;
        ypos = transform.position.y;
        scoreTex.text = totalScore.ToString();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        charcont.Move(move * moveSpeed * Time.deltaTime);
        Vector3 amhere = transform.position;
        transform.position = new Vector3(amhere.x, ypos, amhere.z);

        // pill time text
        float timedif = Time.time - PillPickupTime;
        if(timedif > PowerPillDuration) timedif = PowerPillDuration;
        timeTex.text = string.Format("{0:0.0}",(PowerPillDuration-timedif));
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ghost") // some issue here, sometimes collisions dont register until shimmying in another direction
        {
            if (hasPowerPill == false)
            {
                Debug.Log("ah!");
                Destroy(gameObject); 
            }
            else
            {
                other.gameObject.GetComponent<BasicGhostScript>().doDead();
                killScore += 10 * killMulti;
                killMulti++;
                //Destroy(other.gameObject);
            }
        }
        else if (other.tag == "PointPill")
        {
            Destroy(other.gameObject);
            pelletScore++;
            doScoreCheck();
        }
        else if (other.tag == "PowerPill")
        {
            Destroy(other.gameObject);
            hasPowerPill = true;
            PillPickupTime = Time.time;
            StartCoroutine(powerPillTime());
        }
    }

    void doScoreCheck()
    {
        totalScore = pelletScore + killScore;
        scoreTex.text = totalScore.ToString();
        if (pelletScore >= 244) // max score value
        {
            // win game ?
        }
    }

    int pillsTaken = 0;
    IEnumerator powerPillTime()
    {
        pillsTaken++;
        yield return new WaitForSeconds(PowerPillDuration);
        pillsTaken--;
        if (pillsTaken == 0)
        {
            hasPowerPill = false;
            killMulti = 1;
        }
    }
}
