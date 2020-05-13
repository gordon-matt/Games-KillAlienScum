// Copied from tutorial: https://www.youtube.com/playlist?list=PLt_Y3Hw1v3QSFdh-evJbfkxCK_bjUD37n

using UnityEngine;

public interface IFloatingTextPositioner
{
    bool GetPosition(ref Vector2 position, GUIContent content, Vector2 size);
}