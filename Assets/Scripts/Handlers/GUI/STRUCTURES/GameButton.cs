using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class GameButton : GameBar
{
    Button button;

    public GameButton(Seed world, Transform transform) : base (world, transform)
    {
        this.image.raycastTarget = true;
        setButton();
    }
    protected virtual void setButton()
    {
        this.button = this.transform.gameObject.AddComponent<Button>();
        this.button.transition = Selectable.Transition.None;
    }
    public void initialize(string text, float size, UnityAction action = null, type buttonType = type.basic)
    {
        base.initialize(text, size, buttonType);
        onClick(action);
    }

    public virtual void onClick(UnityAction action)
    {
        button.onClick.RemoveAllListeners();

        if (action != null)
        {
            button.onClick.AddListener(action);
        }
    }
}