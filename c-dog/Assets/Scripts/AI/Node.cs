using System.Collections;
using System.Collections.Generic;

class Node
{
    public virtual bool Invoke()
    {
        return true;
    }
};

class CompositeNode : Node
{
    public void AddChild(Node node)
    {
        children.Add(node);
    }

    public List<Node> GetChildren()
    {
        return children;
    }

    List<Node> children = new List<Node>();
};

class Selector : CompositeNode
{
    public override bool Invoke()
    {
        foreach(Node node in GetChildren())
        {
            if (node.Invoke())
                return true;
        }
        return false;
    }
};

class Sequence : CompositeNode
{
    public override bool Invoke()
    {
        foreach(Node node in GetChildren())
        {
            if (!node.Invoke())
                return false;
        }
        return true;
    }
};
