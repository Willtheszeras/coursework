using UnityEngine;
using System.Collections.Generic;
using System;

public class Translator : MonoBehaviour
{
    public int[] packet = new int[47] { 54, 44, 23, 14, 19, 8, 103, 0, 241, 101, 0, 240, 100, 0, 243, 99, 0, 243, 99, 0, 244, 108, 0, 243, 103, 0, 243, 110, 0, 243, 114, 0, 244, 116, 0, 243, 107, 0, 242, 112, 0, 243, 148, 11, 73, 104, 59 };

    float startAngleDegrees;
    float endAngleDegrees;
    float startAngle;
    float endAngle;
    float[] measurements;
    float step;
    public Tuple<float, float>[] coordinates;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void translate()
    {
        // See https://aka.ms/new-console-template for more information
        //Debug.Log("Hello, World!");

        startAngleDegrees = LittleEndian(packet[4], packet[5]) * 0.01f;
        endAngleDegrees = LittleEndian(packet[42], packet[43]) * 0.01f;

        //If the angle goes over the "seam" between 180 and -180, loop round
        if (startAngleDegrees > 180 && endAngleDegrees < 180)
        {
            endAngleDegrees += 360;
        }

        startAngle = startAngleDegrees * (Mathf.PI / 180);
        endAngle = endAngleDegrees * (Mathf.PI / 180);

        measurements = new float[12] { LittleEndian(packet[6], packet[7]), LittleEndian(packet[9], packet[10]), LittleEndian(packet[12], packet[13]), LittleEndian(packet[15], packet[16]), LittleEndian(packet[18], packet[19]), LittleEndian(packet[21], packet[22]), LittleEndian(packet[24], packet[25]), LittleEndian(packet[27], packet[28]), LittleEndian(packet[30], packet[31]), LittleEndian(packet[33], packet[34]), LittleEndian(packet[36], packet[37]), LittleEndian(packet[39], packet[40]) };

        step = (endAngle - startAngle) / 11;


        coordinates = new Tuple<float, float>[12];
        for (int i = 0; i < measurements.Length; i++)
        {
            coordinates[i] = new Tuple<float, float>(measurements[i], -(startAngle + (step * i)));
        }

        //for (int i = 0; i < coordinates.Length; i++)
        //{
        //    Debug.Log(coordinates[i]);
        //}
    }

    //Helps deal with the weird formatting
    float LittleEndian(float a, float b)
    {
        return (b * 256f) + a;
    }
}
