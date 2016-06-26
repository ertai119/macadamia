using UnityEngine;
using System.Collections;

class SearchNode : Node
{
    private GameObject owner;
    private Transform playerPos;

    public SearchNode(GameObject obj)
    {
        owner = obj;
    }

    public override bool Invoke()
    {
        Debug.Log("search node invoke owner : " + owner);

        playerPos = GameObject.FindGameObjectWithTag ("Player").transform;

        float dist = (playerPos.position - owner.transform.position).sqrMagnitude;
        if (dist < 10)
            return true;

        return false;
    }
}
