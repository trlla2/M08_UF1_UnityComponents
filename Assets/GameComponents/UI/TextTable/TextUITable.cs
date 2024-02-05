using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextUITable : UITable<TextUICell>, IDropHandler
{
    [SerializeField] private List<string> _allTexts = new(); 
    public override int TotalCellsCount => _allTexts.Count;

   

    public override void SetupCell(TextUICell cell)
    {
        cell.label.text = cell.index.ToString() + " " + _allTexts[cell.index];
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Object droped over: " + name);

        GameObject droped = eventData.pointerDrag;
        if(droped.TryGetComponent(out TextUICell cell))
        {
            cell._targetParent = _scrollRect.content;
        }
    }
}

