using UnityEngine;
using System.Collections;

class SearchNode : TestNode
{
    public override bool Invoke()
    {
        Debug.Log("search node invoke");
        return true;
    }
}
