using UnityEngine;
using System.IO;
using System;

public class Reader : MonoBehaviour
{

    StreamReader reader = new StreamReader("C:\\Users\\willt\\Documents\\coursework_code\\Unity_project\\Coursework\\Assets\\Harvested_data3.txt");
    string raw;
    public Translator trans;
    public Plotter plt;
    public bool plot = false;
    int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raw = reader.ReadLine();
        while (raw != null)
        {
            string line = "";
            bool harvest = false;
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] == '\'')
                {
                    harvest = false;
                }
                if (harvest)
                {
                    line += raw[i];
                }
                if (raw[i] == 'b')
                {
                    harvest = true;
                }
            }
            //Debug.Log(line);
            int number = Convert.ToInt32(line, 2);
            //Debug.Log(number);

            if (number == 84)
            {
                plot = true;
            }

            if (plot)
            {
                trans.packet[index] = number;
                index++;
                if (index == 47)
                {
                    index = 0;
                    plt.Plot();
                }
            }
            raw = reader.ReadLine();
        }

        plt.finishedReading = true;
        plt.onfinished();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
