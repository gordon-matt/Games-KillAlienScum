using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressBar : MonoBehaviour
{
    public Slider Slider;

    public Color Color = new Color(0.35f, 1f, 0.35f);

    private void Start()
    {
        if (Slider == null)
        {
            Slider = GetComponent<Slider>();
        }

        Slider.minValue = 0f;
        Slider.maxValue = 100f;

        transform.Find("Bar").GetComponent<Image>().color = Color;

        SetProgress(0f);
    }

    public void SetProgress(float amount)
    {
        Slider.value = amount;
    }
}