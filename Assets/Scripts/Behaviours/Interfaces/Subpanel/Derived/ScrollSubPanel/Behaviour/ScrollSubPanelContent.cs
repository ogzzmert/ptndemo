using UnityEngine;

public class ScrollSubPanelContent : MonoBehaviour
{
    public float Width { get { return width; } }
    public float Height { get { return height; } }
    public float ChildWidth { get { return childWidth; } }
    public float ChildHeight { get { return childHeight; } }


    private RectTransform rectTransform;
    private RectTransform[] rtChildren;
    private float width, height;
    private float childWidth, childHeight;

    public void build()
    {
        rectTransform = GetComponent<RectTransform>();
        rtChildren = new RectTransform[rectTransform.childCount];

        for (int i = 0; i < rectTransform.childCount; i++)
        {
            rtChildren[i] = rectTransform.GetChild(i) as RectTransform;
        }
        
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        childWidth = rtChildren[0].rect.width;
        childHeight = rtChildren[0].rect.height;

        setContent();

    }
    private void setContent()
    {
        float originY = height * 0.5f;
        float posOffset = childHeight * 0.5f;
        for (int i = 0; i < rtChildren.Length; i++)
        {
            Vector2 childPos = rtChildren[i].localPosition;
            childPos.y = originY - posOffset - i * childHeight;
            rtChildren[i].localPosition = childPos;
        }
    }
}