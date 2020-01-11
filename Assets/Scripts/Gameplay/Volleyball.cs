using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Volleyball : MonoBehaviour
{
    public abstract float RADIUS { get; }

    [HideInInspector] public bool SPIKES {
        get { return PlayerPrefs.GetInt("Spikes") == 1; } }

    protected Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObj = collision.gameObject;

        PlayerController player = gameObj.GetComponent<PlayerController>();
        if (player != null)
        {

        if (SPIKES)
            
        }


        ScoreArea area = gameObj.GetComponent<ScoreArea>();
        if (area != null)
            GameState.Instance.EndPoint(area.side);
    }

    protected abstract void SpikeBall(Collision2D collision,
        PlayerController player);
}
