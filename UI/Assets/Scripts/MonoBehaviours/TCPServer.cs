using Assets.Scripts.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TCPServer : MonoBehaviour
{
    #region [structs]
    public struct Agent
	{
		public GameObject GameObject;
		public PhysiologicalModel Need;
	}
	#endregion

	#region [vars] 	
	private TcpListener tcpListener;
	private Thread tcpListenerThread;
	private TcpClient connectedTcpClient;

	private Agent[] agents;

	int i = 0;
	#endregion

	void Update()
	{
		i++;
		if ( i == 300)
        {
			var agentObjs = GameObject.FindGameObjectsWithTag("Agent");

			agents = new Agent[agentObjs.Length];

			var iterator = 0;
			foreach(var obj in agentObjs)
			{
				agents[iterator] = new Agent() { GameObject = obj, Need = obj.GetComponent<PhysiologicalModel>() };
				iterator++;
			}

			tcpListenerThread = new Thread(new ThreadStart(ListenForRequests))
			{
				IsBackground = true
			};

			tcpListenerThread.Start();
		}
	}


	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForRequests()
	{
		try
		{		
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8052);
			tcpListener.Start();
			Debug.Log("Server is listening");
			Byte[] bytes = new Byte[1024];

			while (true)
			{
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					// Get a stream object for reading 					
					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							// Convert byte array to string message. 							
							var clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.Log("client message received as: " + clientMessage);

							ProcessIncomingMessage(clientMessage);
						}
					}
                }
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}

	private void ProcessIncomingMessage(string clientMessage)
    {
		var telegram = clientMessage.Split(';')[0];

		if(telegram == "GN" || telegram == "GetNeeds")
		{
			Debug.Log("Get recognized");

			string msg = "";
			IFormatProvider iFormatProvider = new System.Globalization.CultureInfo("en");

			//here is possible to add more needs if needed
			foreach (var agent in agents)
			{
				msg += String.Format(iFormatProvider, "{0:0.###}", agent.Need.FoodNeed.Value) + ","
					+ String.Format(iFormatProvider, "{0:0.###}", agent.Need.WaterNeed.Value) + ","
					+ String.Format(iFormatProvider, "{0:0.###}", agent.Need.DreamNeed.Value) + ","
					+ String.Format(iFormatProvider, "{0:0.###}", agent.Need.SexNeed.Value) + ","
					+ String.Format(iFormatProvider, "{0:0.###}", agent.Need.ToiletNeed.Value) + ","
					//+ "1\n";
					+ String.Format(iFormatProvider, "{0:0.###}", agent.Need.HigherOrderNeeds.Value) + "\n";
			}

			SendMessage(msg);
		}
		else if(telegram == "SPOL" || telegram == "SetPurposeOfLife")
		{
			Debug.Log("Set recognized");

			var data = clientMessage.Split(';')[1];
			var dataForAgents = data.Split(',');

			if(dataForAgents.Length == agents.Length)
			{
				var iterator = 0;
				foreach (var d in dataForAgents)
				{
					agents[iterator].Need.PurposeOfLife = (ListOfNeeds?)Convert.ToInt32(d);
					iterator++;
				}
			}
		}
    }

	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	private new void SendMessage(string msg)
	{
		if (connectedTcpClient == null)
		{
			return;
		}

		try
		{
			// Get a stream object for writing. 			
			NetworkStream stream = connectedTcpClient.GetStream();
			if (stream.CanWrite)
			{
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(msg);
				// Write byte array to socketConnection stream.               
				stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
				Debug.Log("Server sent his message - should be received by client");
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}
}