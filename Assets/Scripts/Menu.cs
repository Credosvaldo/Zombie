using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class Menu : MonoBehaviourPunCallbacks
    {
        #region Public
        public static Menu instancia;
        #endregion

        #region Serialized
        [SerializeField] GameObject[] painel;
        [SerializeField] Text nomeJogadores;
        [SerializeField] Button botao_Comecar;

        #endregion


        #region Private
        #endregion

        void Awake()
        {
            if (instancia != null)
                Destroy(gameObject);

            instancia = this;

            CarregarPainel(100);

        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = "jogador01";
        }


        public void CarregarPainel(int value = 0)
        {
            foreach(GameObject p in painel)
            {
                p.SetActive(false);
            }

            if(value<painel.Length)
            {
                painel[value].SetActive(true);
            }

        }

        public void Jogar()
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
        }

        public void Sair()
        {
            Application.Quit();
        }

        public void Comecar()
        {
            photonView.RPC("Comecar2", RpcTarget.AllBuffered);
        }

        [PunRPC]
        void Comecar2()
        {
            PhotonNetwork.LoadLevel(1);
        }

        public void SairDaSala()
        {
            PhotonNetwork.LeaveRoom();
            CarregarPainel();
        }

        [PunRPC]
        void AtualizarLista()
        {
            string lista = "";
            foreach (var player in PhotonNetwork.PlayerList)
            {
                lista += player.NickName + "\n";
            }
            
            nomeJogadores.text = lista;
        }

        #region Override Photon
        public override void OnConnectedToMaster()
        {
            CarregarPainel();
        }

        public override void OnJoinedRoom()
        {
            CarregarPainel(2);

            photonView.RPC("AtualizarLista", RpcTarget.AllBuffered);
            botao_Comecar.interactable = PhotonNetwork.IsMasterClient;
 
        }


        public override void OnLeftRoom()
        {
            CarregarPainel();
        }


        #endregion


    }

}
