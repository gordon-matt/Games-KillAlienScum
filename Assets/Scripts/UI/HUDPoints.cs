using UnityEngine;
using UnityEngine.UI;

public class HUDPoints : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        text.text = $"Points: {GameManager.Instance.Points}";
    }
}