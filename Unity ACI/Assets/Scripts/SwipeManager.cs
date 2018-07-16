using UnityEngine;
using System.Collections;

public enum SwipeDirection
{
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,

    ////Diagonal Swipe
    //LeftDown = 9,
    //LeftUp = 5,
    //RightDown = 10,
    //RightUp = 6,
}


public class SwipeManager : MonoBehaviour
{
    public static SwipeManager Instance;

    public SwipeDirection Dir { set; get; }

    private Vector3 touchPosition;
    private float swipeResistanceX = 50.0f;
    private float swipeResistanceY = 100.0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        Dir = SwipeDirection.None;

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 deltaSwipe = touchPosition - Input.mousePosition;

            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                //Swipe on X axis
                Dir |= (deltaSwipe.x < 0) ? SwipeDirection.Right : SwipeDirection.Left;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                //Swipe on Y axis
                Dir |= (deltaSwipe.y < 0) ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
    }

    public bool IsSwiping(SwipeDirection dir)
    {
        return (Dir & dir) == dir;
    }
}
