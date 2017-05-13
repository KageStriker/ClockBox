using UnityEngine;
using System.Collections;


public class AiSight : MonoBehaviour {

    public float fieldOfViewAngle = 110f;
    private SphereCollider col;
    public GameObject player;
    public float elapsedTime;
    AiStateManager enemyScript;
    // Use this for initialization
    void Start () {

        col = GetComponent<SphereCollider>();
        player = GameObject.FindWithTag("Player");
        elapsedTime = 3.5f;
        enemyScript = GameObject.FindWithTag("enemyController").GetComponent<AiStateManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (elapsedTime <= 0f && enemyScript.enemyState != "Engage")
        {
            enemyScript.Engage();
        }

        if(Vector3.Distance(player.transform.position, transform.position)<4f && enemyScript.enemyState != "Engage")    
        {
            enemyScript.Engage();

        }

    }

    void OnTriggerStay (Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        if (Vector3.Distance(other.transform.position, transform.position) <= 15f&& enemyScript.enemyState != "Engage")
                        {
                            enemyScript.Engage();
                        }
                        else if(enemyScript.enemyState!="Engage")
                        {
                            enemyScript.Search();
                            enemyScript.searchPosition = other.transform.position;
                            elapsedTime -= Time.deltaTime;

                        }
                    }
                   
                }
            }
       


        }

    }
}
