using UnityEngine;

public class POI : MonoBehaviour
{
    private bool setup = false;

    private void Update()
    {
        if (!setup) 
        {
            POI_Manager.Instance.Register(this);
            setup = true;
        } 
    }

    [Range(0, 100)]
    public int weight;
}
