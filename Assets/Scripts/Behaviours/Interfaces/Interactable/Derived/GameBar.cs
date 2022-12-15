using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class GameBar : GameInteractable
{
    protected type barType;
    protected Text textComponent;

    public enum type
    {
        basic = 0,
        header = 1,
        bar = 2

    }

    public GameBar(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {
        setOpacity(255);
        this.image.raycastTarget = false;
        this.textComponent = this.transform.GetComponentInChildren<Text>();
    }
    public void initialize(string text, float size = 1.5f, type barType = type.header)
    {
        build(barType, text, size, true);
    }
    protected virtual void build(type barType, string text, float size, bool apply)
    {
        this.barType = barType;
        this.size = size;
        setText(text);
    }
    public void setSprite(Sprite sprite)
    {
        if (this.image != null) this.image.sprite = sprite;
    }
    public void setText(string newText)
    {
        if (this.textComponent != null) this.textComponent.text = newText;
    }
    public void changeType(Image.Type _type)
    {
        this.image.type = _type;

        if (_type == Image.Type.Filled)
        {
            this.image.fillOrigin = 2;
            this.image.fillClockwise = false;
        }
    }
    public void changeFillAmount(float value){ this.image.fillAmount = value; }

}