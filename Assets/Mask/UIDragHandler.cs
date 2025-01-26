using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    
    // We'll use this to store the offset between the mouse pointer
    // and the object's pivot at the beginning of the drag.
    private Vector2 offset;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        // Grab a reference to the parent Canvas.
        // If this script is placed on a UI element nested in a canvas,
        // just find the nearest parent canvas.
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Convert the mouse position to the local point in the RectTransform,
        // and store the difference between that and the current anchoredPosition.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPosition
        );

        // This offset tells us how far the user's mouse is from the object's pivot
        offset = rectTransform.anchoredPosition - localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // As the mouse moves, convert the new mouse position to a local point
        // and apply our offset so the object continues to track under the pointer.
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out var localPointerPosition
        );

        rectTransform.anchoredPosition = localPointerPosition + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Called when dragging ends.
        // You could do things like snap the object to a position or
        // check bounds/clamping here, if desired.
    }
}
