using UnityEngine;
using System.Collections;

class MoveNode : Node
{
    private GameObject owner;

    public MoveNode(GameObject obj)
    {
        owner = obj;
    }

    public override bool Invoke()
    {
        Debug.Log("move node Invoke [Owner : " + owner + "]");
        return true;
    }
}
