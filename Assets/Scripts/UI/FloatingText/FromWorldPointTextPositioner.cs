// Copied from tutorial: https://www.youtube.com/playlist?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n

using UnityEngine;

public class FromWorldPointTextPositioner : IFloatingTextPositioner
{
    private readonly Camera camera;
    private readonly float speed;
    private readonly Vector3 worldPosition;
    private float timeToLive;
    private float yOffset;

    public FromWorldPointTextPositioner(Camera camera, Vector3 worldPosition, float timeToLive, float speed)
    {
        this.camera = camera;
        this.speed = speed;
        this.timeToLive = timeToLive;
        this.worldPosition = worldPosition;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="position"></param>
    /// <param name="content"></param>
    /// <param name="size">Size of the text in pixels</param>
    /// <returns></returns>
    public bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size)
    {
        // Every frame will subtract Time.deltaTime from timeToLive and if that value ever becomes 0...
        if ((timeToLive - Time.deltaTime) <= 0)
        {
            // ...then we return false, which indicates to our floating text's OnGUI() method that it's time to destroy the floating text
            return false;
        }

        var screenPosition = camera.WorldToScreenPoint(worldPosition);

        // Divide by 2 to center the text position
        position.x = screenPosition.x - (size.x / 2);
        position.y = Screen.height - screenPosition.y - yOffset;

        yOffset += Time.deltaTime * speed;
        return true;
    }
}