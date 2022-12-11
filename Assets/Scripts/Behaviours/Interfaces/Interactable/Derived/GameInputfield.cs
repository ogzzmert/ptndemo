using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
public class GameInputfield : GameBar
{
    InputField field;

    public GameInputfield(World world, Transform transform, UnityEvent action) : base (world, transform, action)
    {
        this.image.raycastTarget = true;

        field = transform.gameObject.GetComponent<InputField>();
        field.transition = Selectable.Transition.None;
        Text t = new GameObject().GetComponent<Text>();
        t.supportRichText = false;
        t.transform.SetParent(transform);
        t.transform.localPosition = Vector3.zero;
        t.rectTransform.sizeDelta = Vector2.zero;
        t.font = TextureManager.getFont();
        t.fontSize = 0;
        field.textComponent = t;
        field.onValueChanged.AddListener((string s) => setText(s + "_"));
        field.onEndEdit.AddListener((string s) => setText(s));
    }
    public void initialize(string text, float size, int charLimit, UnityAction<string> action = null, InputField.ContentType ct = InputField.ContentType.Standard, type fieldType = type.header)
    {
        base.initialize(text, size, fieldType);
        field.characterLimit = charLimit;
        field.contentType = ct;
        onClick(action);
    }

    public void onClick(UnityAction<string> action)
    {
        // field.onSubmit.RemoveAllListeners();
        // if (action != null) field.onSubmit.AddListener(action);
    }
    public string getText() { return field.text; }
}