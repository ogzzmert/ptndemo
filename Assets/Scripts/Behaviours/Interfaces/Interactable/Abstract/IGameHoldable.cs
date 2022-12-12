using UnityEngine;
using UnityEngine.Events;

public interface IGameHoldable
{
    public void onClickDown(UnityAction action);
    public void onClickUp(UnityAction action);
}