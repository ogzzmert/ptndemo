using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
public class GameButton : GameBar
{
    Button button;

    public GameButton(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {
        this.image.raycastTarget = true;
        setButton();
        if (action != null) onClick(() => action.Invoke());
    }
    protected virtual void setButton()
    {
        this.button = this.transform.gameObject.GetComponent<Button>();
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