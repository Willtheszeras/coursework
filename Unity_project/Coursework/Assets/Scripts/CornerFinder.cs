using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using System;

public class CornerFinder : MonoBehaviour
{
    public List<coordinate> points = new List<coordinate>();
    public int clusterSize = 11;
    public Regressions regret;


    StreamWriter writer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //writer = new StreamWriter("C:\\Users\\willt\\Documents\\Unity project (MAY BREAK)\\Coursework\\Assets\\varfuncbetter.txt");
    }

    // Update is called once per frame
    void Update()
    {

    }
    public List<int> Find(List<coordinate> points)
    {
        List<int> corners = new List<int>();
        //the first and last datapoints need to be marked as corners because I don't know how to close the seam
        corners.Add(0);

        //Create a list of the variance of the gradients
        Queue<float> previousGradients = new Queue<float>();
        List<float> varFunc = new List<float>();
        List<float> gradients = new List<float>();
        for (int i = 0; i < clusterSize; i++)
        {
            varFunc.Add(0);
        }
        for (int i = 0; i < points.Count; i++)
        {
            List<coordinate> cluster = new();

            if (i + clusterSize >= points.Count)
            {
                for (int j = 0; j < clusterSize; j++)
                {
                    cluster.Add(points[(i + j) % points.Count]);
                }
            }
            else
            {
                cluster = points.GetRange(i, clusterSize);
            }

            List<Vector3> ends = regret.calc(cluster, new List<Vector3>());
            //Using angle instead of gradient helps rationalise big values
            float m = Mathf.Atan(ends[1].z - ends[0].z) / (ends[1].x - ends[0].x);
            gradients.Add(m);
            //Debug.Log(m);
            previousGradients.Enqueue(m);
            if (previousGradients.Count >= 25)
            {
                float ms = 0;
                for (int j = 0; j < previousGradients.Count; j++)
                {
                    ms += Mathf.Pow(previousGradients.ToArray()[j], 2);
                }
                ms /= previousGradients.Count;
                float sm = Mathf.Pow(previousGradients.ToArray().Average(), 2);
                float variance = ms - sm;
                //Debug.Log(variance);
                varFunc.Add(variance);
                previousGradients.Dequeue();
            }

        }
        //Debug.Log("safe");
        //account for the first 25 datapoints
        for (int i = 0; i < clusterSize; i++)
        {
            List<coordinate> cluster = new();

            for (int j = 0; j < clusterSize; j++)
            {
                cluster.Add(points[(i + j) % points.Count]);
            }
            List<Vector3> ends = regret.calc(cluster, new List<Vector3>());
            //Using angle instead of gradient helps rationalise big values
            float m = Mathf.Atan(ends[1].z - ends[0].z) / (ends[1].x - ends[0].x);
            previousGradients.Enqueue(m);
            float ms = 0;
            for (int j = 0; j < previousGradients.Count; j++)
            {
                ms += Mathf.Pow(previousGradients.ToArray()[j], 2);
            }
            ms /= previousGradients.Count;
            float sm = Mathf.Pow(previousGradients.ToArray().Average(), 2);
            float variance = ms - sm;
            //Debug.Log(variance);
            varFunc[i] = variance;
            previousGradients.Dequeue();
        }
        //Find the significant spikes in variance
        //Debug.Log(varFunc[i]);
        //using (writer = new StreamWriter("C:\\Users\\willt\\Documents\\Unity project (MAY BREAK)\\Coursework\\Assets\\varfuncbetter.csv"))
        //{
        //    for (int i = 0; i < varFunc.Count; i++)
        //    {
        //        writer.WriteLine(i + ", " + varFunc[i] + ", " + gradients[i]/* + ", " + Mathf.Atan(gradients[i])*/);
        //    }
        //}

        ////Break the list into chunks above 1
        //List<List<Tuple<float, int>>> peaks = new List<List<Tuple<float, int>>>();
        //bool open = false;
        //int active = -1;
        //float threshold = 0.5f;
        //for (int i = 0; i < varFunc.Count; i++)
        //{
        //    if (open)
        //    {
        //        if (varFunc[i] < threshold)
        //        {
        //            open = false;
        //        }
        //        else
        //        {
        //            peaks[active].Add(new Tuple<float, int>(varFunc[i], i));
        //        }
        //    }
        //    else
        //    {
        //        if (varFunc[i] >= threshold)
        //        {
        //            open = true;
        //            peaks.Add(new List<Tuple<float, int>> { new Tuple<float, int>(varFunc[i], i) });
        //            active++;
        //        }
        //    }
        //}
        ////Find the largest item in the chunk
        //foreach (List<Tuple<float, int>> k in peaks)
        //{
        //    Tuple<float, int> best = new Tuple<float, int>(0, -1);
        //    for (int i = 0; i < k.Count; i++)
        //    {
        //        if (k[i].Item1 > best.Item1)
        //        {
        //            best = k[i];
        //        }
        //    }
        //    corners.Add(best.Item2);
        //}

        ////Add a point if it is more than 101% of its neighbors
        //for (int i = 2; i < varFunc.Count(); i++)
        //{
        //    float prev2 = varFunc[(i - 2) % varFunc.Count];
        //    float prev = varFunc[i - 1];
        //    float next = varFunc[(i + 1) % varFunc.Count];
        //    float next2 = varFunc[(i + 2) % varFunc.Count];
        //    if (varFunc[i] / prev >= 1.04f && varFunc[i] / next >= 1.04f && prev / prev2 >= 1.02f && next / next2 >= 1.02f)
        //    {
        //        corners.Add(i);
        //    }
        //}

        //More OLS blimey
        for (int j = 5; j < varFunc.Count - 5; j++)
        {
            List<float> sectionA = varFunc.GetRange(j - 5, 5);
            List<float> sectionB = varFunc.GetRange(j, 5);
            float up = 0;
            float down = 0;
            //TODO:FIX
            for (int i = 0; i < sectionA.Count; i++)
            {
                up += (i - 7.5f) * (varFunc[i] - sectionA.Average());
                down += Mathf.Pow((i - 7.5f), 2);
            }
            float mA = up / down;


            for (int i = 0; i < sectionA.Count; i++)
            {
                up += (i - 7.5f) * (varFunc[i] - sectionB.Average());
                down += Mathf.Pow((i - 7.5f), 2);
            }
            float mB = up / down;

            if (mA >= 0 && mB <= -0)
            {
                corners.Add(j);
            }

        }



        corners.Add(points.Count - 1);
        return corners;
    }
}
