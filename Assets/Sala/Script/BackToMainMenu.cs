using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    // Método público para ligar no botão (Button -> OnClick)
    public void LoadMainMenu()
    {
        string sceneName = "MainMenu";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Cena '{sceneName}' não está nas Build Settings. Adicione-a em File > Build Settings.");
            if (SceneManager.sceneCountInBuildSettings > 0)
                SceneManager.LoadScene(0); // fallback para a cena 0, se existir
        }
    }
}
