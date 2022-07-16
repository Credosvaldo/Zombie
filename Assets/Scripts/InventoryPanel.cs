using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MyGame
{
    public class InventoryPanel : MonoBehaviour
    {
        private Player myPlayer;

        [SerializeField] private Slot[] slots;
        [SerializeField] private ExampleSlot exampleSlot;
        [SerializeField] private GameObject[] botoes;
        [SerializeField] private Pair[] gunSprite;

        [SerializeField] private Image[] slotGuns;
        [SerializeField] private Text[] slotBullets;
        [SerializeField] private Text[] slotOthers;


        public void AtualizarInventario()
        {
            //Guns
            for(int i = 0; i < 3; i++)
            {
                slotGuns[i].sprite = GameManager.instancia.myPlayer.gun[(Equipped)(i+1)].have ? gunSprite[i].com : gunSprite[i].sem;
            }

            //Bullets
            for(int i = 0; i < 3; i++)
            {
                slotBullets[i].text = "x" + GameManager.instancia.myPlayer.gun[(Equipped)(i + 1)].allBullets;
            }

            //Others
            for (int i = 0; i < 3; i++)
            {
                slotOthers[i].text = "x" + GameManager.instancia.myPlayer.other[(Other)i];
            }
            

        }


        public void OnClick(int ind)
        {
            exampleSlot.description.text = slots[ind].description;
            exampleSlot.image.sprite = slots[ind].image;

            //Usar Dropar Sair
            botoes[0].SetActive(slots[ind].usavel);
            botoes[1].SetActive(slots[ind].dropavel);

        }

        public void OnClickDo(int ind)
        {
            switch (ind)
            {
                case 1://Usar
                    //colocar granada na mao //EXTRA: se ele paga a granada e volta pro inventario
                    //abrir o mapa
                    //usar kit medico
                    break;

                case 2://Dropar
                    //diminuir um item no player e no painel
                    //colocalo no chao em forma de item

                    //EXTRA: pegar item do chao
                    break;

                case 3://Sair
                    GameManager.instancia.ChangePanel(true);
                    break;

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

    }

    [System.Serializable]
    public class Pair
    {
        public Sprite sem;
        public Sprite com;
    }

}

