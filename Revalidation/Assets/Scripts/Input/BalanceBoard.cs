using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Timers;
using UnityEngine;
using WiimoteLib;

public class BalanceBoard : MonoBehaviour
{
    private Timer infoUpdateTimer = new Timer() { Enabled = false, Interval = 50 };


    Wiimote wiiDevice = new Wiimote();
    bool once = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (once)
        {
            ConnectBoard();
            once = false;
        }
    }

    private void ConnectBoard()
    {
        try
        {
            // Find all connected Wii devices.

            var deviceCollection = new WiimoteCollection();
            deviceCollection.FindAllWiimotes();

            for (int i = 0; i < deviceCollection.Count; i++)
            {
                wiiDevice = deviceCollection[i];

                // Device type can only be found after connection, so prompt for multiple devices.

                if (deviceCollection.Count > 1)
                {
                    var devicePathId = new Regex("e_pid&.*?&(.*?)&").Match(wiiDevice.HIDDevicePath).Groups[1].Value
                        .ToUpper();
                }

                // Setup update handlers.

                wiiDevice.WiimoteChanged += wiiDevice_WiimoteChanged;
                wiiDevice.WiimoteExtensionChanged += wiiDevice_WiimoteExtensionChanged;

                // Connect and send a request to verify it worked.

                wiiDevice.Connect();
                wiiDevice.SetReportType(InputReport.IRAccel,
                    false); // FALSE = DEVICE ONLY SENDS UPDATES WHEN VALUES CHANGE!
                wiiDevice.SetLEDs(true, false, false, false);

                // Enable processing of updates.
                Debug.Log("we in this ho");
                infoUpdateTimer.Enabled = true;

                break;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    private void wiiDevice_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
    {
        // Called every time there is a sensor update, values available using e.WiimoteState.
        // Use this for tracking and filtering rapid accelerometer and gyroscope sensor data.
        // The balance board values are basic, so can be accessed directly only when needed.
    }

    private void wiiDevice_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs e)
    {
        // This is not needed for balance boards.
    }

    void infoUpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
    {
        StartCoroutine(InfoUpdate());
    }

    private IEnumerator InfoUpdate()
    {
        if (wiiDevice.WiimoteState.ExtensionType != ExtensionType.BalanceBoard)
        {
            Debug.Log("DEVICE IS NOT A BALANCE BOARD...");
            yield return null;
        }

        var rwWeight = wiiDevice.WiimoteState.BalanceBoardState.WeightKg;

        var rwTopLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopLeft;
        var rwTopRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.TopRight;
        var rwBottomLeft = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomLeft;
        var rwBottomRight = wiiDevice.WiimoteState.BalanceBoardState.SensorValuesKg.BottomRight;
        var aButton = wiiDevice.WiimoteState.ButtonState.A;

        Debug.Log(rwTopRight);
    }
}