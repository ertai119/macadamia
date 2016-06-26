using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController: MonoBehaviour
{
    Selector root = new Selector();
    bool isActivate = false;

    void Awake()
    {
        Selector search = new Selector();
        search.AddChild(new SearchNode(gameObject));
        search.AddChild(new MoveNode(gameObject));

        root.AddChild(search);
    }

    void Start()
    {
        
    }

    public void Activate()
    {
        isActivate = true;
        StartCoroutine(UpdateAI());
    }

    public void Deactivate()
    {
        isActivate = false;
        StopCoroutine(UpdateAI());
    }

    void Update()
    {
    }

    IEnumerator UpdateAI()
    {
        float refreshRate = .25f;

        while (isActivate && !root.Invoke())
        {
            yield return new WaitForSeconds(refreshRate);
        }
    }
}