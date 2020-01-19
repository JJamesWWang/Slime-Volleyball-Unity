﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalVolleyball : Volleyball
{
    public override float RADIUS { get { return 0.5f; } }

    void FixedUpdate()
    {
        if (Game.Instance != null)
            BoundBall();
    }

    private void BoundBall()
    {
        Rigidbody2D body = GetComponent<Rigidbody2D>();
        float posX = body.position.x;
        float posY = body.position.y;
        Vector2 modifiedPosition = body.position;

        if (posY - RADIUS <= Game.Instance.GROUND)
            modifiedPosition.y = Game.Instance.DROP_HEIGHT;

        if (posX + RADIUS > Game.Instance.RIGHT_WALLX)
            modifiedPosition.x = Game.Instance.RIGHT_WALLX - RADIUS;
        else if (posX - RADIUS < Game.Instance.LEFT_WALLX)
            modifiedPosition.x = Game.Instance.LEFT_WALLX + RADIUS;

        body.position = modifiedPosition;
    }

    protected override void DetectSpike(Volleyball v, PlayerController player)
    {
        if (!SPIKES)
            return;

        Rigidbody2D body = v.GetComponent<Rigidbody2D>();
        Vector2 playerPos = player.transform.position;
        Vector2 ballPos = body.position;
        float dunkTop = playerPos.y + player.DUNK_HEIGHT;
        float dunkBot = playerPos.y;

        if (ballPos.y - dunkTop < 0 && ballPos.y - dunkBot > -RADIUS)
            Messenger.Broadcast(GameEvent.SPIKE_HIT, v, player);
    }

    protected override void SpikeBall(Volleyball v, PlayerController player)
    {
        Rigidbody2D body = v.GetComponent<Rigidbody2D>();
        Vector2 playerPos = player.transform.position;
        Vector2 ballPos = body.position;
        float dunkTop = playerPos.y + player.DUNK_HEIGHT;

        Vector2 direction = new Vector2(
            ballPos.x - playerPos.x, dunkTop - ballPos.y);

        if (ballPos.x > playerPos.x)
            direction += Vector2.right * player.XRADIUS;
        else
            direction += Vector2.left * player.XRADIUS;

        direction.Normalize();
        body.AddForce(direction * player.DUNK_STRENGTH);
    }
}
