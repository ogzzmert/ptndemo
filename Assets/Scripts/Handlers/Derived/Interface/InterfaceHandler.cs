using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterfaceHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Canvas canvas;

    [Serializable]
    private class GameInterface
    {
        public WorldType type;
        public GameObject panel;
    } 

    Dictionary<WorldType, Pool> panels = new Dictionary<WorldType, Pool>();
    Dictionary<SubPanelType, Pool> subpanels = new Dictionary<SubPanelType, Pool>();

    Panel panel;

    protected override void initialize()
    {
        foreach(WorldType wt in Enum.GetValues(typeof(WorldType)))
        {
            string pname = wt.ToString();
            GameObject p = ResourceManager.load<GameObject>("Prefab/UI/Panel/" + pname);
            if (p != null)
            {
                panels.Add(wt, world.handle<PoolHandler>().poolify(p, 1));

            }
        }

        foreach(SubPanelType spt in Enum.GetValues(typeof(SubPanelType)))
        {
            string pname = spt.ToString();
            GameObject p = ResourceManager.load<GameObject>("Prefab/UI/Subpanel/" + pname);
            if (p != null)
            {
                subpanels.Add(spt, world.handle<PoolHandler>().poolify(p, 32));

            }
        }
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        if (panels.ContainsKey(worldType))
        {
            PoolObject pooledPanel = panels[worldType].bring();

            panel = pooledPanel.getObject().GetComponent<Panel>();
            
            panel.gameObject.transform.SetParent(this.canvas.gameObject.transform);
            panel.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            panel.initialize(world, worldIndex);
            panel.setAsPooled(pooledPanel);

            pooledPanel.wakeObject();
        }
    }
    public void degenerate()
    {
        if (panel != null && panel.getPooledObject().isAwake())
        {
            panel.discard();
            panel.getPooledObject().sendback();
            panel = null;
        }
    }
    public T bringSubPanel<T, V>(SubPanelType subpanel, V parentPanel) where T : SubPanel where V : Panel
    {
        if (subpanels.ContainsKey(subpanel))
        {
            PoolObject pooledPanel = subpanels[subpanel].bring();

            GameObject obj = pooledPanel.getObject();
            obj.gameObject.transform.SetParent(parentPanel.transform);
            
            T sp = obj.GetComponent<T>();
            sp.initialize<V>(world, parentPanel);
            sp.setAsPooled(pooledPanel);

            pooledPanel.wakeObject();

            return sp;
        }
        else return null;
    }

    public SubPanel bringMessage(string content, UnityAction action = null)
    {
        SubPanel message = bringSubPanel<SubPanel, Panel>(SubPanelType.Message, panel);
        message.getInteractable<GameBar>(Panel.type.bar, "box").setText(content);

        if (action != null) message.getInteractable<GameButton>(Panel.type.button, "button").onClick
            (
                () => 
                {
                    action.Invoke();
                    message.discard();
                }
            );

        return message;
    }
    public SubPanel bringPrompt(string content, UnityAction accept = null, UnityAction decline = null)
    {
        SubPanel message = bringSubPanel<SubPanel, Panel>(SubPanelType.Prompt, panel);
        message.getInteractable<GameBar>(Panel.type.bar, "box").setText(content);

        if (accept != null) message.getInteractable<GameButton>(Panel.type.button, "accept").onClick
            (
                () => 
                {
                    accept.Invoke();
                    message.discard();
                }
            );

        if (decline != null) message.getInteractable<GameButton>(Panel.type.button, "decline").onClick
            (
                () => 
                {
                    decline.Invoke();
                    message.discard();
                }
            );

        return message;
    }
}
