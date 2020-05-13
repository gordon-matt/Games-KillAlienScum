// Gained this script from doing this tutorial: https://pixelnest.io/tutorials/2d-game-unity/ and made some changes.

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scrolling : MonoBehaviour
{
    private List<Transform> backgroundPart;
    private Vector2 repeatableSize;
    private Vector3 gap = Vector3.zero;

    public Vector2 Speed = new Vector2(10, 10);

    public Vector2 Direction = Vector2.left;

    public bool IsLinkedToCamera = false;

    public bool IsLooping = false;

    public int GapOffsetX = 0;  // Fix for particle systems (Nebulae in this case)

    private void Start()
    {
        // For infinite background only
        if (IsLooping)
        {
            //---------------------------------------------------------------------------------
            // 1 - Retrieve background objects
            // -- We need to know what this background is made of
            // -- Store a reference of each object
            // -- Order those items in the order of the scrolling, so we know the item that will be the first to be recycled
            // -- Compute the relative position between each part before they start moving
            //---------------------------------------------------------------------------------

            // Get all part of the layer
            backgroundPart = new List<Transform>();

            for (int i = 0; i < transform.childCount; i++)
            {
                var childTransform = transform.GetChild(i);

                // Only visible children
                if (childTransform.GetComponent<Renderer>() != null)
                {
                    backgroundPart.Add(childTransform);

                    // First element
                    if (backgroundPart.Count == 1)
                    {
                        // Gap is the space between zero and the first element.
                        // We need it when we loop.
                        gap = childTransform.transform.position;

                        if (Direction.x == 0)
                        {
                            gap.x = 0;
                        }
                        if (Direction.y == 0)
                        {
                            gap.y = 0;
                        }
                    }
                }
            }

            if (backgroundPart.Count == 0)
            {
                Debug.LogError("Nothing to scroll!");
            }

            // Sort by position
            // -- Depends on the scrolling direction
            backgroundPart = backgroundPart.OrderBy(t => t.position.x * (-1 * Direction.x)).ThenBy(t => t.position.y * (-1 * Direction.y)).ToList();

            // Get the size of the repeatable parts
            var first = backgroundPart.First();
            var last = backgroundPart.Last();

            repeatableSize = new Vector2(
              Mathf.Abs(last.position.x - first.position.x),
              Mathf.Abs(last.position.y - first.position.y));

            gap.x += GapOffsetX; // Fix for particle systems
        }
    }

    private void Update()
    {
        // Movement
        var movement = new Vector3(Speed.x * Direction.x, Speed.y * Direction.y, 0);

        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Move the camera
        if (IsLinkedToCamera)
        {
            Camera.main.transform.Translate(movement);
        }

        // Loop
        if (IsLooping)
        {
            //---------------------------------------------------------------------------------
            // 2 - Check if the object is before, in or after the camera bounds
            //---------------------------------------------------------------------------------

            // Camera borders
            float distance = (transform.position - Camera.main.transform.position).z;
            float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
            float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
            // float width = Mathf.Abs(rightBorder - leftBorder);

            float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y;
            float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y;
            // float height = Mathf.Abs(topBorder - bottomBorder);

            // Determine entry and exit border using direction
            var exitBorder = Vector3.zero;
            var entryBorder = Vector3.zero;

            if (Direction.x < 0)
            {
                exitBorder.x = leftBorder;
                entryBorder.x = rightBorder;
            }
            else if (Direction.x > 0)
            {
                exitBorder.x = rightBorder;
                entryBorder.x = leftBorder;
            }

            if (Direction.y < 0)
            {
                exitBorder.y = bottomBorder;
                entryBorder.y = topBorder;
            }
            else if (Direction.y > 0)
            {
                exitBorder.y = topBorder;
                entryBorder.y = bottomBorder;
            }

            // Get the first object
            var firstChild = backgroundPart.FirstOrDefault();

            if (firstChild != null)
            {
                bool checkVisible = false;

                // Check if we are after the camera
                // The check is on the position first as IsVisibleFrom is a heavy method
                // Here again, we check the border depending on the direction
                if (Direction.x != 0)
                {
                    if ((Direction.x < 0 && (firstChild.position.x < exitBorder.x)) ||
                        (Direction.x > 0 && (firstChild.position.x > exitBorder.x)))
                    {
                        checkVisible = true;
                    }
                }
                if (Direction.y != 0)
                {
                    if ((Direction.y < 0 && (firstChild.position.y < exitBorder.y)) ||
                        (Direction.y > 0 && (firstChild.position.y > exitBorder.y)))
                    {
                        checkVisible = true;
                    }
                }

                // Check if the sprite is really visible on the camera or not
                if (checkVisible)
                {
                    //---------------------------------------------------------------------------------
                    // 3 - The object was in the camera bounds but isn't anymore.
                    // -- We need to recycle it
                    // -- That means he was the first, he's now the last
                    // -- And we physically moves him to the further position possible
                    //---------------------------------------------------------------------------------

                    if (!firstChild.GetComponent<Renderer>().IsVisibleFrom(Camera.main))
                    {
                        // Set position in the end
                        firstChild.position = gap + new Vector3(
                            firstChild.position.x + ((repeatableSize.x + firstChild.GetComponent<Renderer>().bounds.size.x) * -1 * Direction.x),
                            firstChild.position.y + ((repeatableSize.y + firstChild.GetComponent<Renderer>().bounds.size.y) * -1 * Direction.y),
                            firstChild.position.z);

                        // The first part become the last one
                        backgroundPart.Remove(firstChild);
                        backgroundPart.Add(firstChild);
                    }
                }
            }
        }
    }
}