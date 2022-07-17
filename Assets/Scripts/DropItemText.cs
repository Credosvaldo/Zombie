using UnityEngine;
using UnityEngine.UI;

public class DropItemText : MonoBehaviour
{
    Text texto;
    [SerializeField] Slider slider;
    void Start()
    {
        texto = GetComponent<Text>();
    }

    void Update()
    {
        texto.text = slider.value.ToString();
    }
}
