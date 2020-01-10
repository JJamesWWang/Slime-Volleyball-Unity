using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float defaultXPosition = default;
    [SerializeField] float defaultYPosition = default;
    [SerializeField] string horizontalAxis = default;
    [SerializeField] string verticalAxis = default;

    [HideInInspector] public int HSPEED {
        get { return PlayerPrefs.GetInt("Horizontal Speed"); }
    }
    [HideInInspector] public int VSPEED
    {
        get { return PlayerPrefs.GetInt("Vertical Speed"); }
    }
    [HideInInspector] public float JUMP_TIME
    {
        get { return PlayerPrefs.GetFloat("Jump Time"); }
    }
    [HideInInspector] public float HOVER_TIME
    {
        get { return PlayerPrefs.GetFloat("Hover Time"); }
    }
    public Side SIDE;
    public float DUNK_HEIGHT { get { return 0.4f; } }
    public float DUNK_STRENGTH { get { return 1000f; } }
    public float XRADIUS { get { return 2f; } }

    float leftXBound;
    float rightXBound;

    float jumpTimer;
    bool jumping;
    bool jumpingUp;

    float hoveringTimer;
    bool hovering;

    // Start is called before the first frame update
    void Start()
    {
        if (SIDE == Side.UNSET)
            throw new UnassignedReferenceException("Player side unset");

        if (gameObject.layer == LayerMask.NameToLayer("Left Players"))
        {
            leftXBound = GameState.Instance.LEFT_WALLX + XRADIUS;
            rightXBound = GameState.Instance.NET_LEFT - XRADIUS;
        } else if (gameObject.layer == LayerMask.NameToLayer("Right Players"))
        {
            leftXBound = GameState.Instance.NET_RIGHT + XRADIUS;
            rightXBound = GameState.Instance.RIGHT_WALLX - XRADIUS;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = HandleMovement();
    }

    public void Reset()
    {
        jumping = false;
        jumpTimer = 0;
        hovering = false;
        hoveringTimer = 0;
        transform.position = new Vector2(defaultXPosition, defaultYPosition);
    }

    Vector2 HandleMovement()
    {
        if (Input.GetAxisRaw(verticalAxis) > 0 && !jumping && !hovering)
            StartJump();
        else if (Input.GetAxisRaw(verticalAxis) < 0 && jumpingUp && !hovering)
            StartHover();

        return new Vector2(getNewX(), getNewY());
    }


    private void StartJump()
    {
        jumpTimer = JUMP_TIME;
        jumpingUp = true;
        jumping = true;
    }

    private void StartHover()
    {
        jumping = false;
        hovering = true;
        hoveringTimer = HOVER_TIME;
    }

    float getNewX()
    {
        return Mathf.Clamp(transform.position.x +
            Input.GetAxis(horizontalAxis) * HSPEED * Time.deltaTime,
            leftXBound, rightXBound);
    }

    float getNewY()
    {
        if (jumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpingUp)
                return Jump();
            else
                return Fall();
        }

        if (hovering)
        {
            hoveringTimer -= Time.deltaTime;
            if (hoveringTimer < 0)
                StartFall();
        }
        return transform.position.y;
    }

    private float Jump()
    {
        if (jumpTimer < 0)
        {
            StartHover();
            return transform.position.y;
        }
        else
            return transform.position.y + VSPEED * Time.deltaTime;
    }

    private float Fall()
    {
        if (jumpTimer < 0)
        {
            jumping = false;
            return 0f;
        }
        else
        {
            float newY = transform.position.y - VSPEED * Time.deltaTime;
            if (newY < GameState.Instance.GROUND)
            {
                jumping = false;
            }
            return newY;
        }
    }

    private void StartFall()
    {
        hovering = false;
        jumping = true;
        jumpingUp = false;
        jumpTimer = JUMP_TIME;
    }

}
