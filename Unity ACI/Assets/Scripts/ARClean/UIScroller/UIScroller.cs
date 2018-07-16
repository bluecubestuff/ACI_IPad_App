/* 
 * Author: Lim Rui An Ryan
 * Filename: UIScroller.cs
 * Description: The UIScroller is used for the tool inventory to have a scrollable interface for the user to be able to view all his/her tools.
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIScroller : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	private ScrollRect ScrollingRect;
	private RectTransform SRTransform;
	private RectTransform PageContainer;
	
	// Container Data
	private int PageCount;
	// Page Positions
	private List<Vector2> PagePositionList = new List<Vector2>();
	
	// Lerping Info
	private bool IsLerping;
	private Vector2 LerpingTarget;
	private bool IsDragging;

	// Settings
	public int CurrentPage = 0;
	public bool HorizontalScrollingEnabled = true;
	public int StartingPage = 0;
	public float RateOfDeceleration = 10f;
    public bool AutoInitiallize = false;

    private void Start()
    {
        if (AutoInitiallize)
            Initiallize();
    }

    // Update is called once per frame
    void Update()
	{
		if (PageCount > 0 && IsLerping){
			float Deceleration = Mathf.Min(RateOfDeceleration * Time.deltaTime, 1f);
			PageContainer.anchoredPosition = Vector2.Lerp(PageContainer.anchoredPosition, LerpingTarget, Deceleration);
			
			// Stop the lerp and snap to a target if below a certain threshold
			if (Vector2.SqrMagnitude(PageContainer.anchoredPosition - LerpingTarget) < 0.05f){
				PageContainer.anchoredPosition = LerpingTarget;
				IsLerping = false;
				ScrollingRect.velocity = Vector2.zero;
			}
		}
	}

	public void Initiallize()
	{
		ScrollingRect = GetComponent<ScrollRect>();
		SRTransform = GetComponent<RectTransform>();
		PageContainer = ScrollingRect.content;
		PageCount = PageContainer.childCount;

		if (PageCount <= 0)
			return;

		if (HorizontalScrollingEnabled == true)
		{
			ScrollingRect.horizontal = true;
			ScrollingRect.vertical = false;
		}
		else
		{
			ScrollingRect.horizontal = false;
			ScrollingRect.vertical = true;
		}

		IsLerping = false;
		SetPagePositions();
		SetPage(StartingPage);
	}

	private void SetPagePositions()
	{
		Vector2 SRPixelDimensions = new Vector2();
		Vector2 CentralizationOffset = new Vector2();
		Vector2 ContainerDimensions = new Vector2();

		if (HorizontalScrollingEnabled){
			SRPixelDimensions.x = SRTransform.rect.width;
			CentralizationOffset.x = SRPixelDimensions.x / 2;
			ContainerDimensions.x = SRPixelDimensions.x * PageCount;
		}
		else {
			SRPixelDimensions.y = SRTransform.rect.height;
			CentralizationOffset.y = SRPixelDimensions.y / 2;
			ContainerDimensions.y = SRPixelDimensions.y * PageCount;
		}

		// Resize and move the container accordingly
		Vector2 NewSize = new Vector2(ContainerDimensions.x, ContainerDimensions.y);
		Vector2 NewPosition = new Vector2(ContainerDimensions.x / 2, ContainerDimensions.y / 2);
		PageContainer.sizeDelta = NewSize;
		PageContainer.anchoredPosition = NewPosition;
		PagePositionList.Clear();

		// Iterate and set the positions accordingly
		for (int i = 0; i < PageCount; i++){
			RectTransform CurrentChild = PageContainer.GetChild(i).GetComponent<RectTransform>();
			Vector2 ChildPosition;
			if (HorizontalScrollingEnabled)
				ChildPosition = new Vector2(i * SRPixelDimensions.x - ContainerDimensions.x / 2 + CentralizationOffset.x, 0f);
			else ChildPosition = new Vector2(0f, -(i * SRPixelDimensions.y - ContainerDimensions.y / 2 + CentralizationOffset.y));
			CurrentChild.anchoredPosition = ChildPosition;
			PagePositionList.Add(-ChildPosition);
		}
	}

	public void SetPage(int PageIndex)
	{
		PageIndex = Mathf.Clamp(PageIndex, 0, PageCount - 1);
		PageContainer.anchoredPosition = PagePositionList[PageIndex];
		CurrentPage = PageIndex;
	}

	public void LerpToPage(int PageIndex)
	{
		PageIndex = Mathf.Clamp(PageIndex, 0, PageCount - 1);
		LerpingTarget = PagePositionList[PageIndex];
		IsLerping = true;
		CurrentPage = PageIndex;
	}

	private void NextPage()
	{
		LerpToPage(CurrentPage + 1);
	}

	private void PreviousPage()
	{
		LerpToPage(CurrentPage - 1);
	}

	private int GetNearestPage()
	{
		// Through comparing the distance between pages, find the closest
		Vector2 CurrentPosition = PageContainer.anchoredPosition;
		float ClosestDistance = float.MaxValue;
		int NearestPage = CurrentPage;
		for (int i = 0; i < PagePositionList.Count; i++){
			float CurrentDistance = Vector2.SqrMagnitude(CurrentPosition - PagePositionList[i]);
			if (CurrentDistance < ClosestDistance){
				ClosestDistance = CurrentDistance;
				NearestPage = i;
			}
		}
		return NearestPage;
	}

	public void OnBeginDrag(PointerEventData EventData)
	{
		// Reinit the variables
		IsLerping = false;
		IsDragging = false;
        ARCleanDataStore.PlayerInputFlagReset = false;
    }

	public void OnEndDrag(PointerEventData EventData)
	{
		LerpToPage(GetNearestPage());
		IsDragging = false;
	}

	public void OnDrag(PointerEventData EventData)
	{
		if (!IsDragging){
			IsDragging = true;
		}
	}
}