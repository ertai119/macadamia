using UnityEngine;
using System.Collections;

class MoveNode : Node {

    public override bool Invoke()
    {
        Debug.Log("move node Invoke");
        return true;
    }
}
