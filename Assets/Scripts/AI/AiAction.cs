using UnityEngine;
using System.Collections;

public class AiAction : MonoBehaviour
{
    public GameObject[] keyPositions;
    public GameObject[] covers;
    public GameObject[] idlePath;
    int idlePathIndex=0;
    public float rotateSpeed = 2f;
    public float distanceToNextNode = 1.0f;
    GameObject currentTarget;
    Rigidbody rb;
    UnityEngine.AI.NavMeshAgent nmAgent;
    AiStateManager enemyScript;
    public string currentTask;
    public GameObject player;
    bool doneSearch = false;
    public float fireRate = 4f;
    public float meleeRange=2f;
    public float meleeCD=2f;
    public float vibrationTimer = 3f;
    GameObject GetClosest(GameObject[] arrayObjects)
    {
        GameObject closest = null;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in arrayObjects)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            float minDist = Mathf.Infinity;
            if (dist < minDist&&dist>=0.4f)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }
    // Use this for initialization
    void Start()
    {
        enemyScript = GameObject.FindWithTag("enemyController").GetComponent<AiStateManager>();
        player = GameObject.FindWithTag("Player");
        covers = GameObject.FindGameObjectsWithTag("CoverNode");
        keyPositions = GameObject.FindGameObjectsWithTag("KeyNode");
        rb = GetComponent<Rigidbody>();
        nmAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (idlePath.Length > 0)
            currentTarget = idlePath[idlePathIndex];

        if (!currentTarget)
        {
            Debug.Log("Target not found.");
        }
        else
        {
            nmAgent.SetDestination(currentTarget.transform.position);
        }

        currentTask = "Idle";
       
    }

    // Update is called once per frame
    void Update()
    {

        if (currentTask == "Idle")

        {

            if ((Vector3.Distance(currentTarget.transform.position, transform.position)) <= distanceToNextNode)
            {

                idlePathIndex++;


                if (idlePathIndex >= idlePath.Length)
                    idlePathIndex = 0;

                currentTarget = idlePath[idlePathIndex];

                nmAgent.SetDestination(currentTarget.transform.position);
            }
        }

        else if (currentTask == "Searching")
        {
             

            if ((Vector3.Distance(enemyScript.searchPosition, transform.position)) <= distanceToNextNode)
            {

                doneSearch = true;

            }
           

            if (doneSearch)
            {
                currentTarget = GetClosest(covers);
                nmAgent.SetDestination(currentTarget.transform.position);
                if ((Vector3.Distance(currentTarget.transform.position, transform.position)) <= distanceToNextNode)
                {

                    doneSearch = false;

                }

            }
            else if (!doneSearch)
            {
                nmAgent.SetDestination(enemyScript.searchPosition);
            }
        }

        else if (currentTask == "Melee")
        {
            launchExplode();



        }
        else if (currentTask == "Ranged")
        {


            if (fireRate < 4)
            {
                fireRate += Time.deltaTime;
            }
            
            float step = rotateSpeed * Time.deltaTime;
            Vector3 direction = player.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, direction, step, 0.0F);
            Debug.DrawRay(transform.position, newDir, Color.red);
            transform.rotation = Quaternion.LookRotation(newDir);
            /*
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    if (fireRate >=3.9f)
                    {
                        nmAgent.SetDestination(transform.position);

                        GetComponent<EnemyShoot>().rayShot();
                        fireRate = 0;
                        
                    }
                }
                else
                {
                    nmAgent.SetDestination(player.transform.position);

                }
               

            }
            */
            if (fireRate >= 3.9f)
            {
                nmAgent.SetDestination(transform.position);

                GetComponent<EnemyShoot>().launchShot(player.transform);
                fireRate = 0;
            }
            if (Vector3.Distance(player.transform.position, transform.position) < 3f)
            {
                currentTask = "Melee";

            }


        }
        else if (currentTask == "Protect")
        {
            nmAgent.SetDestination(GetClosest(keyPositions).transform.position);


        }


        }


    public void launchExplode()
    {
        if (vibrationTimer > 0f)
        {
            vibrationTimer -= Time.deltaTime;
            Debug.Log("BZBZBBZBZBZBBZ"+vibrationTimer);
        }
        else
        {
            nmAgent.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            if (transform.position.y <= 8f)
            {
                rb.AddForce(transform.up * 1.2f, ForceMode.Impulse);

            }

            else
            {
                Vector3 target = player.transform.position - transform.position;
                target.Normalize();
                rb.AddForce(target * 1.2f, ForceMode.Impulse);
                rb.AddForce(transform.up * -1.2f, ForceMode.Impulse);

            }
        }

    }
   
 
}


