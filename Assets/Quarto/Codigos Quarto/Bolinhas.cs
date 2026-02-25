using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolinhas : MonoBehaviour
{
    private bool arrastando = false;
    public Transform caixa;           // arraste a Caixa no Inspector
    public Sprite[] spritesCaixa;     // sprites diferentes da caixa
    private static int contador = 0;  // quantas vezes já mudou
    private static SpriteRenderer caixaSR;

    void Start()
    {
        if (caixaSR == null) // pega o sprite da caixa uma única vez
        {
            caixaSR = caixa.GetComponent<SpriteRenderer>();
        }
    }

    private void OnMouseDown()
    {
        arrastando = true;
    }

    private void OnMouseDrag()
    {
        if (arrastando)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    private void OnMouseUp()
    {
        arrastando = false;

        // Se a bolinha foi solta perto da caixa
        if (Vector2.Distance(transform.position, caixa.position) < 1f)
        {
            TrocarSpriteCaixa();
            Destroy(gameObject); // some com a bolinha
        }
    }

    void TrocarSpriteCaixa()
    {
        if (contador < spritesCaixa.Length)
        {
            caixaSR.sprite = spritesCaixa[contador];
            contador++;
            Debug.Log("Caixa mudou para sprite " + contador);
        }
    }
}