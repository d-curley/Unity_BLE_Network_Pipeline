using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;
using Mirror;

public class manager : MonoBehaviour
{
    // Use this for initialization
    //CheckNetIsolation.exe LoopbackExempt -is -n=BluetoothDesktopServer_t05x7yb6e6yp0
    BluetoothHelper bluetoothHelper;
    //string deviceName = "ButtonLED";
    string deviceName = "Accel_Controller";
    public Text text;
    public float datum;
    //public GameObject sphere;
    bool found=false;
    string received_message;
    List<BluetoothHelperService> eMG_char;
    List<String> serviceNames;
    public bool connected = false;

    [Server]
    void Start()
    {
        text.text = "Waiting for connection";
        try
        {
            BluetoothHelper.BLE_AS_CLIENT = true;
            BluetoothHelper.BLE = true;
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnScanEnded += OnScanEnded;
            bluetoothHelper.OnServiceNotFound += (helper, serviceName) =>
            {
                Debug.Log("didn't find service " + serviceName);
            };
            bluetoothHelper.OnCharacteristicNotFound += (helper, serviceName, characteristicName) =>
            {
                Debug.Log("didn't find characteristic " + characteristicName);
            };
            bluetoothHelper.OnCharacteristicChanged += (helper, value, characteristic) =>
            {
                datum = (value[0] - 40f) / 10f;
            };

            bluetoothHelper.ScanNearbyDevices();
            text.text = "Scanning";
        }
        catch (Exception ex)
        {
            //sphere.GetComponent<Renderer>().material.color = Color.yellow;
            Debug.Log(ex.Message);
            text.text = ex.Message;
        }
    }

    [Server]
    void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> nearbyDevices)
    {
        //this is still finding our BLE device even hours after it's plugged in. 
        //If the device is plugged in, this works fine, so maybe tell users to make sure their device is 
        //connected if the values don't change?
        Debug.Log("Found: " + nearbyDevices.Count.ToString() + " Devices");
        Debug.Log("Looking for: " + deviceName);
        if (nearbyDevices.Count == 0)
        {
            Debug.Log("didn't find any, looking again");
            helper.ScanNearbyDevices();
            return;
        }
        foreach (BluetoothDevice device in nearbyDevices)
        {
            Debug.Log(device.DeviceName);
            if (device.DeviceName == deviceName)
            {
                Debug.Log("Found!!");
                found = true; 
            } 
        }
        if (found==true)
        {
            Debug.Log("found=true");
            try
            {
                bluetoothHelper.setDeviceName(deviceName);
                bluetoothHelper.Connect();
                //bluetoothHelper.isDevicePaired();
                Debug.Log("Now Connecting...");
            }
            catch (Exception ex)
            {
                bluetoothHelper.ScanNearbyDevices();
                Debug.Log(ex.Message);
            }
        }
        else
        {
            Debug.Log("Need to Scan again");
            helper.ScanNearbyDevices();
            return;
        } 
    }
    [Server]
    void OnConnected(BluetoothHelper helper)
    {
        Debug.Log("Connected");
        connected = true;

        List<BluetoothHelperService> services = helper.getGattServices();
        foreach (BluetoothHelperService s in services)
        {
            Debug.Log("Service : " + s.getName());
            foreach (BluetoothHelperCharacteristic item in s.getCharacteristics())
            {
                Debug.Log(item.getName());
            }
        }
        BluetoothHelperCharacteristic c = new BluetoothHelperCharacteristic("2A19");
        c.setService("180F");
        bluetoothHelper.Subscribe(c);
        Debug.Log("Smashing subscribe");
    }

    //Asynchronous method to receive messages
    [Server]
    void OnConnectionFailed(BluetoothHelper helper)
    {
        //sphere.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("Connection Failed");
    }
    [Server]
    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }
}


