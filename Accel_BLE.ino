

/*
  This example creates a BLE peripheral with service that contains a
  characteristic to control an LED and another characteristic that
  represents the state of the button.

  You can use a generic BLE central app, like LightBlue (iOS and Android) or
  nRF Connect (Android), to interact with the services and characteristics
  created in this sketch.
*/

#include <ArduinoBLE.h>
#include <Arduino_LSM6DS3.h>

BLEService Acceleration("180F"); // create service
//19B10011-E8F2-537E-4F6C-D104768A1214

// create switch characteristic and allow remote device to read and notify
BLEUnsignedCharCharacteristic Z_Axis("2A17",BLERead | BLENotify);  // standard 16-bit characteristic UUID                                    
BLEUnsignedCharCharacteristic Y_Axis("2A18",BLERead | BLENotify);
BLEUnsignedCharCharacteristic X_Axis("2A19", BLERead | BLENotify); 
                                     
void setup() {
  Serial.begin(9600);
  Serial.println("setup");
  
  // begin BLE initialization
  Serial.println("Starting BLE");
  if (!BLE.begin()) {
    Serial.println("starting BLE failed!");
    while (1);
  }

  BLE.setLocalName("Accel_Controller");  // set the local name peripheral advertises
  BLE.setDeviceName("Accel_Controller");
  BLE.setAdvertisedService(Acceleration);// set the UUID for the service this peripheral advertises:

  //maybe we could send an array instead of 3 different characteristics?
  //add inidivudal characteristics to our service "Acceleration
  Acceleration.addCharacteristic(Z_Axis);
  Acceleration.addCharacteristic(Y_Axis);
  Acceleration.addCharacteristic(X_Axis);

  BLE.addService(Acceleration);// add the service
  BLE.advertise();
  Serial.println("Bluetooth device active, waiting for connections...");

  //initialize IMU, which will be collecting acceleration data to send over BLE
  IMU.begin();
  delay(3000);
  if (!IMU.begin()) {
    Serial.println("IMU failed");
    while (1);
  }
}

void loop() {
  //initialize IMU variables
  float x, y, z;
  //float rx,ry,rz;
  
  BLE.poll(); // poll for BLE events

  if (IMU.accelerationAvailable()) {
    //read acceleration, convert to 0-180 so we can send ints instead of floats
    IMU.readAcceleration(x, y, z);
    int c = (z+1)*90;//0-180
    int b = (y+1)*90;
    int a = (x+1)*90;

    //write new values to our characteristics on the service
    X_Axis.writeValue(a);
    Y_Axis.writeValue(b);
    Z_Axis.writeValue(c);

  //serial prints for troubleshooting
  Serial.print("X: ");Serial.print(a);
  Serial.print("    Y: ");Serial.print(b);
  Serial.print("    Z: ");Serial.println(c);
  }



  /*below is for if we wanted to send gyroscope data instead
   * if(IMU.gyroscopeAvailable()){
    IMU.readGyroscope(rx,ry,rz);

    float ra=(rx+1000)/10;
    float rb=(ry+1000)/10;
    float rc=(rz+1000)/10;
    
    X_Axis.writeValue(ra);
    Y_Axis.writeValue(rb);
    Z_Axis.writeValue(rc);

    Serial.print("RX: ");Serial.print(ra);
    Serial.print("    RY: ");Serial.print(rb);
    Serial.print("    RZ: ");Serial.println(rc);
  }*/
  
  delay(100);
}
