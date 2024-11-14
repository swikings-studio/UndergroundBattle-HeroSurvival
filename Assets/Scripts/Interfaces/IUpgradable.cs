public interface IUpgradable
{
    public PlayerSystem PlayerSystem { get; }
    public void Upgrade(float value);
}