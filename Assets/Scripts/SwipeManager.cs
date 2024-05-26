using UnityEngine;

public class SwipeManager : MonoBehaviour
{

    public static SwipeManager instance;

    public enum Direction { Left, Right, Up, Down };
    bool[] swipe = new bool[4];

    Vector2 startTouch;
    Vector2 swipeDelta;
    bool touchMoved;

    const float SWIPE_THRESHOLD = 15.0f;

    public delegate void MoveDelegate(bool[] swipes);
    public MoveDelegate MoveEvent;

    public delegate void ClickDelegate(Vector2 pos);
    public ClickDelegate ClickEvent;

    Vector2 TouchPosition() => (Vector2)Input.mousePosition;
    bool TouchBegan() => Input.GetMouseButtonDown(0);
    bool TouchEnded() => Input.GetMouseButtonUp(0);
    bool GetTouch() => Input.GetMouseButton(0);

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) Debug.Log("Up Arrow key was pressed");
        if (TouchBegan())
        {
            Debug.Log("method");
            startTouch = TouchPosition();
            touchMoved = true;
        } else if (TouchEnded() && touchMoved)
        {
            SendSwipe();
            touchMoved = false;
        }
        
        swipeDelta = Vector2.zero;
        if (touchMoved && GetTouch())
        {
            swipeDelta = TouchPosition() - startTouch;
        }


        if (swipeDelta.magnitude > SWIPE_THRESHOLD)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                swipe[(int)Direction.Left] = swipeDelta.x < 0;
                swipe[(int)Direction.Right] = swipeDelta.x > 0;
            }else
            {
                swipe[(int)Direction.Down] = swipeDelta.y < 0;
                swipe[(int)Direction.Up] = swipeDelta.y > 0;
            }
            SendSwipe();
        }

    }

    void SendSwipe()
    {
        if (swipe[0] || swipe[1] || swipe[2] || swipe[3] )
        {
            MoveEvent?.Invoke(swipe);
            Debug.Log(swipe[0] + "|" + swipe[1] + "|" + swipe[2] + "|" + swipe[3]);
        }
        else
        {
            ClickEvent?.Invoke(TouchPosition());
            Debug.Log("Click");
        }
        Reset();
    }

    private void Reset()
    {
        startTouch = swipeDelta  = Vector2.zero;
        touchMoved = false;
        for (int i = 0; i < 4; i++)
            swipe[i] = false;
    }

}
