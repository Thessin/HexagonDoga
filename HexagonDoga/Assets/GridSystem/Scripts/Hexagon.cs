using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hexagon
{
    private Vector3 position = Vector3.zero;
    private List<HexagonGroup> surroundingHexagonGroups = new List<HexagonGroup>();

    public int x, y;   // X,Y locations of the this hexagon on the grid.

    public GameObject obj;
    public Color color;

    private float animationDuration = 0.5f;
    private int pointsToAdd = 5;        // Points to add when this instance is destroyed.

    private bool destroyed = false;

    public Hexagon(Vector3 position, List<HexagonGroup> surroundingHexagonGroups, int x, int y)
    {
        this.position = position;
        this.surroundingHexagonGroups = surroundingHexagonGroups;
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Set GameObject to manipulate for this instance.
    /// </summary>
    /// <param name="obj"></param>
    public void SetGameObject(GameObject obj)
    {
        destroyed = false;

        this.obj = obj;
    }

    /// <summary>
    /// Returns this instance's position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Returns this instance's gameobject's transform.
    /// </summary>
    /// <returns></returns>
    public Transform GetTransform()
    {
        return obj.transform;
    }

    /// <summary>
    /// Exchanges necessary data for moving between hexagons.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="color"></param>
    /// <param name="cb"></param>
    public void ExchangeData(GameObject obj, Color color, TweenCallback cb)
    {
        this.obj = obj;
        this.color = color;

        Debug.Log("moving from " + obj.transform.position + " to " + position);

        // TODO: Animate this with DOTween.
        obj.transform.DOMove(position, animationDuration).OnComplete(cb);
    }

    /// <summary>
    /// Returns the destroyed state of this hexagon.
    /// </summary>
    /// <returns></returns>
    public bool IsDestroyed()
    {
        return destroyed;
    }

    /// <summary>
    /// Destroys this hexagon.
    /// </summary>
    public void DestroyHexagon()
    {
        if (destroyed) return;

        destroyed = true;

        Debug.LogWarning("DESTROYING HEXAGON ON " + position + " of the grid x: " + x + ", y: " + y + " color: " + color);
        GameObject.Destroy(obj);

        ScoringSystem.Instance.AddScore(pointsToAdd);

        // TODO: Play animation for hexagon gameobject when DOTween is added.
    }

    /// <summary>
    /// Registers itself to the hexagon groups that this instance has relations with.
    /// </summary>
    public void RegisterToHexagonGroup()
    {
        foreach (var group in surroundingHexagonGroups)
        {
            group.AddHexagon(this);
        }
    }

    /// <summary>
    /// Returns true if any of this hexagon's attached groups is destroyable and destroys that group.
    /// </summary>
    /// <returns></returns>
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
