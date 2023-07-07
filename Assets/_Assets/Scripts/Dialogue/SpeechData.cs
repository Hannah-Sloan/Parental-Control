using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpeechData", menuName = "Dialogue/NewSpeechData", order = 2)]
public class SpeechData : ScriptableObject
{
    [SerializeField]
    public Sprite image;
    [SerializeField]
    public string speakerName;

    [Header("Rich Text Field")]
    [TextArea(10,100)]
    [SerializeField]
    public string text;
}
