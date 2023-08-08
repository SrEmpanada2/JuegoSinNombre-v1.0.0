using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesScript : MonoBehaviour
{

    public void PlayScene() {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame() {
        Application.Quit();
        Debug.Log("Salir...");
    }

    public void Empanada() {
        Application.OpenURL("https://twitter.com/SrEmpanada2?t=OS9obOlveZnJ-ZwdgpdKag&s=09");
    }
}
