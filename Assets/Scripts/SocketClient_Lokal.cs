using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
public class SocketClient_Lokal: MonoBehaviour 
{
    internal Boolean socketReady = false;
    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    StreamReader theReader;
    String Host = "localhost";
    Int32 Port = 9999;

    void Start()
    {
        setupSocket();
    }

    void Update() 
    { 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            writeSocket("FUNCTION#CreateCube");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Schließe Socket...");
            closeSocket();
        }
    }

    private void startClientCoroutines()
    {
        // Start infinite coroutine for reading server messages
        StartCoroutine("receiveMessages");
        Debug.Log("Coroutine receiveMessages gestartet...");
        StartCoroutine("sendCheckMessage");
        Debug.Log("Coroutine sendCheckMessage gestartet...");
    }

    private IEnumerator sendCheckMessage()
    {
        while (true)
        {
            String foo = "AAA" + "\r\n";
            theWriter.Write(foo);
            theWriter.Flush();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator receiveMessages()
    {
        while(true)
        {
            String message = readSocket();
            if (message.Length > 0)
            {
                if (message.Equals("UpdateAAA"))
                {
                    Debug.Log("Update erhalten...");
                    // Krasser UI Kram switchen etc.
                }
                else if (message.Equals("AAA"))
                {
                    // No update received.. progress further
                }
                else
                {
                    Debug.Log("Message received: " + message);
                    // Normale verarbeitung ...
                }
            }
            yield return new WaitForSeconds(0.1f); //Führ es wieder nach 1s aus
        }
    }

    // **********************************************
    public void setupSocket() 
    {
        try {
            Debug.Log("Starte Tcp Client");
            mySocket = new TcpClient(Host, Port);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
            theReader = new StreamReader(theStream);
            socketReady = true;
            Debug.Log("Socket ist bereit...");
            startClientCoroutines(); // Start Couroutine function
        } catch (Exception e) {
            Debug.Log("Socket error: " + e);
        }
    }

    public void writeSocket(string theLine) 
    {
        if (!socketReady)
            return;
        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
    }

    public String readSocket() 
    {
        if (!socketReady)
            return "";
        if (theStream.DataAvailable)
            return theReader.ReadLine();
        return "";
    }

    public void closeSocket() 
    {
        if (!socketReady)
            return;
        theWriter.Close();
        theReader.Close();
        mySocket.Close();
        socketReady = false;
        Debug.Log("Socket geschlossen...");
    }
} // end class 