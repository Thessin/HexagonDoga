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
    private Vector2 dragEndVec = Vector2.zero;

    private float totalDegreesTurned = 0.0f;
    private bool dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("on begin drag");
        dragStartVec = eventData.position;
        dragLastVec = dragStartVec;

        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("on drag");
        Vector2 dragCurrentVec = eventData.position - dragStartVec;

        float adding = Mathf.Atan2(dragLastVec.x * dragCurrentVec.y - dragLastVec.y * dragCurrentVec.x, dragLastVec.x * dragCurrentVec.x + dragLastVec.y * dragCurrentVec.y) * Mathf.Rad2Deg;

        totalDegreesTurned += adding;

        dragLastVec = dragCurrentVec;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("on end drag");
        CalculateClockwiseRotation();

        dragStartVec = Vector2.zero;
        dragLastVec = Vector2.zero;
        dragEndVec = Vector2.zero;

        totalDegreesTurned = 0.0f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("press pos is " + eventData.pressPosition);
        Debug.Log("camera is " + cam.ScreenToWorldPoint(eventData.pressPosition));

        if (dragging)
        {
            dragging = false;
            return;
        }

        GridCreator.Instance.SelectGroup(cam.ScreenToWorldPoint(new Vector3(eventData.pressPosition.x, eventData.pressPosition.y, -cam.transform.position.z)));
    }

    private void CalculateClockwiseRotation()
    {
        if (totalDegreesTurned > 0)
            GridCreator.Instance.TurnSelected(false);
        else if (totalDegreesTurned < 0)
            GridCreator.Instance.TurnSelected(true);
    }
}
