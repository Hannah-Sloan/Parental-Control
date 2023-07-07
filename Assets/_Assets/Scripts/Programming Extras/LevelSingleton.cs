using UnityEngine;

public class LevelSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;

    private static object padlock = new object();

    public static T Instance
    {
        get
        {
            lock (padlock) 
            {
                if (instance == null)
                {
                    instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[LevelSingleton] There should never be more than 1 singleton!");
                        return instance;
                    }

                    if (instance == null)
                    {
                        Debug.Log("[LevelSingleton] No instance of " + typeof(T) + " found!");
                        return null;
                    }
                }
                return instance;
            }

        }
    }
}
