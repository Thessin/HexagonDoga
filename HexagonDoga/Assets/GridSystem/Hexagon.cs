using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hexagon
{
    private Vector3 position = Vector3.zero;
    private List<HexagonGroup> surroundingHexagonGroups = new List<HexagonGroup>();

    private GameObject obj;
    public Color color;

    private bool destroyed = false;

    public Hexagon(Vector3 position, List<HexagonGroup> surroundingHexagonGroups)
    {
        this.position = position;
        this.surroundingHexagonGroups = surroundingHexagonGroups;
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

    public void DestroyHexagon()
    {
        destroyed = true;

        // TODO: Play animation for hexagon gameobject when DOTween is added.
    }

    public void RegisterToHexagonGroup()
    {
        foreach (var group in surroundingHexagonGroups)
        {
            group.AddHexagon(this);
        }
    }

    public void CheckDestroyable()
    {

    }
}
