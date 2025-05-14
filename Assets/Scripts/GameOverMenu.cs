using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void TryAgain()
    {
        SceneManager.LoadScene("Menu"); // Substitua "Menu" pelo nome exato da sua cena do menu
    }
}
