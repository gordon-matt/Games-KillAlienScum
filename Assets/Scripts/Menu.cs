using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    //public void OnGUI()
    //{
    //    const int buttonWidth = 84;
    //    const int buttonHeight = 60;

    //    bool pressed = GUI.Button(
    //        new Rect(
    //            Screen.width / 2 - (buttonWidth / 2),
    //            (2 * Screen.height / 3) - (buttonHeight / 2),
    //            buttonWidth, buttonHeight),
    //        "Start!");

    //    if (pressed)
    //    {
    //        SceneManager.LoadScene("Level01");
    //    }
    //}

    public void StartGame()
    {
        SceneManager.LoadScene("Level01");
    }
}