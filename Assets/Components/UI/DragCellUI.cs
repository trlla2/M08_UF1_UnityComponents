using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[RequireComponent(typeof(CanvasGroup))]
public class DragCellUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rect;
    public Transform _targetParent;
    private CanvasGroup _canvasGroup;

    [Header("Events")]
    public UnityEvent<GameObject> OnStartGrab;
    public UnityEvent<GameObject> OnEndGrab;
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Start Drag");
        _canvasGroup.blocksRaycasts = false;
        _targetParent = _rect.parent;
        _rect.SetParent(GetComponentInParent<DragContainer>().Rect);
        this.gameObject.GetComponentInParent<DropZoneUI>().OnGrabbed(this.gameObject);//?
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On Drag");

        if (eventData.pointerEnter == null || eventData.pointerEnter.transform as RectTransform == null)
        {
            return;
        }

        RectTransform plane = eventData.pointerEnter.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(plane,
            eventData.position,
            eventData.pressEventCamera,
            out Vector3 globalMousePos))
        {
            _rect.position = globalMousePos;
            _rect.rotation = plane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag");
        _canvasGroup.blocksRaycasts = true;
        _rect.SetParent(_targetParent);
    }
}
