using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class GameBar : GameInteractable
{
    protected type barType;
    protected string text;

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
    }
    public void initialize(string text, float size = 1.5f, type barType = type.header)
    {
        build(barType, text, size, true);
    }
    public void initialize(Sprite sprite, float size = 1.5f)
    {
        this.size = size;
        this.barType = type.bar;

        this.image.sprite = sprite;
        image.rectTransform.sizeDelta = new Vector2(this.image.sprite.texture.width * this.size, this.image.sprite.texture.height * this.size);
    }
    protected virtual void build(type barType, string text, float size, bool apply)
    {
        this.barType = barType;
        this.size = size;
        setText(text, apply);
    }
    public void setText(string newText, bool apply = true)
    {
        this.text = newText;
        /*
        TextureManager.setFramedImage(
            image, 
            this.text, 
            GameBar.frames[this.barType], 
            GameBar.fonts[this.barType], 
            true, 
            this.size, 
            TextureManager.basicColor,
            "",
            "",
            apply
        );
        */
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