using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Thrower : LevelSingleton<Thrower>
{
    public int simSteps = 100;
    [Range(0,30)]
    public int throwVelocity = 1;
    [Range(0,90)]
    public float throwAngle = 20;
    [Range(1,10)]
    public int quality = 1;
    [Range(0,.1f)]
    public float timeStepAmount = 0.01666667f;
    [SerializeField] private GameObject potionPrefab;

    private GameControls gameControls;

    [SerializeField] private LineRenderer lineRenderer;

    private void Awake()
    {
        gameControls = new GameControls();
        lineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        gameControls.Enable();
    }

    private void OnDisable()
    {
        gameControls.Disable();
    }

    public void Throwing()
    {
        lineRenderer.enabled = true;
        float theta = throwAngle;
        if (gameControls.Game.Move.ReadValue<float>() < 0) theta = 360 - (theta + 180);
        theta *= Mathf.PI / 180f;

        var throwVelocity = this.throwVelocity * Mathf.Abs(gameControls.Game.Move.ReadValue<float>());

        lineRenderer.positionCount = simSteps/quality;
        for (int i = 0; i < simSteps/quality; i++)
        {
            var t = i * timeStepAmount * quality;
            float x = throwVelocity * t * Mathf.Cos(theta);
            float y = (throwVelocity * t * Mathf.Sin(theta)) + ((1f/2) * Physics2D.gravity.y * (t*t));
            lineRenderer.SetPosition(i, new Vector2(x, y));
        }
    }

    public void NotThrowing() 
    {
        lineRenderer.enabled = false;
    }

    public void Throw() 
    {
        Debug.Log("Throw!");

        var potion = Instantiate(potionPrefab, transform.position, Quaternion.identity);

        float theta = throwAngle;
        if (gameControls.Game.Move.ReadValue<float>() < 0) theta = 360 - (theta + 180);
        theta *= Mathf.PI / 180f;

        potion.GetComponent<Rigidbody2D>().velocity = throwVelocity * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)).normalized;
        
        potion.GetComponent<PotionGO>().potion = Inventory.Instance.potion;
        Inventory.Instance.potion = null;
    }
}
