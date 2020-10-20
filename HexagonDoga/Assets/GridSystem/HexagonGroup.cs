using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HexagonGroup
{
    private Vector3 position = Vector3.zero;
    private List<Hexagon> hexagons = new List<Hexagon>();

    private int groupingNumber;     // This group's number that is on GridCreator script.

    public HexagonGroup(Vector3 position, int groupingNumber)
    {
        this.position = position;
        this.groupingNumber = groupingNumber;
    }

    /// <summary>
    /// Adds hexagon to this group.
    /// </summary>
    /// <param name="hex"></param>
    public void AddHexagon(Hexagon hex)
    {
        hexagons.Add(hex);
    }

    /// <summary>
    /// Get this group's middle point's position.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// Wrapper to rotate group.
    /// </summary>
    /// <param name="clockWise"></param>
    public void RotateGroup(bool clockWise)
    {
        Debug.LogWarning("ROTATE GROUP CALLED with clockwise" + clockWise + " and groupingNumber%2" + (groupingNumber % 2));

        if (groupingNumber % 2 == 0)
        {
            if (clockWise)
                MoveClockwise(true);
            else
                MoveCounterClockwise(true);
        }
        else
        {
            if (clockWise)
                MoveCounterClockwise(true);
            else
                MoveClockwise(true);
        }
    }

    /// <summary>
    /// Moves the group in clockwise rotation.
    /// </summary>
    /// <param name="checkForDestruction"></param>
    private void MoveClockwise(bool checkForDestruction)
    {
        Debug.LogWarning("clockwise with check" + checkForDestruction);

        GameObject obj = hexagons[hexagons.Count - 1].obj;
        Color col = hexagons[hexagons.Count - 1].color;
        Debug.Log(hexagons.Count - 1 + "'s color is" + col);

        bool checkedOnce = false;

        bool foundDestroyableGroup = false;
        TweenCallback destCheck = () => {
            if (checkForDestruction && !checkedOnce)
            {
                checkedOnce = true;

                foreach (var hex in hexagons)
                {
                    if (hex.IsGroupDestroyable())
                        foundDestroyableGroup = true;
                }

                if (!foundDestroyableGroup)
                    MoveCounterClockwise(false);
            }
        };

        for (int i = hexagons.Count - 1; i >= 0; i--)
        {
            Debug.Log("in clockWise " + i + " hexagon x:" + hexagons[i].x + " y:" + hexagons[i].y + " color: " + hexagons[i].color);

            if (i != 0)
                hexagons[i].ExchangeData(hexagons[i - 1].obj, hexagons[i - 1].color, destCheck);
            else
                hexagons[i].ExchangeData(obj, col, destCheck);
        }
    }

    /// <summary>
    /// Moves the group in counter clockwise rotation.
    /// </summary>
    /// <param name="checkForDestruction"></param>
    private void MoveCounterClockwise(bool checkForDestruction)
    {
        Debug.LogWarning("counterClockwise with check" + checkForDestruction);

        GameObject obj = hexagons[0].obj;
        Color col = hexagons[0].color;
        Debug.Log("0's color is" + col);

        bool checkedOnce = false;

        bool foundDestroyableGroup = false;
        TweenCallback destCheck = () => {
            if (checkForDestruction && !checkedOnce)
            {
                checkedOnce = true;

                foreach (var hex in hexagons)
                {
                    if (hex.IsGroupDestroyable())
                        foundDestroyableGroup = true;
                }

                if (!foundDestroyableGroup)
                    MoveClockwise(false);
            }
        };

        for (int i = 0; i < hexagons.Count; i++)
        {
            Debug.Log("in counterClockwise " + i + " hexagon x:" + hexagons[i].x + " y:" + hexagons[i].y + " color: " + hexagons[i].color);

            if (i != hexagons.Count - 1)
                hexagons[i].ExchangeData(hexagons[i+1].obj, hexagons[i+1].color, destCheck);
            else
                hexagons[i].ExchangeData(obj, col, destCheck);
        }

        
    }

    /// <summary>
    /// Returns true if this group is explodable/destroyable.
    /// </summary>
    /// <returns></returns>
    public bool IsExplodable()
    {
        if (hexagons[0].IsDestroyed() && hexagons[1].IsDestroyed() && hexagons[2].IsDestroyed())                                // If any one of the hexagons is destroyed, the group can not be explodable.
            return false;

        return hexagons[0].color.IsEqualColor(hexagons[1].color) && hexagons[0].color.IsEqualColor(hexagons[2].color);            // Check if all three hexagons' colors are equal.
    }

    /// <summary>
    /// Destroys this group. Destroying all the hexagons attached to this group.
    /// </summary>
    public void DestroyGroup()
    {
        foreach (var hex in hexagons)
        {
            hex.DestroyHexagon();
        }

        GridCreator.Instance.RemoveSelection();
        GridCreator.Instance.CheckIfMoveExists();
    }

    /// <summary>
    /// Returns transforms of all hexagons attached to this group.
    /// </summary>
    /// <returns></returns>
    public List<Transform> GetHexagonsTransformList()
    {
        List<Transform> posList = new List<Transform>();

        foreach (var hex in hexagons)
        {
            posList.Add(hex.GetTransform());
        }

        return posList;
    }
}
