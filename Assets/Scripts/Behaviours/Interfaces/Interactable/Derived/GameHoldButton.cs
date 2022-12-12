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
        this.button = transform.gameObject.GetComponent<HoldButton>();
        this.button.transition = Selectable.Transition.None;
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
    protected class HoldButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        UnityAction action, actionDown, actionUp;

        bool ready = false;
        bool readyDown = false;
        bool readyUp = false;
        bool pressed = false;
        
        public void setAction(UnityAction action)
        {
            ready = action != null;
            this.action = action;
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
        void Update()
        {
            if (pressed && ready)
            {
                this.action.Invoke();
            }
        }
    }
}