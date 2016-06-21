using UnityEngine;
using System.Collections;

class AttackRange : TestNode
{
    public override bool Invoke()
    {
        Debug.Log("attack range");
        return true;
    }
}
