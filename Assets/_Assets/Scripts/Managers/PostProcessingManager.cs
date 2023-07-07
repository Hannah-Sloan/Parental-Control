using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : LevelSingleton<PostProcessingManager>
{
    [HideInInspector]
    public Volume volume;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }
}
