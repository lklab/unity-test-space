using TMPro;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectHandler : MonoBehaviour
{
    [SerializeField] private MyScriptableObject _scriptableObject;
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        _text.text = _scriptableObject.value.ToString();
        _scriptableObject.value--;

#if UNITY_EDITOR
        EditorUtility.SetDirty(_scriptableObject);
#endif
    }
}
