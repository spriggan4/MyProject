using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugDraggable : MonoBehaviour , IDragHandler, IBeginDragHandler {
    private Transform tr = null;
    private void Start()
    {
        tr = this.transform;
    }
    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { tr.position = eventData.position; }
}
