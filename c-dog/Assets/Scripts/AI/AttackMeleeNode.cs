using UnityEngine;
using System.Collections;

class AttackMelee : TestNode 
{
    public override bool Invoke()
    {
        Debug.Log("attack melee");
        return true;
    }
}
