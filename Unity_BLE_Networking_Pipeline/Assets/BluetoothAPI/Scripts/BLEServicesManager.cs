using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using System;
using System.Text;

public class BLEServicesManager : MonoBehaviour
{
    private BluetoothHelper bluetoothHelper;
    private float timer;
    public Text text;
    int datum;
    void Start()
    {
        timer = 0;
        try{
            Debug.Log("HI");
            BluetoothHelper.BLE_AS_CLIENT = true;
            BluetoothHelper.BLE = true;  //use Bluetooth Low Energy Technology
            bluetoothHelper = BluetoothHelper.GetInstance();
            bluetoothHelper.OnConnected += (helper) => {
                List<BluetoothHelperService> services = helper.getGattServices();
                foreach (BluetoothHelperService s in services)
                {
                    Debug.Log("Service : " + s.getName());
                    foreach (BluetoothHelperCharacteristic item in s.getCharacteristics())
                    {
                        Debug.Log(item.getName());
                    }
                }

                Debug.Log("Connected");
                BluetoothHelperCharacteristic c = new BluetoothHelperCharacteristic("2A19");
                c.setService("180F");
                bluetoothHelper.Subscribe(c);
                Debug.Log("Smashing subscribe");
                //sendData();
            };
            bluetoothHelper.OnConnectionFailed += (helper)=>{
                Debug.Log("Connection failed");
            };
            bluetoothHelper.OnScanEnded += OnScanEnded;
            bluetoothHelper.OnServiceNotFound += (helper, serviceName) =>
            {
                Debug.Log("didn't find service " + serviceName);
            };
            bluetoothHelper.OnCharacteristicNotFound += (helper, serviceName, characteristicName) =>
            {
                Debug.Log("didn't find characteristic "+characteristicName);
            };
            bluetoothHelper.OnCharacteristicChanged += (helper, value, characteristic) =>
            {
                //Debug.Log(characteristic.getName());
                datum = value[0];
            };

            // BluetoothHelperService service = new BluetoothHelperService("FFE0");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("FFE1"));
            // BluetoothHelperService service2 = new BluetoothHelperService("180A");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            // bluetoothHelper.Subscribe(service);
            // bluetoothHelper.Subscribe(service2);
            // bluetoothHelper.ScanNearbyDevices();

            // BluetoothHelperService service = new BluetoothHelperService("19B10000-E8F2-537E-4F6C-D104768A1214");
            // service.addCharacteristic(new BluetoothHelperCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214"));
            //BluetoothHelperService service2 = new BluetoothHelperService("180A");
            //service.addCharacteristic(new BluetoothHelperCharacteristic("2A24"));
            // bluetoothHelper.Subscribe(service);
            //bluetoothHelper.Subscribe(service2);
            bluetoothHelper.ScanNearbyDevices();
        }catch(Exception ex){
            Debug.Log(ex.StackTrace);
        }
    }

    private void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> devices){
        Debug.Log("FOund " + devices.Count);
        if(devices.Count == 0){
            bluetoothHelper.ScanNearbyDevices();
            return;
        }

        foreach(var d in devices)
        {
            Debug.Log(d.DeviceName);
        }
            
        try
        {
            bluetoothHelper.setDeviceName("ButtonLED");
            bluetoothHelper.Connect();
            Debug.Log("Connecting");
        }catch(Exception ex)
        {
            bluetoothHelper.ScanNearbyDevices();
            Debug.Log(ex.Message);
        }

    }


    void OnDestroy()
    {
        if (bluetoothHelper != null)
            bluetoothHelper.Disconnect();
    }

    void Update(){
        //include if here, or text control
        text.text = datum.ToString();
        //if(bluetoothHelper == null)
        //    return;
        //timer += Time.deltaTime;

        //if(timer < 5)
        //    return;
        //timer = 0;
        //read();
    }

    void sendData(){
        // Debug.Log("Sending");
        // BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("FFE1");
        // ch.setService("FFE0"); //this line is mandatory!!!
        // bluetoothHelper.WriteCharacteristic(ch, new byte[]{0x44, 0x55, 0xff});

        Debug.Log("Sending");
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("19B10001-E8F2-537E-4F6C-D104768A1214");
        ch.setService("19B10000-E8F2-537E-4F6C-D104768A1214"); //this line is mandatory!!!
        bluetoothHelper.WriteCharacteristic(ch, "10001000"); //string: 10001000 is this binary? no, as string.
    }

    void read(){
        BluetoothHelperCharacteristic ch = new BluetoothHelperCharacteristic("2A19");
        ch.setService("180F");//this line is mandatory!!!
        Debug.Log("reading..");
        bluetoothHelper.ReadCharacteristic(ch);
        //Debug.Log(System.Text.Encoding.ASCII.GetString(x));
    }

}