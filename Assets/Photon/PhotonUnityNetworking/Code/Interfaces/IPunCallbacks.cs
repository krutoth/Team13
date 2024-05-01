
namespace Photon.Pun
{
    using UnityEngine;
    using Photon.Realtime;


    /// <summary>Defines the OnPhotonSerializeView method to make it easy to implement correctly for observable scripts.</summary>
    /// \ingroup callbacks
    public interface IPunObservable
    {
        ///<摘要>
        ///由双关语每秒调用几次，以便脚本可以为光子视图写入和读取同步数据。
        ///</摘要>
        ///<备注>
        ///此方法将在指定为光子视图的“观察”组件的脚本中调用<br/>
        ///PhotonNetwork.SerializationRate会影响此方法的调用频率<br/>
        ///PhotonNetwork.SendRate影响此客户端发送包的频率<br/>
        ///
        ///通过实现此方法，可以自定义PhotonView定期同步的数据。
        ///您的代码定义了正在发送的内容以及接收客户端如何使用您的数据。
        ///
        ///与其他回调不同，<i>OnPhotonSerializeView只有在被赋值时才会被调用
        ///到PhotonView</i>作为PhotonView.observed脚本。
        ///
        ///要使用此方法，PhotonStream是必不可少的。它将在
        ///控制PhotonView（PhotonStream.IsWriting==true）并处于
        ///仅接收控制客户端发送的远程客户端。
        ///
        ///如果跳过将任何值写入流，PUN将跳过更新。小心使用，这个可以
        ///节省带宽和消息（每个房间/秒有一个限制）。
        ///
        ///请注意，当发送方不发送时，不会在远程客户端上调用OnPhotonSerializeView
        ///任何更新。这不能用作“每秒x次更新（）”。
        ///</备注>
        ///\公共Api组
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }


    /// <summary>
    /// Global Callback interface for ownership changes. These callbacks will fire for changes to ANY PhotonView that changes.
    /// Consider using IOnPhotonViewControllerChange for callbacks from a specific PhotonView.
    /// </summary>
    public interface IPunOwnershipCallbacks
    {
        /// <summary>
        /// Called when another player requests ownership of a PhotonView. 
        /// Called on all clients, so check if (targetView.IsMine) or (targetView.Owner == PhotonNetwork.LocalPlayer) 
        /// to determine if a targetView.TransferOwnership(requestingPlayer) response should be given.
        /// </summary>
        /// <remarks>
        /// The parameter viewAndPlayer contains:
        ///
        /// PhotonView view = viewAndPlayer[0] as PhotonView;
        ///
        /// Player requestingPlayer = viewAndPlayer[1] as Player;
        /// </remarks>
        /// <param name="targetView">PhotonView for which ownership gets requested.</param>
        /// <param name="requestingPlayer">Player who requests ownership.</param>
        void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer);

        /// <summary>
        /// Called when ownership of a PhotonView is transfered to another player.
        /// </summary>
        /// <remarks>
        /// The parameter viewAndPlayers contains:
        ///
        /// PhotonView view = viewAndPlayers[0] as PhotonView;
        ///
        /// Player newOwner = viewAndPlayers[1] as Player;
        ///
        /// Player oldOwner = viewAndPlayers[2] as Player;
        /// </remarks>
        /// <example>void OnOwnershipTransfered(object[] viewAndPlayers) {} //</example>
        /// <param name="targetView">PhotonView for which ownership changed.</param>
        /// <param name="previousOwner">Player who was the previous owner (or null, if none).</param>
        void OnOwnershipTransfered(PhotonView targetView, Player previousOwner);
        
        /// <summary>
        /// Called when an Ownership Request fails for objects with "takeover" setting.
        /// </summary>
        /// <remarks>
        /// Each request asks to take ownership from a specific controlling player. This can fail if anyone
        /// else took over ownership briefly before the request arrived.
        /// </remarks>
        /// <param name="targetView"></param>
        /// <param name="senderOfFailedRequest"></param>
        void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest);
    }

    /// \ingroup callbacks
    public interface IPunInstantiateMagicCallback
    {
        void OnPhotonInstantiate(PhotonMessageInfo info);
    }

    /// <summary>
    /// Defines an interface for object pooling, used in PhotonNetwork.Instantiate and PhotonNetwork.Destroy.
    /// </summary>
    /// <remarks>
    /// To apply your custom IPunPrefabPool, set PhotonNetwork.PrefabPool.
    ///
    /// The pool has to return a valid, disabled GameObject when PUN calls Instantiate.
    /// Also, the position and rotation must be applied.
    ///
    /// Note that Awake and Start are only called once by Unity, so scripts on re-used GameObjects
    /// should make use of OnEnable and or OnDisable. When OnEnable gets called, the PhotonView
    /// is already updated to the new values.
    ///
    /// To be able to enable a GameObject, Instantiate must return an inactive object.
    ///
    /// Before PUN "destroys" GameObjects, it will disable them.
    ///
    /// If a component implements IPunInstantiateMagicCallback, PUN will call OnPhotonInstantiate
    /// when the networked object gets instantiated. If no components implement this on a prefab,
    /// PUN will optimize the instantiation and no longer looks up IPunInstantiateMagicCallback
    /// via GetComponents.
    /// </remarks>
    public interface IPunPrefabPool
    {
        /// <summary>
        /// Called to get an instance of a prefab. Must return valid, disabled GameObject with PhotonView.
        /// </summary>
        /// <param name="prefabId">The id of this prefab.</param>
        /// <param name="position">The position for the instance.</param>
        /// <param name="rotation">The rotation for the instance.</param>
        /// <returns>A disabled instance to use by PUN or null if the prefabId is unknown.</returns>
        GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation);

        /// <summary>
        /// Called to destroy (or just return) the instance of a prefab. It's disabled and the pool may reset and cache it for later use in Instantiate.
        /// </summary>
        /// <remarks>
        /// A pool needs some way to find out which type of GameObject got returned via Destroy().
        /// It could be a tag, name, a component or anything similar.
        /// </remarks>
        /// <param name="gameObject">The instance to destroy.</param>
        void Destroy(GameObject gameObject);
    }
}