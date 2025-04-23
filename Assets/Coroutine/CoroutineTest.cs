using System.Collections;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    private int _count = 0;

    void Update()
    {
        if (_count == 5) {
            Debug.Log($"Update 1");
            StartCoroutine(MyCoroutine());
            Debug.Log($"Update 2");
        }

        ++_count;
    }

    private IEnumerator MyCoroutine()
    {
        Debug.Log($"MyCoroutine 1");
        yield return null;
        Debug.Log($"MyCoroutine 2");
    }
}
