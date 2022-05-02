using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebsocketClient : MonoBehaviour {	

	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread;
	private NetworkStream stream;
	private Byte[] bytes;	
	#endregion


	// Use this for initialization 	
	void Start () 
	{
		ConnectToTcpServer();
	} 

	private void ConnectToTcpServer () {
		socketConnection = new TcpClient("localhost", 9999);			
		bytes = new Byte[1024];
		Debug.Log("TCPClient steht");	
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ReceiveMessage)); 			
			clientReceiveThread.IsBackground = true;			
			clientReceiveThread.Start();  		
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}	

	// Update is called once per frame
	void Update () 
	{         
		if (Input.GetKeyDown(KeyCode.Space)) 
		{             
			SendMessage();         
		}
	}

	private void ReceiveMessage()
	{
		try { 			
			
			Debug.Log("Variablen initialiiert ReceiveMessage");        
			while (true) {
				
							
				// Get a stream object for reading
				NetworkStream stream = socketConnection.GetStream();							
				int length;					
				// Read incomming stream into byte arrary. 					
				while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) {
					Debug.Log("Lese Zeug in ReceiveMesage");						
					var incommingData = new byte[length]; 						
					Array.Copy(bytes, 0, incommingData, 0, length); 						
					// Convert byte array to string message. 						
					string serverMessage = Encoding.ASCII.GetString(incommingData); 						
					Debug.Log("server message received as: " + serverMessage);

					if ((length = stream.Read(bytes, 0, bytes.Length)) == 0)
					{
						// Closing the tcpClient instance does not close the network stream.
						
						Debug.Log("habe fertig gelesen");
						socketConnection = new TcpClient("localhost", 9999);
						Debug.Log("Socket neu aufgrmacht");
					}
				} 							
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		} 
	}
	
	private void SendMessage() {         
		if (socketConnection == null) {
			Debug.Log("Socket ist null");           
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
				string clientMessage = "This is a message from one of your clients."; 				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}
}