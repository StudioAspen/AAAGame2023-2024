using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public abstract class SwordAnimation : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent<GameObject> OnContact = new UnityEvent<GameObject>();
    public UnityEvent OnEndAnimation = new UnityEvent();

    public abstract void StartStabAnimation();
    public abstract void StartSlashAnimation();
    public abstract void StartDownwardStabAnimation();
    public abstract void StartSlashDashAnimation();
    public abstract void StartStabDashAnimation();
    public abstract void EndAnimation();
}