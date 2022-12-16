using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class GameHoldButton : GameButton, IGameHoldable
{
    HoldButton button;

    public GameHoldButton(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {

    }
    protected override void setButton()
    {
        this.button = transform.gameObject.AddComponent<HoldButton>();
        this.button.transition = Selectable.Transition.None;
        if (action != null) onClick(() => action.Invoke());
    }

    public new void onClick(UnityAction action)
    {
        button.setAction(action);
    }
    public void onClickDown(UnityAction action)
    {
        button.setActionDown(action);
    }
    public void onClickUp(UnityAction action)
    {
        button.setActionUp(action);
    }
    public void onClickHover(UnityAction actionEnter, UnityAction actionExit)
    {
        button.setActionHover(actionEnter, actionExit);
    }
    protected class HoldButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        UnityAction actionHold, actionDown, actionUp, actionHoverEnter, actionHoverExit;

        bool ready = false;
        bool readyDown = false;
        bool readyUp = false;
        bool readyHover = false;

        bool pressed = false;
        bool hovered = false;
        
        public void setAction(UnityAction action)
        {
            ready = action != null;
            this.actionHold = action;
        }
        public void setActionDown(UnityAction action)
        {
            readyDown = action != null;
            this.actionDown = action;
        }
        public void setActionUp(UnityAction action)
        {
            readyUp = action != null;
            this.actionUp = action;
        }
        public void setActionHover(UnityAction actionEnter, UnityAction actionExit)
        {
            readyHover = actionEnter != null && actionExit != null;
            this.actionHoverEnter = actionEnter;
            this.actionHoverExit = actionExit;
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
            if (readyDown) actionDown.Invoke();
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
            if (readyUp) actionUp.Invoke();
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            hovered = true;
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            hovered = false;
            if (readyHover) actionHoverExit.Invoke();
        }
        void Update()
        {
            if (pressed && ready)
            {
                actionHold.Invoke();
            }
            else if (hovered && ready)
            {
                actionHoverEnter.Invoke();
            }
        }
    }
}