using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public void Apply() 
    {
        SceneDirector.Instance.StartGame();
    }
}
