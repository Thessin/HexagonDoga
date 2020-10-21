using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchSystem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public Camera cam;
    private Image img;

    private Vector2 dragStartVec = Vector2.zero;
    private Vector2 dragLastVec = Vector2.zero;

    private float totalDegreesTurned = 0.0f;
    private bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    /// <summary>
    /// Called when dragging begun. To set the initial datas.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartVec = eventData.position;
        dragLastVec = dragStartVec;

        dragging = true;
    }

    /// <summary>
    /// Called when a drag is happening. To calculate dragging rotation.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragCurrentVec = eventData.position - dragStartVec;

        float adding = Mathf.Atan2(dragLastVec.x * dragCurrentVec.y - dragLastVec.y * dragCurrentVec.x, dragLastVec.x * dragCurrentVec.x + dragLastVec.y * dragCurrentVec.y) * Mathf.Rad2Deg;

        totalDegreesTurned += adding;

        dragLastVec = dragCurrentVec;
    }

    /// <summary>
    /// Called on the end of a drag. To reset the datas.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        CalculateClockwiseRotation();

        dragStartVec = Vector2.zero;
        dragLastVec = Vector2.zero;

        totalDegreesTurned = 0.0f;
    }

    /// <summary>
    /// Called on pointer is clicked. For selecting a HexagonGroup.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (dragging)
        {
            dragging = false;
            return;
        }

        GridSystem.Instance.SelectGroup(cam.ScreenToWorldPoint(new Vector3(eventData.pressPosition.x, eventData.pressPosition.y, -cam.transform.position.z)));
    }

    /// <summary>
    /// Calculates the dragging rotation.
    /// </summary>
    private void CalculateClockwiseRotation()
    {
        if (totalDegreesTurned > 0)
            GridSystem.Instance.TurnSelected(false);
        else if (totalDegreesTurned < 0)
            GridSystem.Instance.TurnSelected(true);
    }
}
