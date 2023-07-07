using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void Apply() 
    {
        SceneDirector.Instance.LoadMainMenu();
    }
}
