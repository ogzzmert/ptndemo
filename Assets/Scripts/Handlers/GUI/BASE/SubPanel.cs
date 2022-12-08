
public class SubPanel : Panel
{
    protected override void launch()
    {
        onLaunch = false;
    }
    public virtual void initialize<T>(Seed world, T panel) where T : Panel
    {
        setWorld(world);
    }
}