// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNameInputField.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Let the player input his name to be saved as the network player Name, viewed by alls players above each  when in the same room. 
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.Demo.PunBasics
{
	/// <summary>
	/// Player name input field. Let the user input his name, will appear above the player in the game.
	/// </summary>
	[RequireComponent(typeof(InputField))]
	public class PlayerNameInputField : MonoBehaviour
	{
		#region Private Constants

		//存储PlayerPref密钥以避免拼写错误
		const string playerNamePrefKey = "PlayerName";

		#endregion

		#region MonoBehaviour CallBacks

		/// <summary>
		///Unity在初始化阶段对GameObject调用的MonoBehavior方法。
		/// </summary>
		void Start () 
		{
		
			string defaultName = string.Empty;
			InputField _inputField = this.GetComponent<InputField>();

			if (_inputField!=null)
			{
				if (PlayerPrefs.HasKey(playerNamePrefKey))
				{
					defaultName = PlayerPrefs.GetString(playerNamePrefKey);
					_inputField.text = defaultName;
				}
			}

			PhotonNetwork.NickName =	defaultName;
		}

		#endregion

		#region Public Methods

		/// <summary>
		///设置玩家的名称，并将其保存在PlayerPrefs中以备将来使用。
		/// </summary>
		/// <param name="value">The name of the Player</param>
		public void SetPlayerName(string value)
		{
			// #Important
		    if (string.IsNullOrEmpty(value))
		    {
                Debug.LogError("玩家名称为null或为空");
		        return;
		    }

			PhotonNetwork.NickName = value;

			PlayerPrefs.SetString(playerNamePrefKey, value);
		}
		
		#endregion
	}
}
