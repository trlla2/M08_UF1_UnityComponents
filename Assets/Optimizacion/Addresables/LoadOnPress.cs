using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnPress : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    public void OnPress()
    {
        Instantiate(prefab);
    }

    
}
