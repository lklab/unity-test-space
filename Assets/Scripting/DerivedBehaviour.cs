using UnityEngine;

public class DerivedBehaviour : BaseBehaviour
{
    private bool isFirst2 = true;

    protected override void Awake()
    {
        base.Awake();
        Debug.Log("DerivedBehaviour.Awake()");
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("DerivedBehaviour.Start()");
    }

    protected override void Update()
    {
        base.Update();
        if (isFirst2)
        {
            Debug.Log("DerivedBehaviour.Update()");
            isFirst2 = false;
        }
    }
}
