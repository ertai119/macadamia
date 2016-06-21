
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(testClass))]
public class customInspector : Editor
{
    private testClass obj;
    private SerializedObject _object;


    void OnEnable()
    {
        _object = new SerializedObject(target);
        obj = (testClass)target;
    }

    //3. uncomment the below function fully ................... from line 20 to line 31
    public override void OnInspectorGUI()
    {
        _object.Update();

        if (obj.baseClassArray == null || obj.baseClassArray.Length == 0)
        {
            obj.baseClassArray = new baseClass[1];
            obj.baseClassArray[0] = new derivedClass();
            _object.ApplyModifiedProperties();
        }

        obj.baseClassArray[0].foo();

        DrawDefaultInspector();
    }

}