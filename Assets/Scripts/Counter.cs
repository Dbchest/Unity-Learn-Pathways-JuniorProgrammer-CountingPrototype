using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;

    public string Text
    {
        get => "Sequence: " + Sequence.Instance.Count;
    }

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        Sequence.IndexChanged += UpdateSequenceText;
    }

    private void UpdateSequenceText()
    {
        textMeshProUGUI.text = Text;
    }

    private void OnDisable()
    {
        Sequence.IndexChanged -= UpdateSequenceText;
    }
}