using UnityEngine;
using System.Collections;

public class AiStateManager : MonoBehaviour {

    public string enemyState;
    public Vector3 searchPosition;
    public float enemyNumber;
    public GameObject[] enemies;



    void Start () {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        enemyState = "Idle";
	}
    GameObject GetClosest(GameObject[] arrayObjects)
    {
        GameObject closest = null;
        foreach (GameObject t in arrayObjects)
        {
            float dist = Vector3.Distance(t.transform.position, searchPosition);
            float minDist = Mathf.Infinity;
            if (dist < minDist)
            {
                closest = t;
                minDist = dist;
            }
        }
        return closest;
    }

    public void Search()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            GetClosest(enemies).GetComponent<AiAction>().currentTask = "Searching";


        }

    }

    public void Engage()
    {
        enemyState = "Engage";
        for (int i = 0; i < enemies.Length; i++)
        {
            if ((i + 3) % 3 == 0)
                enemies[i].GetComponent<AiAction>().currentTask = "Ranged";
            else if ((i + 3) % 3 == 1)
                enemies[i].GetComponent<AiAction>().currentTask = "Melee";
            else if ((i + 3) % 3 == 2)
                enemies[i].GetComponent<AiAction>().currentTask = "Protect";



        }
    }
  
}
