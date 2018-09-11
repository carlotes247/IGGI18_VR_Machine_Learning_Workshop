using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component to receive OSC messages from Wekinator
/// </summary>
public class OSCReceiverController : MonoBehaviour {
    #region Variables

    //You can set these variables in the scene because they are public 
    // Ip To connect to
    public string RemoteIP = "127.0.0.1";

    // Ports to work with Wekinator
    public int SendToPort = 6448;
    public int ListenerPort  = 12000;

    // OSC message handler
    private Osc handler;

    // Signals received from the message
    public float sig1;
    public float sig2;

    // The object to moidfy in the scene
    public GameObject m_ObjectToUse;

    #endregion

    ~OSCReceiverController()
    {
        if (handler != null)
        {
            handler.Cancel();
        }

        // speed up finalization
        handler = null;
        System.GC.Collect();
    }


    // Use this for initialization
    void Start () {
        //Initializes on start up to listen for messages
        //make sure this game object has both UDPPackIO and OSC script attached

        UDPPacketIO udp = GetComponent<UDPPacketIO>();
        udp.init(RemoteIP, SendToPort, ListenerPort);
        handler = GetComponent<Osc>();
        handler.init(udp);

        //Tell Unity to call function Example1 when message /wek/outputs arrives
        handler.SetAddressHandler("/wek/outputs", Example1);

        // Find the GameObject we want to use if it is not set up from the inspector
        if (m_ObjectToUse == null)
            m_ObjectToUse = GameObject.Find("Cube1");

        Debug.Log("OSC Running");

    }

    // Update is called once per frame
    void Update () {
        // Apply the rotation from the two signals received
        m_ObjectToUse.transform.Rotate(0, sig1, sig2);
	}

    /// <summary>
    /// Example method that is called when /wek/outputs arrives (specified on Start)
    /// </summary>
    /// <param name="message"></param>
    public void Example1(OscMessage message)
    {
        // Debug calls to show how the osc message from Wekinator looks like
        Debug.Log("Called Example One > " + Osc.OscMessageToString(message));
        Debug.Log("Message Values > " + message.Values[0] + " " + message.Values[1]);

        // Get the two first values from the message 
        sig1 = (float)message.Values[0];
        sig2 = (float)message.Values[1];

    }
}
