using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//[RequireComponent(typeof(Rigidbody))]
public class Detector : MonoBehaviour
{
    [Header("Objects Setup")]
    [SerializeField, Min(1)] private uint _requiredAmountOfObjects = 1;
    [SerializeField] private List<GameObject> _objectsInside = new();

    [Header("Events")]
    public UnityEvent OnActivated;
    public UnityEvent OnDeactivate;
 
    private void OnTriggerEnter(Collider other)
    {
        _objectsInside.Add(other.gameObject);

        if(_objectsInside.Count == _requiredAmountOfObjects){
            OnActivated.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _objectsInside.Remove(other.gameObject);

        if (_objectsInside.Count == _requiredAmountOfObjects -1)
        {
            OnDeactivate.Invoke();
        }
    }
}
