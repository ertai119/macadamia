
using UnityEngine;
using System.Collections;

public class testClass : MonoBehaviour
{
    public baseClass[] baseClassArray;// = new baseClass[1];
    void Start ()
    {
        //1. run and check the debug log in unity ... you'd see
        // derived class foo 

        //2. comment below two lines and UNCOMMENT AS MENTIONED STEP 3 AT 'CUSTOMINSPECTOR.CS ... LINE 19
        //baseClassArray = new baseClass[1];
        //baseClassArray[0] = new derivedClass();

        //3. now run again and check the output log in unity ... you'd see
        // base class foo 

        //baseClassArray[0].foo();

    }

}

//[System.Serializable] //**Commenting this out will give you your expected behavior.
public class baseClass
{
    public baseClass()
    {

    }

    public virtual void foo()
    {
        Debug.Log("base class foo");
    }
}

[System.Serializable]
public class derivedClass : baseClass
{
    public derivedClass()
    {

    }

    public override void foo()
    {
        Debug.Log("derived class foo");
    }
}
