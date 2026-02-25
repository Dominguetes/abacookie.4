using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocarCena : MonoBehaviour
{
    // Método público e sem parâmetros -> fácil de vincular no Button OnClick
    public void TrocarMemoria()
    {
        SceneManager.LoadScene("JogoMemoria");
    }
}