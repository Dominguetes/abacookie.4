using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botões : MonoBehaviour
{

    public void ApertouBolinhas()
    {
        SceneManager.LoadScene("bolinhas");
    }
    public void ApertourRoleta()
    {
        SceneManager.LoadScene("Roleta");
    }
    public void ApertourAlfabeto()
    {
        SceneManager.LoadScene("Alfabeto");
    }
    public void AVoltar()
    {
        SceneManager.LoadScene("Inicio");
    }
    public void ApertouVoltarMenu()
    {
        SceneManager.LoadScene("Inicio");
    }

}
