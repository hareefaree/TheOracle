using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G_SnapController : MonoBehaviour
{
    public List<Transform> snapPoints;
    public List<G_Draggable> draggableObjects;
    public float snapRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        foreach (G_Draggable draggable in draggableObjects) 
        {
            draggable.dragEndedCallback = onDragEnded;

        }
    }

  private void onDragEnded(G_Draggable draggable) 
    {
        float closestDistance = -1;
        Transform closestSnapPoint = null;

        foreach(Transform snapPoint in snapPoints) 
        {
            float currentDistance = Vector2.Distance(draggable.transform.localPosition, snapPoint.localPosition);
            if(closestSnapPoint == null || currentDistance < closestDistance) 
            {
                closestSnapPoint = snapPoint;
                closestDistance = currentDistance;
            }
        }

        if (closestSnapPoint != null && closestDistance <= snapRange) 
        {
            draggable.transform.localPosition = closestSnapPoint.localPosition;
        }
    }
}
