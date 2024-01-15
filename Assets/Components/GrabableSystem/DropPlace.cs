using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DropPlace : MonoBehaviour
{
    [System.Flags]
    public enum CheckMode
    {
        CheckObject = 1, //000001
        CheckGrabbableType = 2, //000010
    }

    
    [Header("Setup")]
    [SerializeField] private List<Grabbable> _validGrabbables = new();
    [SerializeField] private Grabbable.GrabbableType _validGrabbableType;
    [SerializeField] private CheckMode _checkMode;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDropped;
    public UnityEvent<GameObject> OnObjectGrabbed;
    
    public bool IsValid(Grabbable grabbable)
    {
        bool isValid = true;

        if (_checkMode.HasFlag(CheckMode.CheckObject))
        {
            if(_validGrabbables.Count != 0 && _validGrabbables.Contains(grabbable)) 
            {
                isValid = false;
            }
        }

        if (_checkMode.HasFlag(CheckMode.CheckGrabbableType))
        {
            if (_validGrabbableType.HasFlag(grabbable.grabbableType))
            {
                isValid = false;
            }
        }

        return isValid;
    }

    public void OnDrop(Grabbable grabbable)
    {
        OnObjectDropped.Invoke(grabbable.gameObject);
        grabbable.OnStartGrab.AddListener(OnGrab);
    }

    private void OnGrab(GameObject grabbableObject, GameObject parent)
    {
        if(grabbableObject.TryGetComponent(out Grabbable grabbable))
        {
            grabbable.OnStartGrab.RemoveListener(OnGrab);
        }

        OnObjectGrabbed.Invoke(grabbable.gameObject);
    }

    
}
