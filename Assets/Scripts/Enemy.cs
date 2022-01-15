using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Photon.Pun;


namespace MyGame
{
    #region Enemy NavMesh
    /*
    public class Enemy : MonoBehaviour
    {
        #region Public

        #endregion

        #region Serialized
        [SerializeField] float vertical;
        [SerializeField] float horizontal;
        #endregion

        #region Private
        NavMeshAgent agent;
        Transform destino;
        #endregion
        Rigidbody2D rig;
        void Start()
        {
            rig = GetComponent<Rigidbody2D>();
            agent = GetComponent<NavMeshAgent>();

            agent.updateUpAxis = false;
            agent.updateRotation = false;

            InvokeRepeating("SetDestinaition", 0, 1);
        }

        void Update()
        {
            agent.SetDestination(destino.position);
        }

        void SetDestinaition()
        {
            float dist = Mathf.Infinity;
            foreach(GameObject player in GameManager.instancia.playerList)
            {
                if(dist >  Vector2.Distance(player.transform.position, transform.position))
                {
                    dist = Vector2.Distance(player.transform.position, transform.position);
                    destino = player.transform;
                }

            }


        }

    }
    
    */
    #endregion

    public class Enemy : Life
    {
        #region Public

        #endregion

        #region Serialized
        GameObject colisorDano;
        #endregion

        #region Private
        Transform destino;
        AIPath agent;
        Animator anim;
        float timepass;
        #endregion
        Rigidbody2D rig;
        protected override void Start()
        {
            base.Start();
            colisorDano = transform.GetChild(0).gameObject;
            colisorDano.SetActive(false);

            if (!photonView.IsMine)
                return;

            rig = GetComponent<Rigidbody2D>();
            agent = GetComponent<AIPath>();
            anim = GetComponent<Animator>();

            InvokeRepeating("SetDestinaition", 0, 1);
        }

        void Update()
        {
            if (!photonView.IsMine)
                return;

            anim.SetBool("isMoving", agent.velocity != Vector3.zero);

            if(destino==null)
            {
                return;
            }

            agent.destination = destino.position;

            bool perto = Vector2.Distance(transform.position, destino.position) < 1;

            if(perto && timepass < Time.time)
            {
                anim.SetTrigger("Attack");
                timepass = Time.time + .5f;
            }

        }

        void SetDestinaition()
        {
            float dist = Mathf.Infinity;
            foreach(GameObject player in GameManager.instancia.livePlayerList)
            {
                if(dist > Vector2.Distance(player.transform.position, transform.position))
                {
                    dist = Vector2.Distance(player.transform.position, transform.position);
                    destino = player.transform;
                }

            }


        }

        void AnimAct(string function)
        {
            if (!photonView.IsMine)
                return;

            photonView.RPC("PreColisorDano", RpcTarget.AllViaServer);
        }

        [PunRPC]
        void PreColisorDano()
        {
            StartCoroutine("ColisorDano");
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
        }

        [PunRPC]
        IEnumerator ColisorDano()
        {
            colisorDano.SetActive(true);
            yield return new WaitForSeconds(.1f);
            colisorDano.SetActive(false);
        }


    }
}

