using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Simple OSC test communication script
/// </summary>
[AddComponentMenu("Scripts/OSCTestSender")]
public class OSCTestSender : MonoBehaviour
{

    private Osc oscHandler;

    public string remoteIp = "127.0.0.1";
    public int sendToPort = 6448;
    public int listenerPort;


    ~OSCTestSender()
    {
        if (oscHandler != null)
        {            
            oscHandler.Cancel();
        }

        // speed up finalization
        oscHandler = null;
        System.GC.Collect();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //Debug.LogWarning("time = " + Time.time);

        //OscMessage oscM = Osc.StringToOscMessage("/test1 TRUE 23 0.501 bla");
        //oscHandler.Send(oscM); 

        OscMessage msgToSend = new OscMessage();
        msgToSend.Address = "/wek/inputs";
        //msgToSend.Values.Add(Input.mousePosition.x);
        //msgToSend.Values.Add(Input.mousePosition.y);

        msgToSend.Values.Add(8);

        oscHandler.Send(msgToSend);

    }

    void OnDisable()
    {
        // close OSC UDP socket
        Debug.Log("closing OSC UDP socket in OnDisable");
        oscHandler.Cancel();
        oscHandler = null;
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        
        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(remoteIp, sendToPort, listenerPort);
        
	    oscHandler = GetComponent<Osc>();
        oscHandler.init(udp);
        
        oscHandler.SetAddressHandler("/wek/inputs", Example);

        // Copy of code from Leap Motion
        OscMessage msg = new OscMessage();
        msg.Address = "/wek/inputs";
        msg.Values.Add(8);
        msg.Values.Add(10);

        oscHandler.Send(msg);

        
    }

    public static void Example(OscMessage m)
    {
        Debug.Log("--------------> OSC example message received: ("+m+")");
    }



}
