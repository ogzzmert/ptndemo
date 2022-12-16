using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollSubPanel : SubPanel, IBeginDragHandler, IDragHandler, IScrollHandler
{
    [SerializeField]
    private ScrollSubPanelContent scrollContent;

    [SerializeField]
    private float outOfBoundsThreshold;
    private ScrollRect scrollRect;
    private Vector2 lastDragPosition;
    private bool positiveDrag;

    Dictionary<EntityProductType, SubPanel> list = new Dictionary<EntityProductType, SubPanel>();

    public override void initialize<T>(World world, T parentPanel)
    {
        base.initialize(world, parentPanel);
        this.build();
    }

    private void build()
    {
        list.Clear();

        scrollContent.initialize<ScrollSubPanel>(world, this);

        foreach(EntityProductType ept in Enum.GetValues(typeof(EntityProductType)))
        {
            SubPanel item = world.handle<InterfaceHandler>()
                .bringSubPanel<SubPanel, ScrollSubPanelContent>
                    (
                        SubPanelType.ListItem,
                        scrollContent
                    );

            item.getInteractable<GameBar>(type.bar, "bar").setSprite(ResourceManager.load<Sprite>("UI/Entities/Product/" + ept.ToString()));
            GameButton button = item.getInteractable<GameButton>(type.button, "button");
            button.setText(ept.ToString());
            button.onClick(() => selectItem(ept));
        }

        scrollRect = GetComponent<ScrollRect>();
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;

        scrollContent.build();
    }
    private void selectItem(EntityProductType type)
    {
        Debug.Log(type.ToString());
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        lastDragPosition = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        positiveDrag = eventData.position.y > lastDragPosition.y;
        lastDragPosition = eventData.position;
    }
    public void OnScroll(PointerEventData eventData)
    {
        positiveDrag = eventData.scrollDelta.y > 0;
    }
    public void OnViewScroll()
    {
        HandleScroll();
    }
    private void HandleScroll()
    {
        int currItemIndex = !positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currItem = scrollRect.content.GetChild(currItemIndex);

        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = !positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform endItem = scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (positiveDrag)
        {
            newPos.y = endItem.position.y - scrollContent.ChildHeight * 1.33f;
        }
        else
        {
            newPos.y = endItem.position.y + scrollContent.ChildHeight * 1.33f;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }
    private bool ReachedThreshold(Transform item)
    {
        float posYThreshold = transform.position.y + scrollContent.Height * 0.5f + outOfBoundsThreshold;
        float negYThreshold = transform.position.y - scrollContent.Height * 0.5f - outOfBoundsThreshold;
        return positiveDrag ? item.position.y - scrollContent.ChildWidth * 0.5f > posYThreshold :
            item.position.y + scrollContent.ChildWidth * 0.5f < negYThreshold;
    }
}