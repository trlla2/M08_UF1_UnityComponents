using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropZoneUI : MonoBehaviour, IDropHandler
{

    [Header("Setup")]
    [SerializeField] private RectTransform _parentRectTransform;

    [Header("Events")]
    public UnityEvent<GameObject> OnObjectDrop;
    public UnityEvent<GameObject> OnObjectGrabbed;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Object droped over: " + name);

        GameObject droped = eventData.pointerDrag;
        if (droped.TryGetComponent(out DragCellUI cell))
        {
            cell._targetParent = _parentRectTransform;
        
            OnObjectDrop.Invoke(droped);
        }
    }

    public void OnGrabbed(GameObject Grabbed)
    {
        OnObjectGrabbed.Invoke(Grabbed);
    }
}
