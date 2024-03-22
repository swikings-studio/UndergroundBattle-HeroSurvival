using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockayablesManager
{
    private readonly ILockayable[] lockayables;

    public LockayablesManager(MonoBehaviour monoBehaviour)
    {
        lockayables = monoBehaviour.GetComponents<ILockayable>();
    }
    public void LockAll()
    {
        foreach (ILockayable lockayable in lockayables) if (lockayable.IsLocked == false) lockayable.Lock();
    }
    public void UnlockAll()
    {
        foreach (ILockayable lockayable in lockayables) if (lockayable.IsLocked) lockayable.Unlock();
    }
}