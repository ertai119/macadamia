using UnityEngine;
using System.Collections;

class SearchNode : Node
{
    public override bool Invoke()
    {
        Debug.Log("search node invoke");
        return true;
    }
}
