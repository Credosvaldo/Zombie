using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace MyGame
{
    public class PainelJogar : MonoBehaviourPunCallbacks
    {
        [SerializeField] Button botao_Comecar;
        void Start()
        {
            botao_Comecar.interactable = PhotonNetwork.IsMasterClient;
        }

        


    }

}

