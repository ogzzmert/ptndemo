
public class SubPanel : Panel
{
    private Panel parentPanel;
    protected override void launch()
    {
        onLaunch = false;
    }
    public virtual void initialize<T>(World world, T parentPanel) where T : Panel
    {
        this.parentPanel = parentPanel;
        
        initialize(world);
    }
    protected virtual T getParentPanel<T>() where T : Panel
    {
        return parentPanel as T;
    }
}