using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "DialogueSection", menuName = "Dialogue/NewDialogueSection", order = 1)]
public class DialogueSection : ScriptableObject
{
    [SerializeField]
    public List<SpeechData> dialogue = new List<SpeechData>();

    [HideInInspector] public int index = 0;
    public SpeechData GetNext() 
    {
        if (index == dialogue.Count) return null;
        return dialogue[index++];
    }

    public void Reset()
    {
        index = 0;
    }

    public void Back() { index--; }
}
