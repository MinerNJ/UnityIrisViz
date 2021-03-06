﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointPlotter : MonoBehaviour
{
    public string inputFile;

    public GameObject DataPointPrefab;
    public GameObject DataPointHolder;

    private List<Dictionary<string, object>> dataPointList;

    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;

    public string xName;
    public string yName;
    public string zName;

    public float plotScale = 10;

    private void Start()
    {
        dataPointList = CSVReader.Read(inputFile);

        List<string> columnList = new List<string>(dataPointList[1].Keys);

        xName = columnList[columnX];
        yName = columnList[columnY];
        zName = columnList[columnZ];

        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);

        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);

        for (var i = 0; i < dataPointList.Count; i++)
        {
            float x = (System.Convert.ToSingle(dataPointList[i][xName]) - xMin) / (xMax - xMin);
            float y = (System.Convert.ToSingle(dataPointList[i][yName]) - yMin) / (yMax - yMin);
            float z = (System.Convert.ToSingle(dataPointList[i][zName]) - zMin) / (zMax - zMin);

            GameObject dataPoint = Instantiate(
                    DataPointPrefab,
                    new Vector3(x, y, z)*plotScale,
                    Quaternion.identity);

            dataPoint.transform.parent = DataPointHolder.transform;
            // dataPoint.transform.localScale += new Vector3(x, y, z);
            //scales the data points by their x, y and z variables.

            string dataPointName =
                dataPointList[i][xName] + " "
                + dataPointList[i][yName] + " "
                + dataPointList[i][zName];

            dataPoint.transform.name = dataPointName;

            dataPoint.GetComponent<Renderer>().material.color =
                new Color(x, y, z, 1.0f);

        }
    }

    private float FindMaxValue(string columnName)
    {
        float maxValue = Convert.ToSingle(dataPointList[0][columnName]);

        for(var i = 0; i < dataPointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(dataPointList[i][columnName])) 
                maxValue = Convert.ToSingle(dataPointList[i][columnName]);
        }

        return maxValue;
    }

    private float FindMinValue(string columnName)
    {
        float minValue = Convert.ToSingle(dataPointList[0][columnName]);

        for(var i = 0; i < dataPointList.Count; i++)
        {
            if (Convert.ToSingle(dataPointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(dataPointList[i][columnName]);
        }

        return minValue;
    }
}
