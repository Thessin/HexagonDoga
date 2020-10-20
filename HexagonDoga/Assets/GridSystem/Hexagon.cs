using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon
{
    private Vector3 position = Vector3.zero;
    private List<HexagonGroup> surroundingHexagonGroups = new List<HexagonGroup>();

    private int x, y;   // X,Y locations of the this hexagon on the grid.

    public GameObject obj;
    public Color color;

    private bool destroyed = false;

    public Hexagon(Vector3 position, List<HexagonGroup> surroundingHexagonGroups, int x, int y)
    {
        this.position = position;
        this.surroundingHexagonGroups = surroundingHexagonGroups;
        this.x = x;
        this.y = y;
    }

    public void SetGameObject(GameObject obj)
    {
        destroyed = false;

        this.obj = obj;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Transform GetTransform()
    {
        return obj.transform;
    }

    public void ExchangeData(GameObject obj, Color color)
    {
        this.obj = obj;
        this.color = color;

        Debug.Log("moving from " + obj.transform.position + " to " + position);

        // TODO: Animate this with DOTween.
        obj.transform.position = position;
    }

    public bool IsDestroyed()
    {
        return destroyed;
    }

    public void DestroyHexagon()
    {
        destroyed = true;

        Debug.LogWarning("DESTROYING HEXAGON ON " + position + " of the grid:" + x + "," + y);
        GameObject.Destroy(obj);
        // TODO: Add 5 points to scoreboard.
        // TODO: Play animation for hexagon gameobject when DOTween is added.
    }

    public void RegisterToHexagonGroup()
    {
        foreach (var group in surroundingHexagonGroups)
        {
            group.AddHexagon(this);
        }
    }

    public bool IsGroupDestroyable()
    {
        bool destroyable = false;

        foreach (var group in surroundingHexagonGroups)
        {
            if (group.IsExplodable())
            {
                destroyable = true;
                group.DestroyGroup();
                break;
            }
        }

        return destroyable;
    }
}
