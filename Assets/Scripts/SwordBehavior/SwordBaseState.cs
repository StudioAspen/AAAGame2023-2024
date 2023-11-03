using UnityEngine;

// abstract class for the different sword states to copy/derive from when in those states
// i.e when thirsty or when fully fed, etc.
public abstract class SwordBaseState
{
    public abstract void EnterState(SwordStateManager sword);

    public abstract void UpdateState(SwordStateManager sword);

    public abstract void OnCollisionEnter(SwordStateManager sword, Collision collision);
}
