using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] targets;

    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
        if (targets.Length == 0) return;

        //tim target gan nhat
        GameObject target = null;
        float minDistance = Mathf.Infinity;
        foreach (var t  in targets)
        {
            var distance = Vector3.Distance(t.transform.position, transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                target = t;
            }
        }
        if(targets != null)
            agent.SetDestination(target.transform.position);
    }
}
