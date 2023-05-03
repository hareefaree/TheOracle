using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_Draggable : MonoBehaviour
{
    public delegate void DragEndDelegate(G_Draggable draggableObjects);

    public DragEndDelegate dragEndedCallback;

    private bool IsDragged = false;
    private Vector3 mouseDragStartPosition;
    private Vector3 spriteDragStartPosiion;
    private void OnMouseDown()
    {
        IsDragged = true;
        mouseDragStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteDragStartPosiion = transform.localPosition;
    }

    private void OnMouseDrag()
    {
        if(IsDragged) 
        {
            transform.localPosition = spriteDragStartPosiion + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseDragStartPosition);
        }
    }
    private void OnMouseUp()
    {
        IsDragged = false;
        dragEndedCallback(this);
      
    }

   
   
}
