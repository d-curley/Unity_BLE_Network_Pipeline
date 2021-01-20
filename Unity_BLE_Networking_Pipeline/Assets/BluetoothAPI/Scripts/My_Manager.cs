using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArduinoBluetoothAPI;

public class My_Manager : MonoBehaviour
{
//    // Start is called before the first frame update
//    //CheckNetIsolation.exe LoopbackExempt -is -n=BluetoothDesktopServer_t05x7yb6e6yp0

//    private BluetoothHelper helper;
//    private string deviceName;
//    void Start()
//    {
//        deviceName = "ButtonLED";
//        try
//        {
//            helper = BluetoothHelper.Instance(deviceName);
//            helper.OnConnected += OnConnected;
//            helper.OnConnectionFailed += OnConnFailed;
//            helper.setTerminatorBasedStream("\n");

//            //helper.setLengthBasedStream();
//            if (helper.isDeviceFound())
//            {
//                helper.Connect();
//            }

//        }
//        catch (Exception ex)
//        {
//            sphere.GetComponent<Renderer>().material.color = Color.yellow;
//            Debug.Log(ex.Message);
//        }
//    }

//    void OnConnected()
//    {
//        Debug.Log("Connected to");
//        Debug.Log(deviceName);
//        helper.StartListening();

//        helper.SendData("Hi Arduino");
//    }

//    void OnConnFailed()
//    {
//        Debug.Log("Conn failed!");
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (helper.Available)
//        {
//            string msg = helper.Read();
//            print(msg);
//        }
//    }

//    void OnDestroy()
//    {
//        helper.StopListening();
//    }
}
