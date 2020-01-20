using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public abstract class Volleyball : MonoBehaviour
{
    public abstract float RADIUS { get; }
    static bool addedListeners;

    [HideInInspector] public bool SPIKES {
        get { return PlayerPrefs.GetInt("Spikes") == 1; } }

    void Start()
    {
        AddListeners();
        SceneManager.sceneUnloaded += (s) => { addedListeners = false; };

        int layer = LayerMask.NameToLayer("Volleyball");
        if (PlayerPrefs.GetInt("Ball Collision") == 1)
            Physics2D.IgnoreLayerCollision(layer, layer, false);
        else
            Physics2D.IgnoreLayerCollision(layer, layer, true);
    }

    void Update()
    {
        DetectOutOfBounds();
    }

    protected virtual void AddListeners()
    {
        if (!addedListeners)
        {
            Messenger.AddListener<Volleyball, PlayerController>(
                GameEvent.PLAYER_CONTACT, DetectSpike);
            Messenger.AddListener<Volleyball, PlayerController>(
                GameEvent.SPIKE_HIT, SpikeBall);
            addedListeners = true;
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObj = collision.gameObject;

        PlayerController player = gameObj.GetComponent<PlayerController>();
        if (player != null)
        {
            Messenger.Broadcast(GameEvent.PLAYER_CONTACT, this, player);
            return;
        }

        ScoreArea area = gameObj.GetComponent<ScoreArea>();
        if (area != null)
        {
            Messenger.Broadcast(GameEvent.SCORE_AREA_CONTACT, this, area);
            return;
        }

        Tilemap tilemap = gameObj.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            Messenger.Broadcast(GameEvent.WALL_CONTACT, this, tilemap);
            return;
        }
    }

    protected abstract void DetectSpike(Volleyball v, PlayerController player);
    protected abstract void SpikeBall(Volleyball v, PlayerController player);
    protected virtual void DetectOutOfBounds()
    {
        float posX = transform.position.x;
        float posY = transform.position.y;

        // Allow slight buffer
        if (posX - RADIUS < Game.Instance.LEFT_WALLX - 0.01f)
            Messenger.Broadcast(GameEvent.VOLLEYBALL_OUT_OF_BOUNDS,
                this, Side.LEFT);
        if (posX + RADIUS > Game.Instance.RIGHT_WALLX + 0.01f)
            Messenger.Broadcast(GameEvent.VOLLEYBALL_OUT_OF_BOUNDS,
                this, Side.RIGHT);
        if (posY - RADIUS < Game.Instance.GROUND - 0.01f)
            Messenger.Broadcast(GameEvent.VOLLEYBALL_OUT_OF_BOUNDS,
                this, Side.GROUND);
        if (posY + RADIUS > Game.Instance.CEILING + 0.01f)
            Messenger.Broadcast(GameEvent.VOLLEYBALL_OUT_OF_BOUNDS,
                this, Side.CEILING);
    }
}
