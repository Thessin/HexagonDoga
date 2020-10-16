﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : Singleton<GridCreator>
{
    [SerializeField]
    private GameObject gridGO;

    private float hexagonX = 0.0f;
    private float hexagonY = 0.0f;

    [SerializeField] private int tileCountX = 8;
    [SerializeField] private int tileCountY = 9;
    [SerializeField] private float spacing = 0.0f;

    [SerializeField]
    private List<Color> colors;

    private List<Vector3> hexagonPositions = new List<Vector3>();
    private List<HexagonGroup> hexagonGroups = new List<HexagonGroup>();

    private List<Vector3> midPoints = new List<Vector3>();

    private void Start()
    {
        hexagonX = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
        hexagonY = gridGO.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int y = 0; y < tileCountY; y++)
        {
            for (int x = 0; x < tileCountX; x++)
            {
                float xPos = x * 1.5f * hexagonX + (spacing * x);
                float yPos = ((x % 2) * (hexagonY + (spacing / 2))) + (-2 * y * hexagonY) - (spacing * y);

                hexagonPositions.Add(new Vector3(xPos, yPos, 0.0f));        // For actual hexagon positions


                if (x < tileCountX - 1 && y < tileCountY - 1)               // For grouping points
                {
                    //hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (1.0f - (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f), 0.0f)));
                    //hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f)));

                    midPoints.Add(new Vector3(xPos + (hexagonX * (1.0f - (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f), 0.0f));
                    midPoints.Add(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f));
                }
            }

            //for (int x = 0; x < tileCountX; x++)
            //{
            //    float xPos = x * 1.5f * hexagonX + (spacing * x);

            //    if (x < tileCountX - 1 && y < tileCountY - 1)
            //        midPoints.Add(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f));

            //}

            //for (int x = 0; x < tileCountX; x++)
            //{
            //    float xPos = x * 1.5f * hexagonX + (spacing * x);

            //    if (x < tileCountX - 1 && y < tileCountY - 1)
            //    {
            //hexagonGroups.Add(new HexagonGroup(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f)));
            //        midPoints.Add(new Vector3(xPos + (hexagonX * (0.5f + (x % 2) * 0.5f)), (-2 * y * hexagonY) - (spacing * y * 1.0f) - hexagonY, 0.0f));
            //    }
            //}

            //for (int x = 0; x < tileCountX; x++)
            //{
            //    float xPos = x * 1.5f * hexagonX + (spacing * x);
            //    float yPos = ((x % 2) * (hexagonY + (spacing / 2))) + (-2 * y * hexagonY) - (spacing * y);

            //    hexagonPositions.Add(new Vector3(xPos, yPos, 0.0f));        // For actual hexagon positions

            //    //int hexagonNumber = x + (y * tileCountY);
            //    //// Six points
            //    //switch (hexagonNumber % 2)
            //    //{
            //    //    case 0:
            //    //        if(hexagonNumber-(hexagonNumber%))
            //    //        hexagonGroups[hexagonNumber].AddHexagonNumber(hexagonNumber);
            //    //        break;
            //    //    case 1:
            //    //        break;
            //    //}


            //}
        }

        for (int i = 0; i < midPoints.Count; i++)
        {
            int midPointsOnXAxis = (tileCountX - 1) * 2;    // Total mid points on the x axis.
            int xAxisNum = i % midPointsOnXAxis;            // The number the index is on the current axis.
            int yAxisNum = i / midPointsOnXAxis;

            int a = (xAxisNum / 2) + (yAxisNum * tileCountX);
            int b = (yAxisNum * tileCountX) + (xAxisNum/2);    //(xAxisNum + (midPointsOnXAxis * yAxisNum)) - (xAxisNum / 2) - 1 - (yAxisNum * 2);


            switch (xAxisNum % 2)
            {
                case 0:
                    Debug.Log("xAxisNum: " + xAxisNum + " for " + i + " mid point, numbers are: " + a + "  " + (a + 1) + "  " + (a + 1 + tileCountX));
                    break;
                case 1:
                    Debug.Log("xAxisNum: " + xAxisNum + " for " + i + " mid point, numbers are: " + b + "  " + (b + tileCountX) + "  " + (b + tileCountX + 1));
                    break;
            }

            //switch ((xAxisNum / 2) % 2)
            //{
            //    case 0:
            //        switch (xAxisNum % 2)
            //        {
            //            case 0:
            //                Debug.Log("xAxisNum: " + xAxisNum + " for " + i + " mid point, numbers are: " + a + "  " + (a + 1) + "  " + (a + 1 + tileCountX));
            //                break;
            //            case 1:
            //                Debug.Log("xAxisNum: " + xAxisNum + " for " + i + " mid point, numbers are: " + b + "  " + (b+tileCountX) + "  " + (b + tileCountX+1));
            //                break;
            //        }
            //        break;
            //    case 1:
            //        break;
            //}
        }

        ShowGrid();
    }
    
    private void ShowGrid()
    {
        foreach (var pos in hexagonPositions)
        {
            SendHexagon(pos,1.0f);
        }

        // For debugging purposes, delete on prod
        foreach (var pos in midPoints)
        {
            SendHexagon(pos,0.15f);
        }


        FindGroup(new Vector3(1, 0, 0));
    }

    private void SendHexagon(Vector3 pos,float scale)
    {
        GameObject obj = Instantiate(gridGO);

        obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        if (colors.Count != 0)
            obj.GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Count)];
    }

    private void FindGroup(Vector3 clickPos)
    {
        Vector3 closestPoint = Vector3.zero;
        float smallestDistance = float.MaxValue;
        int midPointNumber = 0;

        List<int> selectedHexagonNumbers = new List<int>();

        for (int i = 0; i < midPoints.Count; i++)
        {
            float clickDist = Vector3.Distance(clickPos, midPoints[i]);
            if (smallestDistance > clickDist)
            {
                smallestDistance = clickDist;
                closestPoint = midPoints[i];
                midPointNumber = i;
            }
        }

        selectedHexagonNumbers.Add(midPointNumber / 2);
        selectedHexagonNumbers.Add((midPointNumber / (tileCountX - 1)) * tileCountX);

        foreach (var num in selectedHexagonNumbers)
        {
            Debug.Log("selectedHexNumber is " + num);
        }

        Debug.Log("closest groupPoint to " + clickPos + " is " + closestPoint + " with a distance of " + smallestDistance);
    }
}
