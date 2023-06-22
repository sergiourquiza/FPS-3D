using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LifeControllerUI : MonoBehaviour
{
    [SerializeField]
    private Sprite[] imagenesVida;
    [SerializeField]
    private GameObject player;
    private Image imagen;
    
    void Start()
    {
        imagen = GetComponent<Image>();
        imagen.sprite = imagenesVida[0];
    }

    // Update is called once per frame
    void Update()
    {
        float vidaActual = player.GetComponent<PlayerController>().health;

        if (vidaActual >= 85 && vidaActual <= 100)
        {
            imagen.sprite = imagenesVida[0];
        }
        if (vidaActual >= 60 && vidaActual < 85)
        {
            imagen.sprite = imagenesVida[1];
        }
        if (vidaActual >=30  && vidaActual < 60)
        {
            imagen.sprite = imagenesVida[2];
        }
        if (vidaActual > 10 && vidaActual < 30)
        {
            imagen.sprite = imagenesVida[3];
        }
        if (vidaActual > 0 && vidaActual <= 10)
        {
            imagen.sprite = imagenesVida[4];
        }
    }
}
