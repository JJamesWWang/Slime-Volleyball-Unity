using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string horizontalAxis = default;
    [SerializeField] string verticalAxis = default;
    [SerializeField] public Side SIDE = default;

    [HideInInspector] public int HSPEED {
        get { return PlayerPrefs.GetInt("Horizontal Speed"); } }
    [HideInInspector] public int VSPEED {
        get { return PlayerPrefs.GetInt("Vertical Speed"); } }
    [HideInInspector] public float JUMP_TIME {
        get { return PlayerPrefs.GetFloat("Jump Time"); } }
    [HideInInspector] public float HOVER_TIME {
        get { return PlayerPrefs.GetFloat("Hover Time"); } }
    public float DUNK_HEIGHT { get { return 0.4f; } }
    public float DUNK_STRENGTH { get { return 1000f; } }
    public float XRADIUS { get { return 2f; } }
    public float YRADIUS { get { return 1f; } }

    float leftXBound;
    float rightXBound;

    float jumpTimer;
    bool jumping;
    bool jumpingUp;

    float hoveringTimer;
    bool hovering;

    bool allowMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        if (SIDE == Side.UNSET)
            throw new UnassignedReferenceException("Player side unset");

        if (gameObject.layer == LayerMask.NameToLayer("Left Players"))
        {
            leftXBound = Game.Instance.LEFT_WALLX + XRADIUS;
            rightXBound = Game.Instance.NET_LEFT - XRADIUS;
        } else if (gameObject.layer == LayerMask.NameToLayer("Right Players"))
        {
            leftXBound = Game.Instance.NET_RIGHT + XRADIUS;
            rightXBound = Game.Instance.RIGHT_WALLX - XRADIUS;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (allowMovement)
            transform.position = HandleMovement();
    }

    public void Reset()
    {
        jumping = false;
        jumpTimer = 0;
        hovering = false;
        hoveringTimer = 0;
    }

    Vector2 HandleMovement()
    {
        if (Input.GetAxisRaw(verticalAxis) > 0 && !jumping && !hovering)
            StartJump();
        else if (Input.GetAxisRaw(verticalAxis) < 0 && jumpingUp && !hovering)
            StartHover();

        return new Vector2(getNewX(), getNewY());
    }

    public void AllowMovement(bool allow)
    {
        allowMovement = allow;
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
            if (newY < Game.Instance.GROUND)
            {
                jumping = false;
                return 0f;
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
