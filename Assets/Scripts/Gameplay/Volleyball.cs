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
            Physics.IgnoreLayerCollision(layer, layer, false);
        else
            Physics.IgnoreLayerCollision(layer, layer, true);
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
}
