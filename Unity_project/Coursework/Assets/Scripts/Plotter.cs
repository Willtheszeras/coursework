using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
using TMPro.EditorUtilities;
using TMPro;
using System.Net.NetworkInformation;

public class Plotter : MonoBehaviour
{
    public GameObject point;
    public Translator translator;
    public float scale = 5f;
    public bool finishedReading = false;
    public Regressions regret;
    public CornerFinder cornerFinder;
    public LineRenderer lr;
    public Canvas worldCanvas;

    public GameObject cornerMarker;
    //int index = 0;

    public GameObject label;
    TextMeshPro labelText;

    public List<GameObject> objs = new List<GameObject>();
    public List<coordinate> points = new List<coordinate>();

    private void Start()
    {
        points = new();
        labelText = label.GetComponent<TextMeshPro>();
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Plot()
    {
        //translator = new Translator();
        translator.translate();
        for (int i = 0; i < translator.coordinates.Length; i++)
        {
            float x = (translator.coordinates[i].Item1 * Mathf.Cos(translator.coordinates[i].Item2)) / scale;
            float z = (translator.coordinates[i].Item1 * Mathf.Sin(translator.coordinates[i].Item2)) / scale;

            //Reject points that are too close to the origin -> 50cm deadzone
            if (translator.coordinates[i].Item1 >= 500)
            {
                points.Add(new coordinate()
                {
                    polarcoord = translator.coordinates[i],
                    cartcoord = new Tuple<float, float>(x, z)
                });
            }
            //Instantiate(point, new Vector3(points[i].cartcoord.Item1, 0, points[i].cartcoord.Item2), transform.rotation);
            //index = i;
            //if (index % 40000 == 4500) {
            //    onfinished();
            //}

            //ends = regret.calc(points, ends);


            //Instantiate(point, new Vector3(x, 0, z), transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onfinished()
    {

        points = points.OrderBy(coord => coord.polarcoord.Item2).ToList();
        for (int i = 0; i < points.Count; i++)
        {
                GameObject obj = Instantiate(point, new Vector3(points[i].cartcoord.Item1, 0, points[i].cartcoord.Item2), transform.rotation);
                objs.Add(obj);

        }




        List<int> corners = cornerFinder.Find(points);

        //Strip consecutive numbers, as they break the regression

        for (int i = 0; i < corners.Count - 1; i++)
        {
            if (corners[i+1] == corners[i] + 1)
            {
                corners.RemoveAt(i+1);
            }
        }

        List<Vector3> ends = new();
        for (int i = 0; i < corners.Count - 1; i++)
        {
            ends = regret.calc(points.GetRange(corners[i], corners[i+1] - corners[i]), ends);
        }
        lr.positionCount = ends.Count;
        for (int i = 0; i < ends.Count; i++)
        {
            //Debug.Log(ends[i]);
            lr.SetPosition(i, ends[i]);
            GameObject obj = Instantiate(cornerMarker, ends[i], transform.rotation);
            objs.Add(obj);
        }

        for (int i = 0; i < ends.Count - 1; i++)
        {
            float distance = Vector3.Distance(ends[i], ends[i + 1]) * scale;
            var text = Instantiate(label, new Vector3((ends[i].x + ends[i + 1].x)/2, 5, (ends[i].z + ends[i + 1].z) / 2), transform.rotation, worldCanvas.transform);
            text.GetComponent<TextMeshProUGUI>().text = distance.ToString() + "mm";
        }

    }

}


public record coordinate
{
    public Tuple<float, float> polarcoord;
    public Tuple<float, float> cartcoord;
}