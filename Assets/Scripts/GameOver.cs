using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Start or quit the game
/// </summary>
public class GameOver : MonoBehaviour
{
    private Button[] buttons;
    private GameObject title;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        title = GameObject.Find("Title");
        
        HideCanvas();
    }

    public void HideCanvas()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(false);
        }
        title.SetActive(false);
    }

    public void ShowCanvas()
    {
        foreach (var b in buttons)
        {
            b.gameObject.SetActive(true);
        }
        title.SetActive(true);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level01");
    }
}