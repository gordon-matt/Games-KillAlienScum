// Copied from tutorial: https://www.youtube.com/playlist?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n

using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private static readonly GUISkin skin = Resources.Load<GUISkin>("GameSkin");
    private GUIContent content;
    private IFloatingTextPositioner positioner;

    public string Text
    {
        get { return content.text; }
        set { content.text = value; }
    }

    public GUIStyle Style { get; set; }

    public static FloatingText Show(string text, string style, IFloatingTextPositioner positioner)
    {
        var go = new GameObject("Floating Text");
        var floatingText = go.AddComponent<FloatingText>();
        floatingText.Style = skin.GetStyle(style);
        floatingText.positioner = positioner;
        floatingText.content = new GUIContent(text);
        return floatingText;
    }

    public void OnGUI()
    {
        var position = new Vector2();
        var contentSize = Style.CalcSize(content);

        // Get the position that the text should be displayed at
        //  However, if GetPosition() returns false, we will destory the object and exit
        if (!positioner.GetPosition(ref position, content, contentSize))
        {
            Destroy(gameObject);
            return;
        }

        GUI.Label(new Rect(position.x, position.y, contentSize.x, contentSize.y), content, Style);
    }
}