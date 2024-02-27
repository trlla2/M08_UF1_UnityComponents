using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AutoDestroy : MonoBehaviour
{
    public float delay;
    private void Start()
    {

        StartCoroutine(DestroyDelay());
    }

    IEnumerator DestroyDelay()
    {
        float time = 0;
        while (time <= delay) 
        {
            time += Time.deltaTime;
            yield return null;

        }
        Addressables.Release(gameObject);
        Destroy(gameObject);
    }
}
