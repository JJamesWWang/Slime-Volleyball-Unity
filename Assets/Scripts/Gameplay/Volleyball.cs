using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volleyball : MonoBehaviour
{
    public float RADIUS;
    [HideInInspector] public bool SPIKES {
        get { return PlayerPrefs.GetInt("Spikes") == 1; } }
    Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = body.position.x;
        float posY = body.position.y;
        Vector2 modifiedPosition = body.position;

        if (posY - RADIUS <= GameState.Instance.GROUND)
            modifiedPosition.y = GameState.Instance.DROP_HEIGHT;

        if (posX + RADIUS > GameState.Instance.RIGHT_WALLX)
            modifiedPosition.x = GameState.Instance.RIGHT_WALLX - RADIUS;
        else if (posX - RADIUS < GameState.Instance.LEFT_WALLX)
            modifiedPosition.x = GameState.Instance.LEFT_WALLX + RADIUS;

        body.position = modifiedPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject gameObj = collision.gameObject;

        PlayerController player = gameObj.GetComponent<PlayerController>();
        if (SPIKES && player != null)
            SpikeBall(collision, player);

        ScoreArea area = gameObj.GetComponent<ScoreArea>();
        if (area != null)
            GameState.Instance.EndPoint(area.side);
    }

    private void SpikeBall(Collision2D collision, PlayerController player)
    {
        Vector2 playerPos = collision.rigidbody.position;
        Vector2 ballPos = body.position;
        float dunkTop = playerPos.y + player.DUNK_HEIGHT;
        float dunkBot = playerPos.y;

        if (ballPos.y - dunkTop < 0 && ballPos.y - dunkBot > -RADIUS)
        {
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
}
