using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
public class GameLabel : GameBar
{
    LabelButton button;
    int distance = 1000;
    float multiplier = 0.05f;

    public GameLabel(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {
        setButton();
        this.image.enabled = false;
    }
    protected virtual void setButton()
    {
        this.button = this.transform.gameObject.GetComponent<LabelButton>();
        this.button.transition = Selectable.Transition.None;
        this.button.initialize(this.world, this);
    }
    public void initialize(string text, float size, Transform baseObject, UnityAction action = null, type buttonType = type.basic)
    {
        base.initialize(text, size, buttonType);
        this.button.setBaseObject(baseObject);
        onClick(action);
    }
    public void onClick(UnityAction action)
    {
        this.image.raycastTarget = action != null;
        button.setAction(action);
    }
    protected class LabelButton : Button, IPointerDownHandler, IPointerUpHandler
    {
        World world;
        GameLabel label;
        float distance;
        int opacity;
        bool isActive = false;

        UnityAction action;
        bool ready = false;
        bool set = false;
        DateTime down;

        Camera cam;
        Transform target, baseObject;
        RectTransform rect;

        public void initialize(World world, GameLabel label)
        {
            this.world = world;
            this.label = label;
            this.rect = GetComponent<RectTransform>();
            StartCoroutine(setTransform());
        }
        public void setBaseObject(Transform baseObject)
        {
            this.baseObject = baseObject;
        }
        public void setAction(UnityAction action)
        {
            ready = action != null;
            this.action = action;
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            down = DateTime.Now;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (ready && isActive && opacity > 125 && world.isReady() && (DateTime.Now - down).TotalMilliseconds < 300)
            {
                this.action.Invoke();
            }
        }
        IEnumerator setTransform()
        {
            yield return new WaitUntil(() => world.isReady());
            cam = Camera.main;
            target = cam.transform.parent.parent;
            set = true;
        }
        void LateUpdate()
        {
            if (set && world.isReady())
            {
                distance = Calculator.getDistance(baseObject.position, target.position);
                if (isActive != distance < label.distance)
                {
                    isActive = !isActive;
                    label.image.enabled = isActive; 
                }
                if (isActive) 
                {
                    rect.position = cam.WorldToScreenPoint(baseObject.position);
                    opacity = 255 - (int)(distance * label.multiplier);
                    label.setAbsoluteOpacity(opacity); 
                }
            }
        }
    }
}