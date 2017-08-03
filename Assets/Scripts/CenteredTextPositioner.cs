using UnityEngine;

public class CenteredTextPositioner : IFloatingTextPositioner
{
    private readonly float speed;
    private float textPosition;

    public CenteredTextPositioner(float speed)
    {
        this.speed = speed;
    }

    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        textPosition += Time.deltaTime * speed;

        // textPosition will be a number between 0 and 1 indicating how far along the text is
        if (textPosition > 1)
        {
            return false;
        }

        position = new Vector2(
            Screen.width / 2f - size.x / 2f,
            Mathf.Lerp(Screen.height / 2f + size.y, 0, textPosition));

        return true;
    }
}