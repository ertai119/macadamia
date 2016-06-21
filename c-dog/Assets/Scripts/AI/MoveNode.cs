using UnityEngine;
using System.Collections;

class MoveNode : TestNode {

    public override bool Invoke()
    {
        Debug.Log("move node Invoke");
        return true;
    }
}
