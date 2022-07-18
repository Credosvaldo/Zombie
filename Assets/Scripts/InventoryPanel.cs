using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace MyGame
{
    public enum Item
    {
        gun,
        bullets,
        other,
    }

    public class InventoryPanel : MonoBehaviour
    {
        public Slot[] slots;

        [SerializeField] private ExampleSlot exampleSlot;
        [SerializeField] private GameObject[] botoes;
        [SerializeField] private Pair[] gunSprite;
        [SerializeField] private GameObject drop_Panel;
        [SerializeField] private Slider drop_Slider;
        [SerializeField] private Image drop_Image;

        [SerializeField] private Image[] slotGuns;
        [SerializeField] private Text[] slotBullets;
        [SerializeField] private Text[] slotOthers;

        private Player myPlayer;

        public void MyPlayer(Player myPlayer)
        {
            this.myPlayer = myPlayer;
        }

        public void AtualizarInventario()
        {
            //Guns
            for (int i = 0; i < 3; i++)
            {
                slotGuns[i].sprite = myPlayer.gun[(Equipped)(i+1)].have ? gunSprite[i].com : gunSprite[i].sem;
            }

            //Bullets
            for(int i = 0; i < 3; i++)
            {
                slotBullets[i].text = "x" + myPlayer.gun[(Equipped)(i + 1)].allBullets;
            }

            //Others
            for (int i = 0; i < 3; i++)
            {
                slotOthers[i].text = "x" + myPlayer.other[(Other)i];
            }

            AtualizarBotaoDrop();

        }

        void AtualizarBotaoDrop()
        {
            if(exampleSlot.indice == 0)
            {
                botoes[1].SetActive(false);
                return;
            }

            bool have = HaveIt(exampleSlot.indice);

            botoes[1].SetActive(have);

            if(exampleSlot.indice > 6)
            {
                botoes[0].SetActive(have);
            }

        }

        public void OnClick(int ind)
        {
            exampleSlot.description.text = slots[ind].name + "\n\n" + slots[ind].description;
            exampleSlot.image.sprite = slots[ind].image;
            exampleSlot.indice = ind;

            //Usar Dropar Sair
            botoes[0].SetActive(slots[ind].usavel);

            AtualizarBotaoDrop();

        }

        public void OnClickDo(int ind)
        {
            int indice = exampleSlot.indice;

            switch (ind)
            {
                case 1://Usar
                    //colocar granada na mao //EXTRA: se ele paga a granada e volta pro inventario
                    //abrir o mapa
                    //usar kit medico
                    break;

                case 2://Dropar
                    if(indice < 4)//dropar a arma em si
                    {
                        myPlayer.gun[(Equipped)indice].have = false;
                        Drop(indice, 1);

                        if((int)myPlayer.equipped == indice)
                        {
                            myPlayer.TrocarArma();
                        }

                        break;
                    }

                    drop_Panel.SetActive(true);
                    drop_Image.sprite = exampleSlot.image.sprite;
                    
                    if(exampleSlot.indice < 7)
                    {
                        drop_Slider.maxValue = myPlayer.gun[(Equipped)(indice-3)].allBullets;
                        break;
                    }

                    drop_Slider.maxValue = myPlayer.other[(Other)(indice - 7)];
                    break;

                case 3://Sair
                    GameManager.instancia.ChangePanel(true);
                    break;

                case 21://Dropar em si
                    if(indice < 7)
                    {
                        myPlayer.gun[(Equipped)(indice - 3)].allBullets -= (int)drop_Slider.value;
                        Drop(indice, (int)drop_Slider.value);
                        drop_Panel.SetActive(false);
                        break;
                    }

                    myPlayer.other[(Other)(indice - 7)] -= (int)drop_Slider.value;
                    //falta jogar no chao
                    drop_Panel.SetActive(false);

                    break;

                case 22://Fechar painel de drop
                    drop_Panel.SetActive(false);
                    break;

            }

            AtualizarInventario();


        }

        void Drop(int indice, int quantidade)
        {
            PhotonNetwork.Instantiate("DropedItem", myPlayer.transform.position, Quaternion.identity).GetPhotonView().RPC("SetUp", RpcTarget.All, indice, quantidade, myPlayer.GetInstanceID());
        }

        bool HaveIt(int ind)
        {
            if (exampleSlot.indice < 4)//Guns
            {
                return myPlayer.gun[(Equipped)(exampleSlot.indice)].have;
            }
            else if (exampleSlot.indice < 7)//Bullets
            {
                return myPlayer.gun[(Equipped)(exampleSlot.indice - 3)].allBullets > 0;
            }
            else//Others
            {
                return myPlayer.other[(Other)(exampleSlot.indice - 7)] > 0;
            }

        }


    }


    [System.Serializable]
    public class Slot
    {
        public string name;
        public string description;
        public Sprite image;
        public bool usavel;
        public bool dropavel;

    }

    [System.Serializable]
    public class ExampleSlot
    {
        public TextMeshProUGUI description;
        public Image image;
        public int indice;

    }

    [System.Serializable]
    public class Pair
    {
        public Sprite sem;
        public Sprite com;
    }

}

