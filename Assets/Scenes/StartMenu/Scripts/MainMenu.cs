using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public void StartGame () {
        SceneManager.LoadScene("Level1.1");
    }

    public void Quit () {
        Application.Quit();
    }
}
