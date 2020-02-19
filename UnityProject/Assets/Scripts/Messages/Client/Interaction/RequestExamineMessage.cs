using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mirror;
using UnityEngine;

/// <summary>
/// Requests for the server to perform examine interaction
/// </summary>
public class RequestExamineMessage : ClientMessage
{
	//TODO: Is this constant needed anymore
	public static short MessageType = (short) MessageTypes.RequestExamine;

	//members
	// netid of target
	public uint examineTarget;

	static RequestExamineMessage()
	{
		//constructor

	}

	public override IEnumerator Process()
	{

		string msg = "";
		//TODO: check break conditions
		if (SentByPlayer == null || SentByPlayer.Script == null)
		{
			yield break;
		}

		// Sort of translates one or more netId to gameobjects contained in NetworkObjects[]
		// it's confusing AF to me atm.
		yield return WaitFor(examineTarget);

		// Here we build the message to send, by looking at the target's components. 
		// anyone extending IExaminable gets a say in it.
		// Look for examinables.
		var examinables = NetworkObject.GetComponents<IExaminable>();
		foreach (var examinable in examinables)
		{
			msg += $"{examinable.Examine()}\n";
		}
		
		// Send the message.
		Chat.AddExamineMsgFromServer(SentByPlayer.GameObject, msg);
	}

	public static void Send(uint targetNetId)
	{
		// TODO: Log something?
		var msg = new RequestExamineMessage()
		{
			examineTarget = targetNetId
		};
		msg.Send();
	}

	// //TODO: Figure out serial/deserialization?
	// public override void Deserialize(NetworkReader reader)
	// {

	// }

	// public override void Serialize(NetworkWriter writer)
	// {
	
	// }

}

