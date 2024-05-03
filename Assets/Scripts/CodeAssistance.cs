using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeAssistance : MonoBehaviour
{
public Photon.Pun.Demo.PunBasics.多人大厅 _a;
    // Start is called before the first frame update
   
    public void CallDUO(){
        _a.加入随机房间();
    }
}
