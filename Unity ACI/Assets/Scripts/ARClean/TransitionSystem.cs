/* 
 * Author: Lim Rui An Ryan
 * Filename: TransitionSystem.cs
 * Description: The ARClean game mode's transition system that is used to transit between scenes with a fading effect.
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionSystem : MonoBehaviour
{
	// Internals
	private int ZIndex = -1000;
	private float Alpha = 1.0f;
	private bool FadingIn = true;
	private bool TransitingScenes = false;
	private int TargetLevel;

	// Settings
	public Texture FadeEffectTexture;
	public float FadeSpeed = 1f;

	// Use this for initialization
	void Start()
	{
		TransitingScenes = false;
		TargetLevel = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (TransitingScenes)
		{
			if (Alpha >= 1)
			{
				TransitingScenes = false;
				SceneManager.LoadScene(TargetLevel);
			}
		}
	}

	void OnGUI()
	{
		// Fade in/out with alpha based on whether its FadingIn
		float FadingDirection;
		if (FadingIn) FadingDirection = -1;
		else FadingDirection = 1;
		Alpha += FadingDirection * FadeSpeed * Time.deltaTime;
		Alpha = Mathf.Clamp(Alpha, 0, 1);

		// Set GUI Color
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, Alpha);
		GUI.depth = ZIndex;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeEffectTexture, ScaleMode.StretchToFill);
	}

	public void CloseApplication()
	{
		Application.Quit(); 
	}

	public float BeginFade(bool FadeIn)
	{
		FadingIn = FadeIn;
		return FadeSpeed;
	}

	void OnLevelLoaded()
	{
		Alpha = 1;
		BeginFade(true);
	}

	float GetFadeTime()
	{
		return FadeSpeed;
	}

    public void ChangeScene(int Index)
    {
        if (!TransitingScenes)
        {
            BeginFade(false);
            TargetLevel = Index;
            TransitingScenes = true;
        }
    }

    public void IncrementScene()
	{
		ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
	}
    public void DecrementScene()
    {
        if (SceneManager.GetActiveScene().buildIndex - 1 >= 0)
            ChangeScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
