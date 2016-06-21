using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

class Base : MonoBehaviour
{
    public int myInt;
}

class Derived : Base
{
    public string myString;
}

class Node : MonoBehaviour
{
    public Base _myBase;

    void Reset()
    {
        #if UNITY_EDITOR
        // Delay this call so it will happen after copy serialization
        UnityEditor.EditorApplication.delayCall += DoInstancing;
        #endif
    }

    void DoInstancing()
    {
        var oldObject = _myBase;

        // Create the new monobehaviour of the correct type
        System.Type polymorphicType = oldObject != null ? oldObject.GetType() : typeof(Derived);
        var newObject = gameObject.AddComponent( polymorphicType ) as Base;

        // Copy over the data from the old object to the new one
        if ( oldObject != null )
            EditorUtility.CopySerialized( oldObject, newObject );

        _myBase = newObject;
        _myBase.hideFlags = HideFlags.HideInInspector;
    }
}

public class BaseDataProcessor : UnityEditor.AssetModificationProcessor
{
    /**
    * When we're about to save an asset, make sure that asset does not contain bad data
    */
    public static void OnWillSaveAssets( string[] assets )
    {
        foreach( string asset in assets )
        {
            // Check prefabs
            if ( asset.EndsWith(".prefab") )
            {               
                Object[] allObjs = AssetDatabase.LoadAllAssetsAtPath( asset );
                CheckList( allObjs );
            }
            else if ( asset.EndsWith(".unity") )
            {
                // If we're saving a level file, assume it's the current one... so process the current scene.
                Base[] allObjs = GameObject.FindSceneObjectsOfType( typeof(Base) ) as Base[];
                CheckList( allObjs );
            }
        }
    }

    /**
     * Check an object list for bad data
     */
    private static void CheckList( IEnumerable  dataObjs )
    {
        foreach( var obj in dataObjs )
        {
            if ( obj == null )
                GameObject.DestroyImmediate((GameObject)obj);
        }
    }
}


[System.Serializable]
public class TestNode : MonoBehaviour
{
    public virtual bool Invoke()
    {
        return true;
    }
};

class CompositeTestNode : TestNode
{
    public void AddChild(TestNode node)
    {
        children.Add(node);
    }

    public List<TestNode> GetChildren()
    {
        return children;
    }

    public List<TestNode> children = new List<TestNode>();
};

class Selector : CompositeTestNode
{
    public override bool Invoke()
    {
        foreach(TestNode node in GetChildren())
        {
            if (node.Invoke())
                return true;
        }
        return false;
    }
};

class Sequence : CompositeTestNode
{
    public override bool Invoke()
    {
        foreach(TestNode node in GetChildren())
        {
            if (!node.Invoke())
                return false;
        }
        return true;
    }
};
