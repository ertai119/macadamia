﻿using UnityEngine;
using System.Collections;

public class PoolObject : MonoBehaviour
{

    public virtual void OnObjectReuse()
    {

    }

    public virtual void Destroy()
    {
        gameObject.SetActive(false);
    }

}
