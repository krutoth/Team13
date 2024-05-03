// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to connect, and join/create room automatically
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections.Generic;
using System.Collections;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

	/// <summary>
	///启动经理。连接、加入随机房间或创建一个房间（如果没有或全部满）。
	/// </summary>
	public class 多人大厅 : MonoBehaviourPunCallbacks
	{
		#region 专用可序列化字段
		[Tooltip("每个房间的最大玩家数")]
		[SerializeField]
		private byte maxPlayersPerRoom = 8;

		#endregion

		#region 专用字段
		/// <summary>
		///跟踪当前流程。由于连接是异步的并且基于来自Photon的几个回调，
		///我们需要跟踪这一点，以便在收到光子的回电时正确调整行为。
		///通常，这用于OnConnectedToMaster（）回调。
		/// </summary>
		bool isConnecting;

		/// <summary>
		///此客户端的版本号。用户之间通过gameVersion（允许您进行突破性更改）进行分隔。
		/// </summary>
		string gameVersion = "1";

		#endregion

		#region MonoBehaviour 回调

		/// <summary>
		///Unity在早期初始化阶段对GameObject调用的MonoBehavior方法。
		/// </summary>
		void Awake()
		{
			//#关键
			//这确保我们可以在主客户端上使用PhotonNetwork.LoadLevel（），并且同一房间中的所有客户端都会自动同步其级别
			PhotonNetwork.AutomaticallySyncScene = true;

			//#至关重要的是，我们必须首先连接到光子在线服务器。
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = this.gameVersion;
		
		}

		public InputField 用户昵称;
		public InputField 公司名称;
		public InputField 电子邮箱;

		public GameObject 弹窗消息界面;
		public GameObject 弹窗消息;

		public Button 登錄按鈕;


		private void Start()
        {
		 加入随机房间();
		}

		#endregion

		#region 公共方法
		public string 当前操作记录 = "";
		public void 加入特定房间(string id)
		{
			当前操作记录 = "加入特定房间";

			//记录加入一个房间的意愿，因为当我们从游戏中回来时，我们会收到一个我们已经连接的回调，所以我们需要知道当时该怎么办
			isConnecting = true;

			//我们检查是否已连接，如果已连接，则加入，否则启动与服务器的连接。
			if (PhotonNetwork.IsConnected)
			{
				Debug.LogError("加入特定会议室结果：" + PhotonNetwork.JoinRoom(id));
			}
			else
			{
				Debug.LogError("至关重要的是，我们必须首先连接到光子在线服务器。  Connecting...");

				//#至关重要的是，我们必须首先连接到光子在线服务器。
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = this.gameVersion;
			}
		}

		public void 加入随机房间()
		{
			当前操作记录 = "加入随机房间";

			Debug.LogError("加入随机房间");

			//记录加入一个房间的意愿，因为当我们从游戏中回来时，我们会收到一个我们已经连接的回调，所以我们需要知道当时该怎么办
			isConnecting = true;

			//我们检查是否已连接，如果已连接，则加入，否则启动与服务器的连接。
			if (PhotonNetwork.IsConnected)
			{
				Debug.LogError("加入会议室...");
				//#关键是，此时我们需要尝试加入随机房间。如果失败，我们将在OnJoinRandomFailed（）中收到通知，并创建一个。

				PhotonNetwork.JoinRandomRoom();
			}
			else
			{

				Debug.LogError("至关重要的是，我们必须首先连接到光子在线服务器。  Connecting...");

				//#至关重要的是，我们必须首先连接到光子在线服务器。
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = this.gameVersion;
			}
		}

		///<摘要>
		///启动连接过程。
		///-如果已经连接，我们尝试加入一个随机房间
		///-如果尚未连接，请将此应用程序实例连接到光子云网络
		///</摘要>
		public void 创建房间(string id)
		{
			当前操作记录 = "创建房间";

			//记录加入一个房间的意愿，因为当我们从游戏中回来时，我们会收到一个我们已经连接的回调，所以我们需要知道当时该怎么办
			isConnecting = true;

			//我们检查是否已连接，如果已连接，则加入，否则启动与服务器的连接。
			if (PhotonNetwork.IsConnected)
			{
				Debug.LogError("创建会议室..." + id);
				//#关键是，此时我们需要尝试加入随机房间。如果失败，我们将在OnJoinRandomFailed（）中收到通知，并创建一个。
				Debug.LogError("房间创建结果：" + PhotonNetwork.CreateRoom(id, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }));
			}
			else
			{

				Debug.LogError("至关重要的是，我们必须首先连接到光子在线服务器。  Connecting...");

				//#至关重要的是，我们必须首先连接到光子在线服务器。
				PhotonNetwork.ConnectUsingSettings();
				PhotonNetwork.GameVersion = this.gameVersion;
			}
		}

		#endregion

		#region MonoBehavior Pun回调回调
		//下面，我们实现了PUN的一些回调
		//你可以在MonoBehaviorPunCallbacks类中找到PUN的回调
		///<摘要>
		///在建立与主机的连接并通过身份验证后调用
		///</摘要>
		public override void OnConnectedToMaster()
		{
			//如果我们不想加入一个房间，我们什么都不想做。
			//这种isConnecting为false的情况通常是当您输掉或退出游戏时，当加载该级别时，将调用OnConnectedToMaster，在这种情况下
			//我们什么都不想做。
			if (isConnecting)
			{
				Debug.LogError("OnConnectedToMaster:下一步->尝试加入随机房间");
				Debug.Log("PUN基础教程/启动器：由PUN调用了OnConnectedToMaster（）。现在，此客户端已连接，可以加入一个文件室。");

				//#关键：我们首先尝试加入一个潜在的现有房间。如果有，很好，否则，我们将用OnJoinRandomFailed（）调用
				PhotonNetwork.JoinRandomRoom();
			}
		}

		///<摘要>
		///当JoinRandom（）调用失败时调用。该参数提供错误代码和消息。
		///</摘要>
		///<备注>
		///很可能所有房间都已满或没有可用的房间<br/>
		///</备注>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			Debug.LogError("加入随机房间失败,没有找到一个可以用的房间");
			Debug.LogError("OnJoinRandomFailed:下一步->新建房间");
			Debug.Log("PUN基础教程/启动器：PUN调用了OnJoinRandomFailed（）。没有可用的随机房间，所以我们创建了一个。呼叫：PhotonNetwork.CreateRoom");

			//#关键：我们未能加入一个随机房间，可能没有，或者房间都满了。不用担心，我们创建了一个新房间。
			PhotonNetwork.CreateRoom("Web", new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
		}


		/// <summary>
		///在断开与光子服务器的连接后调用。
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{
			Debug.LogError("<Color=Red>OnDisconnected</Color> " + cause);
			Debug.LogError("PUN基础教程/启动器：断开连接");

			//#关键：我们无法连接或断开连接。我们能做的不多。通常，应该有一个UI系统，让用户尝试再次连接。

			isConnecting = false;
		}



		///<摘要>
		///在进入房间时调用（通过创建或加入房间）。在所有客户端（包括主客户端）上调用。
		///</摘要>
		///<备注>
		///此方法通常用于实例化玩家角色。
		///如果必须“主动”启动匹配，则可以调用由用户按下按钮或计时器触发的[PunRPC]（@ref PhotonView.RPC）。
		///
		///当调用此选项时，您通常已经可以通过PhotonNetwork.PlayerList访问房间中的现有玩家。
		///此外，所有自定义属性都应已作为Room.customProperties.Check Room可用。。PlayerCount以了解是否
		///房间里有足够多的球员开始比赛。
		///</备注>
		public override void OnJoinedRoom()
		{
			Debug.LogError("<Color=Green>OnJoinedRoom</Color>带有 " + PhotonNetwork.CurrentRoom.PlayerCount + " 个玩家");

			Debug.Log("PUN基础教程/启动器：PUN调用的OnJoinedRoom（）。现在这个客户端在一个房间里。从现在开始，你的游戏就可以运行了。");

			// #关键：只有当我们是第一个玩家时，我们才会加载，否则我们将依赖PhotonNetwork.AutomaticallySyncScene来同步我们的实例场景。
			if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				// #Critical
				//载入房间标高。
				//PhotonNetwork.LoadLevel("PunBasics-Room for 1");
				PhotonNetwork.LoadLevel("PunGame");
			}
		}
		#endregion

	public void 退出APP()
	{
	 Application.Quit(); 
	}



	}
}