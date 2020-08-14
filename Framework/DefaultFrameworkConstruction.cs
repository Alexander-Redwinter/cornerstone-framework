namespace Cornerstone
{
    public class DefaultFrameworkConstruction : FrameworkConstruction
    {
        public DefaultFrameworkConstruction()
        {
            this.Configure().UseDefaultServices();
        }
    }
}
