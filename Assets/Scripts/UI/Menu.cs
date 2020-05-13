using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject NameField;

    public void StartGame()
    {
        string playerName = NameField.GetComponent<Text>().text;
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            GameManager.Instance.PlayerName = playerName;
        }

        SceneManager.LoadScene("GetReady");
    }
}