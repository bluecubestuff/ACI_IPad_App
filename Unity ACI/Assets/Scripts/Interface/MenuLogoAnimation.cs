using UnityEngine;

public class MenuLogoAnimation : MonoBehaviour
{
    /// Public Variables
    public float Speed = 10f;
    public float MovementRange = 0.05f;

    /// Private Variables 
    private RectTransform RectTrans;
    private float IncrementValue = 0f;
    private float Timer = 0f;
    private float StartingY = 0f;

    private void Awake()
    {
        RectTrans = GetComponent<RectTransform>();
        IncrementValue = Screen.height * MovementRange * 0.5f;
        StartingY = RectTrans.anchoredPosition.y;
    }

    private void Update()
    {
        Timer += Speed * Time.deltaTime;
        RectTrans.anchoredPosition = new Vector2(RectTrans.anchoredPosition.x, StartingY + IncrementValue * Mathf.Sin((((Timer) % 360f))));
    }
}
