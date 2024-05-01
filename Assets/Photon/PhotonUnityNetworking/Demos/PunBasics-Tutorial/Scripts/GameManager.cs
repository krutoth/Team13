//--------------------------------------------------------------------------------------------------------------------
//<copyright file=“Launcher.cs”company=“Exit Games GmbH”>版权所有
//部分：Photon Unity网络演示
//</版权所有>
//<摘要>
//在“PUN基本教程”中用于处理典型的游戏管理要求
//</摘要>
//<作者>developer@exitgames.com</作者>
//--------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

	///<摘要>
	///游戏经理。
	///连接并观看光子状态，实例化玩家
	///处理离开房间和游戏
	///处理水平加载（室内同步之外）
	///</摘要>
	public class GameManager : MonoBehaviourPunCallbacks
    {
		#region 公共领域

		public static GameManager Instance;

		#endregion

		#region 专用字段

		private GameObject instance;

        [Tooltip("用于代表玩家的预制件")]
        [SerializeField]
        private GameObject playerPrefab;

		#endregion

		#region MonoBehaviour CallBacks

		///<摘要>
		///Unity在初始化阶段对GameObject调用的MonoBehavior方法。
		///</摘要>
		void Start()
		{
			Instance = this;

			//如果我们在启动这个演示时激活了错误的场景，只需加载菜单场景
			if (!PhotonNetwork.IsConnected)
			{
				Debug.LogError("系统检测连接异常，未连接");
				SceneManager.LoadScene("初始化房间");
				return;
			}




			if (playerPrefab == null)
			{ //#提示永远不要假设组件的公共属性已正确填充，请始终检查并通知开发人员。
				Debug.LogError("<Color=Red><b>缺少</b></Color>播放器预设参考。请在GameObject“Game Manager”中设置", this);
			} else {
				if (PlayerManager.LocalPlayerInstance==null)
				{
				    Debug.LogFormat("我们正在从｛0｝实例化LocalPlayer", SceneManagerHelper.ActiveSceneName);

					//我们在一个房间里。为本地玩家生成一个角色。使用PhotonNetwork进行同步。实例化
					//PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);

					Quaternion roate = Quaternion.Euler(0,-90, 0);     //目的角度
					PhotonNetwork.Instantiate(this.playerPrefab.name,new Vector3(-50f, -2, -2), roate, 0);
				}
				else
				{

					Debug.LogFormat("忽略｛0｝的场景加载", SceneManagerHelper.ActiveSceneName);
				}
			}

		}

		///// <summary>
		///// MonoBehaviour method called on GameObject by Unity on every frame.
		///// </summary>
		//void Update()
		//{
		//	//手机的“后退”按钮等于“退出”。按下后退出应用程序
		//	if (Input.GetKeyDown(KeyCode.Escape))
		//	{
		//		QuitApplication();
		//	}
		//}

		#endregion

		#region Photon Callbacks

		///<摘要>
		///当光子播放器连接时调用。然后我们需要加载一个更大的场景。
		///</摘要>
		///<param name=“other”>其他</参数>
		public override void OnPlayerEnteredRoom( Player other  )
		{
			Debug.Log( "有新的玩家加入： " + other.NickName);//没有看到你是否是连接的玩家

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient ); //在OnPlayer离开房间之前调用

				LoadArena();
			}
		}

		/// <summary>
		///当光子播放器断开连接时调用。我们需要加载一个较小的场景。
		/// </summary>
		/// <param name="other">Other.</param>
		public override void OnPlayerLeftRoom( Player other  )
		{
			Debug.Log( "有新的玩家断开联机： " + other.NickName );//其他断开连接时看到

			if ( PhotonNetwork.IsMasterClient )
			{
				Debug.LogFormat( "OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient );//在玩家离开房间之前调用

				LoadArena(); 
			}
		}

		/// <summary>
		/// 当本地玩家离开房间时呼叫。我们需要加载发射器场景。
		/// </summary>
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene("初始化房间");
		}

		#endregion

		#region Public Methods

		public bool LeaveRoom()
		{
			return PhotonNetwork.LeaveRoom();
		}

		public void QuitApplication()
		{
			Application.Quit();
		}

		#endregion

		#region Private Methods

		void LoadArena()
		{
			if ( ! PhotonNetwork.IsMasterClient )
			{
				Debug.LogError("PhotonNetwork：试图加载一个级别，但我们不是主客户端");
				return;
			}

			Debug.LogFormat("光子网络：加载级别：｛0｝", PhotonNetwork.CurrentRoom.PlayerCount );


			//广播通知所有人切换场景  神经病
			//PhotonNetwork.LoadLevel("PunBasics-Room for "+PhotonNetwork.CurrentRoom.PlayerCount);
		}

		#endregion

	}

}