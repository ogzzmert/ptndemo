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
            else break;
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

}
