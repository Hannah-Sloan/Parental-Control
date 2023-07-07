using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class POI_Manager : Singleton<POI_Manager>
{
    public HashSet<POI> pointsOfIntrest = new HashSet<POI>();
    public float maxDist = 5;

    public void Register(POI poi) 
    {
        pointsOfIntrest.Add(poi);
        pointsOfIntrest.RemoveWhere(x => x == null);
    }

    public Vector2 FindPoint() 
    {
        List<(float, Vector2)> weights = new List<(float, Vector2)>();
        foreach (var poi in pointsOfIntrest)
        {
            var distWeight = 1-Mathf.Clamp01((poi.transform.position - PlayerController.Instance.transform.position).magnitude/maxDist);
            distWeight *= distWeight;
            var totalWeight = distWeight * (poi.weight / 100f);
            
            weights.Add((totalWeight, (Vector2)poi.transform.position));
        }

        float sum = weights.Sum(p => p.Item1);

        this.sum = sum;
        this.weights = weights;

        Vector2 pos = Vector2.zero;
        foreach (var weight in weights)
        {
            pos += (weight.Item1 / sum ) * weight.Item2;
        }

        return pos;
    }

    private float sum = 0; 
    private List<(float, Vector2)> weights; 

    public void OnDrawGizmos()
    {
        var pos = FindPoint();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pos, 0.15f);

        foreach (var weight in weights)
        {
            Gizmos.color = new Color(0,0,1, weight.Item1/sum * 0.5f);
            Gizmos.DrawSphere(weight.Item2, maxDist);
        }
    }
}
