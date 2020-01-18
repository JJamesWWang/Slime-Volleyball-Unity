using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProtect : NormalGame
{
    [SerializeField] float DROP_FORCE = 1000f;
    private void Awake()
    {
        Init();
        if (PlayerPrefs.GetString("Game Mode").Equals("Wall Protect"))
        {
            Instance = this;
            Map.SetActive(true);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("Game Mode").Equals("Wall Protect"))
        {
            AddListeners();
            Setup();
        }
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        Messenger.AddListener<Volleyball, Side>(GameEvent.VOLLEYBALL_DROPPED,
            (volleyball, side) =>
            {
                Rigidbody2D body = volleyball.GetComponent<Rigidbody2D>();
                body.AddForce(Vector2.down * DROP_FORCE);
            });
    }
}
