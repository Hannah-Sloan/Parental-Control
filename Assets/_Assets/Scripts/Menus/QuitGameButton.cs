using UnityEngine;

public class QuitGameButton : MonoBehaviour
{
    public void Apply() 
    {
        SceneDirector.Instance.Quit();
    }
}
