using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;



namespace MyGame
{

    public enum Equipped
    {
        knife,
        pistol,
        rifle,
        shotgun,
    }

    public enum Other
    {
        granade,
        map,
        medkit,
    }

    public enum State
    {
        stand,
        shooting,
        reloading,

    }

    [System.Serializable]
    public struct GunUI
    {
        public Image sprite;
        public Text ammo;
    }

    
    [System.Serializable]
    public class Gun
    {
        public int allBullets;
        public int maxInShell;
        public int inShell;
        public bool have;
    }


    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : Life
    {
        #region Public
        [System.NonSerialized]public Animator anim;

        public Dictionary<Equipped, Gun> gun = new Dictionary<Equipped, Gun> {
            { Equipped.knife, new Gun() },
            { Equipped.pistol, new Gun() },
            { Equipped.rifle, new Gun() },
            { Equipped.shotgun, new Gun() }
        };

        public Dictionary<Other, int> other = new Dictionary<Other, int>
        {
            {Other.granade, 0},
            {Other.map, 0},
            {Other.medkit, 0},
        };

        #endregion

        #region Serialized
        [SerializeField] float speed;
        [SerializeField] GameObject bullet;
        [SerializeField] Transform targetBullet;
        [SerializeField] GameObject meleeColider;
        [SerializeField] Image lifeBar;
        [SerializeField] float smoothLifeBar;
        [SerializeField] Sprite[] gunSprites;
        [SerializeField] LayerMask layerBox;

        #endregion

        #region Private
        FloatingJoystick movmentJoystick;
        FloatingJoystick shotterJoystick;
        Slider slider_lootBoxTime;
        Transform cam;
        Rigidbody2D rig;
        GameObject currentBox;
        Button botao_TrocarArma;
        Button botao_Reload;
        float timeToShotter;
        bool openingBox;
        GunUI selectedGun;
        Equipped equipped;
        State state;
        #endregion

        protected override void Start()
        {
            base.Start();
            if (!photonView.IsMine)
                return;

            rig = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();

            cam = GameManager.instancia.cam;
            movmentJoystick = GameManager.instancia.movmentJoystick;
            shotterJoystick = GameManager.instancia.shotterJoystick;
            slider_lootBoxTime = GameManager.instancia.slider_LootBoxTime;
            selectedGun = GameManager.instancia.selectedGun;
            botao_TrocarArma = GameManager.instancia.botao_TrocarArma;
            botao_Reload = GameManager.instancia.botao_Reload;
            selectedGun = GameManager.instancia.selectedGun;

            GunTemporaria();

            lifeBar.fillAmount = life;
            equipped = Equipped.knife;
            state = State.stand;
            //slider_lootBoxTime.value = 0;
            slider_lootBoxTime.gameObject.SetActive(false);

        }
        
        void GunTemporaria()
        {
            gun[Equipped.knife].have = true;
            gun[Equipped.pistol].have = true;
            gun[Equipped.rifle].have = true;
            gun[Equipped.shotgun].have = true;

            gun[Equipped.knife].allBullets = int.MaxValue;
            gun[Equipped.pistol].allBullets = 120;
            gun[Equipped.rifle].allBullets = 120;
            gun[Equipped.shotgun].allBullets = 120;

            gun[Equipped.knife].inShell = int.MaxValue;
            gun[Equipped.pistol].inShell = 0;
            gun[Equipped.rifle].inShell = 0;
            gun[Equipped.shotgun].inShell = 0;
            
            gun[Equipped.knife].maxInShell = int.MaxValue;
            gun[Equipped.pistol].maxInShell = 12;
            gun[Equipped.rifle].maxInShell = 30;
            gun[Equipped.shotgun].maxInShell = 6;

        }
        

        void Update()
        {
            if (!photonView.IsMine)
                return;

            AtualizarBarraDeVida();
            AtualizarGunUI();

            if (shotterJoystick.press && timeToShotter < Time.time && state == State.stand)
            {
                if(gun[equipped].inShell > 0)
                {
                    anim.SetTrigger("Shoot");
                    state = State.shooting;
                    timeToShotter = Time.time + .1f;
                }
                else
                {
                    if (gun[equipped].allBullets > 0)
                    {
                        anim.SetBool("Reload", true);
                        state = State.reloading;
                    }
                    else
                    {
                        TrocarArma();
                    }
                }

            }

        }

        void FixedUpdate()
        {
            if (!photonView.IsMine)
                return;
         
            Move();

        }

        void Move()
        {
            rig.MovePosition(rig.position + movmentJoystick.Direction * Time.fixedDeltaTime * speed);

            if(movmentJoystick.Direction != Vector2.zero)
            {
                transform.up = movmentJoystick.Direction.normalized;
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

            if(Mathf.Abs(shotterJoystick.Direction.x) > .35f || Mathf.Abs(shotterJoystick.Direction.y) > .35f)
            {
                transform.up = shotterJoystick.Direction.normalized;
            }

            cam.transform.position = transform.position + Vector3.forward * -50;

        }

        public void TrocarArma()
        {
            if (equipped != Equipped.shotgun)
            {
                equipped++;
            }
            else
            {
                equipped = Equipped.knife;
            }

            anim.SetBool("Reload", false);
            
            if (!gun[equipped].have)
                TrocarArma();
            

            anim.SetInteger("Equipped", (int)equipped);

        }

        public void Reload()
        {
            int diferenca = gun[equipped].maxInShell - gun[equipped].inShell;

            if (gun[equipped].allBullets >= diferenca)
            {
                gun[equipped].inShell = gun[equipped].maxInShell;
                gun[equipped].allBullets -= diferenca;
            }
            else
            {
                gun[equipped].inShell += gun[equipped].allBullets;
                gun[equipped].allBullets = 0;
            }

            EndOfAnimation();
        }

        public void AnimReload()
        {
            if(gun[equipped].inShell < gun[equipped].maxInShell)
            {
                anim.SetBool("Reload", true);
            }
            
        }

        void AtualizarBarraDeVida()
        {
            lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, life, smoothLifeBar); 
        }

        public void AnimAct(string function)
        {
            if (!photonView.IsMine)
                return;


        }
        
        public void AnimActRpc(string function)
        {
            if (!photonView.IsMine)
                return;

            photonView.RPC(function, RpcTarget.AllViaServer, equipped);

        }

        public void EndOfAnimation()
        {
            if (!photonView.IsMine)
                return;

            state = State.stand;
            anim.SetBool("Shoot", false);
            anim.SetBool("Reload", false);
            anim.SetBool("Melee", false);
            meleeColider.SetActive(false);


        }


        [PunRPC]
        void Shoot(Equipped equipped)
        {
            Bullet bala;
            switch(equipped)
            {
                case Equipped.knife:
                    meleeColider.SetActive(true);
                    break;
                case Equipped.pistol:
                    bala = Instantiate(bullet, targetBullet.position, transform.rotation).GetComponent<Bullet>();
                    bala.dano = 10;
                    break;

                case Equipped.rifle:
                    bala = Instantiate(bullet, targetBullet.position, transform.rotation).GetComponent<Bullet>();
                    bala.dano = 5;
                    break;

                case Equipped.shotgun:
                    bala = Instantiate(bullet, targetBullet.position, transform.rotation).GetComponent<Bullet>();
                    bala.dano = 8;
                    break;

            }

            gun[equipped].inShell--;


        }

        void AtualizarGunUI()
        {
            selectedGun.ammo.text = gun[equipped].inShell + "/" + gun[equipped].allBullets;
            selectedGun.sprite.sprite = gunSprites[(int)equipped];

            if (equipped == Equipped.knife) selectedGun.ammo.text = "∞";

        }

        public void Usar()
        {
            if (currentBox != null && !openingBox)
            {
                StartCoroutine("LootBoxTime");
            }

        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (!photonView.IsMine)
                return;

            if (collision.CompareTag("Box"))
            {
                RaycastHit2D hit = Physics2D.Linecast(transform.position, collision.transform.position, ~layerBox);
                if(hit.collider == null)
                {
                    currentBox = currentBox == null ? collision.gameObject : currentBox = Vector2.Distance(currentBox.transform.position, transform.position) > Vector2.Distance(collision.transform.position, transform.position) ? collision.gameObject : currentBox;
                }
                else
                {
                    currentBox = null;
                    openingBox = false;
                }

                

            }

            if(collision == null)
            {
                currentBox = null;
                openingBox = false;
            }


        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Box"))
            {
                currentBox = null;
            }
        }

        void LootBox()
        {
            Destroy(currentBox);
        }


        [PunRPC]
        IEnumerator Melee()
        {
            Debug.Log("Melee foi executado");
            meleeColider.SetActive(true);
            yield return new WaitForSeconds(.1f);
            meleeColider.SetActive(false);

        }

        IEnumerator LootBoxTime()
        {
            slider_lootBoxTime.gameObject.SetActive(true);
            slider_lootBoxTime.value = 0;
            openingBox = true;


            while (slider_lootBoxTime.value < 1)
            {
                if (openingBox == false)
                {
                    yield break;
                }

                yield return new WaitForSeconds(0.05f);
                slider_lootBoxTime.value += 0.05f;
            }

            slider_lootBoxTime.gameObject.SetActive(false);
            openingBox = false;
            LootBox();

        }


    }

}

