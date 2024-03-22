public interface ILockayable
{
    public bool IsLocked { get; set; }
    public void Lock() => IsLocked = true;
    public void Unlock() => IsLocked = false;
}