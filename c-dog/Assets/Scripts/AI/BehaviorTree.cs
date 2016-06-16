using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Node))]
public class BehaviorTree : MonoBehaviour {

    private GameObject owner;

    void Awake()
    {
        owner = this.gameObject;
    }

    void Start ()
    {
        Sequence searchTargetSq = new Sequence();
        searchTargetSq.AddChild(new SearchNode());
        searchTargetSq.AddChild(new MoveNode());

        Sequence attack = new Sequence();
        attack.AddChild(new AttackRange());
        attack.AddChild(new AttackMelee());


        Selector aggresive = new Selector();
        aggresive.AddChild(searchTargetSq);
        aggresive.AddChild(attack);

        root.AddChild(aggresive);
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
