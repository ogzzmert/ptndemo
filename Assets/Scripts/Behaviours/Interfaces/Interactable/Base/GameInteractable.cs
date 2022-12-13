using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class GameInteractable
{
    protected World world;
    protected RectTransform transform;
    protected UnityEvent action;
    protected Image image {get; private set;}
    protected Color color;
    protected float size;

    public GameInteractable(World world, Transform transform, UnityEvent action)
    {
        this.world = world;
        this.action = action;
        this.image = transform.gameObject.GetComponent<Image>();
        this.transform = image.rectTransform;
        this.color = new Color(1, 1, 1, 1);
    }
    public void setColor(int red, int green, int blue)
    {
        this.color.r = red / 255f;
        this.color.g = green / 255f;
        this.color.b = blue / 255f;
        refreshColor();
    }
    public void setOpacity(int opacity)
    {
        float next = opacity / 255f;

        if (next != this.color.a)
        {
            this.color.a = next;
            refreshColor();
        }
    }
    public void setAbsoluteOpacity(int opacity)
    {
        float next = opacity / 255f;

        if (next != this.color.a)
        {
            this.color.a = next;
            refreshColor();
        }
    }
    void refreshColor()
    {
        this.image.color = this.color;
    }
    public void setSize(int rawSize)
    {
        this.size = rawSize * 0.1f;
        transform.sizeDelta = new Vector2(image.sprite.texture.width * this.size, image.sprite.texture.height * this.size);
    }
    public void setPosition(Vector3 pose, bool isLocal = false)
    {   
        if (isLocal) transform.localPosition = pose;
        else transform.position = pose;
    }
    public void setRotation(float angle)
    {   
        transform.localEulerAngles = new Vector3(0, 0, angle);
    }
    public void Rotate(float angle)
    {   
        setRotation(angle + transform.localEulerAngles.z);
    }
    public void setPivot(float x, float y)
    {
        transform.pivot = new Vector2(x, y);
    }
    public void setParent(GameInteractable i)
    {
        this.transform.SetParent(i.transform);
        this.transform.localScale = new Vector2(1, 1);
    }
    public void setParent(Panel i)
    {
        this.transform.SetParent(i.transform);
        this.transform.localScale = new Vector2(1, 1);
    }
    public Vector2 getPosition(bool isLocal = false) 
    { 
        if (isLocal) return this.transform.localPosition;
        else return this.transform.position; 
    }
    public float getRotation()
    {   
        return transform.localEulerAngles.z;
    }
    public float getWidth() { return image.rectTransform.sizeDelta.x; }
    public float getHeight() { return image.rectTransform.sizeDelta.y; }
    public void enable(bool condition) { transform.gameObject.SetActive(condition); }
    public void discard() { world.destroy(this.transform.gameObject); }
}