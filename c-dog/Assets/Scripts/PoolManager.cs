using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    static PoolManager _instance;

    public static PoolManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " pool");
        poolHolder.transform.parent = transform;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObj = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObj);
                newObj.SetParent(poolHolder.transform);
            }
        }
    }

    public GameObject ReuseObject(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objToReuse);

            objToReuse.Reuse(pos, rot);

            return objToReuse.gameObject;
        }

        return null;
    }

    public class ObjectInstance
    {
        public GameObject gameObject;
        public Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 pos, Quaternion rot)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = pos;
            gameObject.transform.rotation = rot;

            if (hasPoolObjectComponent)
            {
                poolObjectScript.OnObjectReuse();
            }
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}
