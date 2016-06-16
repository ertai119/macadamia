using UnityEngine;
using System.Collections;

class AttackMelee : Node 
{
    public override bool Invoke()
    {
        Debug.Log("attack melee");
        return true;
    }
}
