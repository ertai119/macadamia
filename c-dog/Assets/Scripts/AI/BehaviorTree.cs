using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Node))]
public class BehaviorTree : MonoBehaviour {

    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {
    }

    public void StartAI()
    {
        isActivate = true;
        StartCoroutine("RunAI");
    }

    public void StopAI()
    {
        isActivate = false;
        StopCoroutine("RunAI");
    }

    IEnumerator RunAI()
    {
        if (isActivate)
        {
            while (!root.Invoke())
            {
                Debug.Log("Run AI");
                yield return new WaitForSeconds(.5f);

            }
        }
    }

    Selector root = new Selector();
    bool isActivate;
}
