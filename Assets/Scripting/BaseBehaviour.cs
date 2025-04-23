using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    private bool isFirst = true;

    protected virtual void Awake()
    {
        Debug.Log("BaseBehaviour.Awake()");
    }

    protected virtual void Start()
    {
        Debug.Log("BaseBehaviour.Start()");
    }

    protected virtual void Update()
    {
        if (isFirst)
        {
            Debug.Log("BaseBehaviour.Update()");
            isFirst = false;
        }
    }
}
