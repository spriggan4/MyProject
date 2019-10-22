using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour , IDragHandler , IBeginDragHandler , IEndDragHandler {
    private Transform tr = null;
    private Vector3 posTemp = Vector3.zero;
    private void Start()
    {
        tr = this.transform;
        posTemp = tr.position;
    }
    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { tr.position = eventData.position; }
    public void OnEndDrag(PointerEventData eventData) { tr.position = posTemp; }

}
