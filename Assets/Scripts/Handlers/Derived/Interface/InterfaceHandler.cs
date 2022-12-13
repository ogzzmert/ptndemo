using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceHandler : Handler, IHandlerGenerator
{
    [field: SerializeField] private Canvas canvas;

    [Serializable]
    private class GameInterface
    {
        public WorldType type;
        public GameObject panel;
    } 

    Dictionary<WorldType, GameObject> panels = new Dictionary<WorldType, GameObject>();
    Dictionary<SubPanelType, GameObject> subpanels = new Dictionary<SubPanelType, GameObject>();
    Panel panel;

    protected override void initialize()
    {
        foreach(WorldType wt in Enum.GetValues(typeof(WorldType)))
        {
            string pname = wt.ToString();
            GameObject p = ResourceManager.load<GameObject>("Prefab/UI/Panel/" + pname);
            if (p != null)
            {
                panels.Add(wt, p);

            }
        }
        foreach(SubPanelType spt in Enum.GetValues(typeof(SubPanelType)))
        {
            string pname = spt.ToString();
            GameObject p = ResourceManager.load<GameObject>("Prefab/UI/Subpanel/" + pname);
            if (p != null)
            {
                subpanels.Add(spt, p);

            }
        }
        
    }
    public void generate(WorldType worldType, int worldIndex)
    {
        if (panel != null)
        {
            panel.discard();

            world.destroy(panel.gameObject);

            panel = null;

        }

        if (panels.ContainsKey(worldType))
        {

            panel = world.spawn(panels[worldType]).GetComponent<Panel>();
            
            panel.gameObject.transform.SetParent(this.canvas.gameObject.transform);
            panel.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            panel.gameObject.transform.localScale = new Vector3(1, 1, 1);

            panel.initialize(world, worldIndex);

        }
    }
    public void degenerate()
    {
        world.destroy(panel.gameObject);
        panel = null;
    }
    public T spawnSubPanel<T, V>(SubPanelType subpanel) where T : SubPanel where V : Panel
    {
        if (subpanels.ContainsKey(subpanel))
        {
            GameObject obj = world.spawn(subpanels[subpanel]);
            obj.gameObject.transform.SetParent(panel.transform);
            obj.gameObject.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            obj.gameObject.transform.localScale = new Vector3(1, 1, 1);
            
            T sp = obj.GetComponent<T>();
            sp.initialize<V>(world, panel as V);

            return obj.GetComponent<T>();
        }
        else return null;
    }
}
