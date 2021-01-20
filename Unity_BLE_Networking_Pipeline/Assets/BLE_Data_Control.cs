using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mirror;
//CheckNetIsolation.exe LoopbackExempt -is -n=BluetoothDesktopServer_t05x7yb6e6yp0

public class BLE_Data_Control: NetworkBehaviour
{
    public GameObject sphere0;
    public GameObject sphere1;
    public GameObject sphere2;
    public GameObject sphere3;
    public GameObject sphere4;
    public GameObject sphere5;
    public GameObject sphere6;
    public GameObject sphere7;
    public GameObject sphere8;
    public GameObject sphere9;
    public GameObject sphere10;

    // public List<float> eMG = new List<float>();

    private manager manager;
    float Ypos = 0f;

    List<float> data = new List<float>();
    int datalength;

    void Awake()
    {
        manager = GetComponent<manager>();
    }

    void Update()
    {
        if (manager.connected)//client is never getting manager as connected
        {
            Ypos = manager.datum;
            data.Add(Ypos);
            datalength = data.Count;

            sphere0.transform.localPosition = new Vector3(10f, Ypos, 0f);
            if (datalength > 50)
            {
                sphere1.transform.localPosition = new Vector3(8f, data[datalength - 5], 0f);
                sphere2.transform.localPosition = new Vector3(6f, data[datalength - 10], 0f);
                sphere3.transform.localPosition = new Vector3(4f, data[datalength - 15], 0f);
                sphere4.transform.localPosition = new Vector3(2f, data[datalength - 20], 0f);
                sphere5.transform.localPosition = new Vector3(0f, data[datalength - 25], 0f);
                sphere6.transform.localPosition = new Vector3(-2f, data[datalength - 30], 0f);
                sphere7.transform.localPosition = new Vector3(-4f, data[datalength - 35], 0f);
                sphere8.transform.localPosition = new Vector3(-6f, data[datalength - 40], 0f);
                sphere9.transform.localPosition = new Vector3(-8f, data[datalength - 45], 0f);
                sphere10.transform.localPosition = new Vector3(-10f, data[datalength - 50], 0f);
            }
        }
        if (isClient && sphere0.transform.localPosition.y!=0f)
        {
            data.Add(sphere0.transform.localPosition.y);
        }
    }

    public void Done()
    {
        Debug.Log("client data length " +data.Count.ToString());
        //---JSON serialization-------
        GraphData GraphObject = new GraphData(); //{ eMG = data, };
        GraphObject.eMG = data; //maybe need to cast?
        string json= JsonUtility.ToJson(GraphObject);
        
        Debug.Log("eMG Count: " + GraphObject.eMG.Count.ToString());
        File.WriteAllText(Application.dataPath + "/save.txt", json);
            
        SceneManager.LoadScene("Graph"); 
    }
}

[Serializable]
public class GraphData
{
    public List<float> eMG = new List<float>();
}



