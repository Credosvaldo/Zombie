using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public
        public static GameManager instancia { get; private set; }
        public Transform cam;
        public FloatingJoystick movmentJoystick;
        public FloatingJoystick shotterJoystick;
        public Slider slider_LootBoxTime;
        public Button botao_TrocarArma;
        public Button botao_Reload;
        public GameObject[] panel;
        public GunUI selectedGun;
        public int zombiesInScene;
        [System.NonSerialized] public GameObject[] livePlayerList;
        [System.NonSerialized] public GameObject[] deadPlayerList;
        [System.NonSerialized] public Player myPlayer;

        #endregion

        #region Serialized
        [SerializeField] private string local_PlayerPrefab;
        [SerializeField] private string local_EnemyPrefab;
        [SerializeField] GameObject[] desativar;
        [SerializeField] GameObject[] ativar;
        [SerializeField] GameObject pahfinder;
        [SerializeField] InventoryPanel inventario;
        [SerializeField] float spawnRate;
        [SerializeField] PolygonCollider2D circle;
        [SerializeField] int maxZombies;
        //[SerializeField] Text teste;

        #endregion

        #region Private
        Vector2[] pos = new Vector2[148];
        #endregion

        void Awake()
        {

            if(!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(0);
            }

            foreach (GameObject d in desativar)
            {
                d.SetActive(false);
                Destroy(d);
            }

            foreach (GameObject a in ativar)
            {
                a.SetActive(true);
            }

            if(instancia != null)
            {
                Destroy(gameObject);
            }

            instancia = this;

        }

        void Start()
        {
            PhotonNetwork.RunRpcCoroutines = true;
            ChangePanel(true);
            CriarJogador();
            if (PhotonNetwork.IsMasterClient)
            {
                Instantiate(pahfinder);
                for(int i = 0; i < circle.points.Length; i++)
                {
                    pos[i] = circle.transform.TransformPoint(circle.points[i]);
                }
                InvokeRepeating("ZombieSpawn", 5, spawnRate);
            }

        }

        void CriarJogador()
        {
            myPlayer = PhotonNetwork.Instantiate(local_PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
            photonView.RPC("AtualizarPlayerList", RpcTarget.AllBuffered);

        }

        [PunRPC]
        public void AtualizarPlayerList() 
        {
            livePlayerList = GameObject.FindGameObjectsWithTag("Player");
            //teste.text = playerList.Length.ToString();    
        }

        public override void OnLeftRoom()
        {
            photonView.RPC("AtualizarPlayerList", RpcTarget.AllBuffered);
        }

        void ZombieSpawn()
        {
            if (zombiesInScene >= maxZombies)
                return;
            //Vector2 pos = circle.transform.TransformPoint(circle.points);
            PhotonNetwork.InstantiateRoomObject(local_EnemyPrefab, pos[Random.Range(0, pos.Length)], Quaternion.identity);
            zombiesInScene++;

        }

        public void TrocarArma() => myPlayer.TrocarArma();

        public void Reload() => myPlayer.AnimReload();

        public void Usar() => myPlayer.Usar();

        public void ChangePanel(bool movePanel)
        {
            panel[0].SetActive(movePanel);
            panel[1].SetActive(!movePanel);
            inventario.AtualizarInventario();
            
        }

    }
}

