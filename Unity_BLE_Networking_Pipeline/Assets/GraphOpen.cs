using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using Mirror;

public class GraphOpen : MonoBehaviour
{
    List<float> eMG_pull = new List<float>();
    private RectTransform GraphContainer;
    [SerializeField] private Sprite circleSprite;
    [SerializeField] private int datalength=750;


    //graph isn't showing up for anyone now that I've added the network behavior and network identity to the canvas. Maybe make this client only?
    void Start()
    {
        Debug.Log("Graph Open!");

        //-----------Load Data----------------
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            GraphData GraphObject=JsonUtility.FromJson<GraphData>(saveString);
            eMG_pull = GraphObject.eMG;
            datalength=eMG_pull.Count;
            Debug.Log("Graph datalength check: " + datalength.ToString());
        }
        else
        {
            Debug.Log("Couldn't find File");
        }

        //------Mock Data for Troubleshooting-----
        //for(int i = 0; i < datalength; i++)
        //{
        //    float l = datalength / 10f;
        //    eMG_pull.Add(((float)i/l)-4);
        //}

        GraphContainer = transform.Find("GraphContainer").GetComponent<RectTransform>();

        for(int i = 50; i < datalength; i++)//take oiut hardcoding
        {
            float x = Map(i, 50f, datalength, 0f, 750f);//take width and height of rectTransform instead
            float y = Map(eMG_pull[i], -4f, 6f, 0f, 400f);

            GraphPoint(new Vector2(x, y));
        }


        //-----Graphing in scene------
        //int l = data.Count;
        //Debug.Log(l);
        //GraphPoint(new Vector2(0, 0));
        //GraphPoint(new Vector2(750, 400));
        //for(int i=0; i < Mathf.Round(l/3); i++)
        //{
        //    float x=Mathf.Round((i - (l / 6)) * (2400 / l));
        //    GraphPoint(new Vector2(-x, (data[i*3])*25));
        //}
    }

    private void GraphPoint(Vector2 anchoredPosition)
    {
        GameObject point = new GameObject("circle", typeof(Image));
        point.transform.SetParent(GraphContainer, false);
        point.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = point.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    private float Map(float check, float fromMin, float fromMax, float toMin, float toMax)
    {
        var fromAbs = check - fromMin;
        var fromMaxAbs = fromMax - fromMin;
        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;
        return to;
    }
}

