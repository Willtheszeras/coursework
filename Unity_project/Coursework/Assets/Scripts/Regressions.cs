using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using System;

public class Regressions : MonoBehaviour
{
    float[] x;
    float[] z;

    //public LineRenderer lr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    public List<Vector3> calc(List<coordinate> coords, List<Vector3> ends)
    {
        //bad implementation of OLS for regression
        
        x = (from coord in coords
            select coord.cartcoord.Item1).ToArray();
        float x_ = x.Average();
        z = (from coord in coords
             select coord.cartcoord.Item2).ToArray();
        float z_ = z.Average();

        float up = 0;
        float down = 0;

        //TODO:FIX
        for (int i = 0; i < x.Length; i++)
        {
            up += (x[i] - x_) * (z[i] - z_);
            down += Mathf.Pow((x[i] - x_), 2);
        }


        float m = up / down;

        float c = z_ - m * x_;
        Vector3 start = new Vector3(x[0], 0, x[0]*m + c);
        Vector3 end = new Vector3(x[x.Length - 1], 0, x[x.Length - 1] * m + c);

        

        ends.Add(start);
        ends.Add(end);
        return ends;

        //lr.SetPosition(0, start);
        //lr.SetPosition(1, end);
    }
}
