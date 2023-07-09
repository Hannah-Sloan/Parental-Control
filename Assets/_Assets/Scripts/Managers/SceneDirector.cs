using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDirector : Singleton<SceneDirector>
{
    private void Start()
    {
        LoadMainMenu();
    }

    public void LoadScene(int sceneNum) 
    {
        SceneManager.LoadScene(sceneNum);
    }

    [ContextMenu("Start")]
    public void StartGame() 
    {
        print("Starting Game");

        SceneManager.LoadScene(2);

        //var op2 = SceneManager.UnloadSceneAsync("MainMenu");
        //op2.completed +=
        //(AsyncOperation b) =>
        //{
        //    SceneManager.LoadScene(2);
        //};

        //var op = SceneManager.LoadSceneAsync("Initialization", LoadSceneMode.Additive);
        //op.completed += 
        //    (AsyncOperation a) => 
        //    { 
        //        var op2 = SceneManager.UnloadSceneAsync("MainMenu");
        //        op2.completed +=
        //        (AsyncOperation b) =>
        //        {
        //            SceneManager.LoadScene(2);
        //        };
        //    };
    }

    public void LoadMainMenu() 
    {
        PauseManager.Instance.Unpause(PauseManager.PauseType.menu);
        PauseManager.Instance.Unpause(PauseManager.PauseType.dialogue);
        PauseManager.Instance.Unpause(PauseManager.PauseType.cutscene);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Quit() 
    {
        Application.Quit();
    }
}
