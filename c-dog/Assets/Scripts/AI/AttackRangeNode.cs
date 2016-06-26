using UnityEngine;
using System.Collections;

class AttackRange : Node
{
    public override bool Invoke()
    {
        Debug.Log("attack range");
        return true;
    }
}
