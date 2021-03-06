﻿using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace GGJ2021
{

  [RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
  public class PlayerMotor : MonoBehaviour
  {

    public static float walkSpeedBoost = 7;
    public static float walkSpeedCap = 50;

    public static float runSpeedBoost = 9;
    public static float runSpeedCap = 90;

    public static float jumpHeight = 4;
    public static float jumpGravity = 8;


    [Range(.2f, 1.2f)]
    public float feetOffset = .4f;

    [Range(.2f, 3f)]
    public float floorDetection = 1f;

    public LayerMask jumpableLayers;

    public Transform mesh;

    private Rigidbody2D rb;

    private Collider2D playerCollider;

    private Lantern lantern;

    private VisualEffect vfx;

    private float vfxBaseAngle;

    private float rawVelocity;

    private bool moving = false, running = false;

    private bool grounded = false, groundFlag = false, jumping = false, jumpFlag = false, jumpHeld = false;

    private Vector3 feet;
    private Vector3 deltaFeet;

    private Direction currDirection = Direction.RIGHT;


    private float speedBoost
    {
      get { return running ? runSpeedBoost : walkSpeedBoost; }
    }

    private float speedCap
    {
      get { return running ? runSpeedCap : walkSpeedCap; }
    }

    void OnValidate()
    {
      deltaFeet = new Vector3(feetOffset, 0);
    }

    void Awake()
    {
      FindComponents();
      feet = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
    }

    void OnEnable()
    {
      FindComponents();
      vfxBaseAngle = vfx.GetFloat("Out Angle");
    }

    private void FindComponents()
    {
      if (playerCollider == null)
        playerCollider = GetComponent<CapsuleCollider2D>();
      if (rb == null)
        rb = GetComponent<Rigidbody2D>();
      if (lantern == null)
        lantern = GetComponentInChildren<Lantern>(true);
      if (vfx == null)
        vfx = GetComponentInChildren<VisualEffect>();
    }


    void FixedUpdate()
    {
      RaycastHit2D hit = Physics2D.Raycast(feet + Vector3.up * playerCollider.bounds.size.y / 10f + deltaFeet, Vector2.down, floorDetection, jumpableLayers.value);
      if (hit.collider == null)
        hit = Physics2D.Raycast(feet + Vector3.up * playerCollider.bounds.size.y / 10f - deltaFeet, Vector2.down, floorDetection, jumpableLayers.value);

      groundFlag = grounded != (hit.collider != null);
      grounded = hit.collider != null;

      if (jumpFlag && !grounded)
        jumpFlag = false;
      else if (!jumpFlag && grounded && rb.velocity.y > -.1f)
      {
        jumping = false;
        if (jumpHeld) Jump();
      }

      if (rb.velocity.x * Mathf.Sign(rawVelocity) < speedCap)
      {
        float sign = Mathf.Sign(rawVelocity);
        Vector2 movForce = Vector2.right * Mathf.Max(speedCap - rb.velocity.x * sign, speedBoost * sign) * rawVelocity;
        if (!grounded)
          movForce += (jumpGravity - 1) * Physics2D.gravity;
        rb.AddForce(movForce * Time.fixedDeltaTime, ForceMode2D.Impulse);
      }
    }

    void LateUpdate()
    {
      feet = new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y);
      if (moving)
      {
        Direction newDirection = rawVelocity > 0 ? Direction.RIGHT : Direction.LEFT;
        if (currDirection != newDirection) Flip(newDirection);
      }
    }

    void OnDrawGizmos()
    {
      if (playerCollider == null) return;
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(feet + deltaFeet + Vector3.up * playerCollider.bounds.size.y / 10f, feet + deltaFeet + Vector3.down * floorDetection);
      Gizmos.DrawLine(feet - deltaFeet + Vector3.up * playerCollider.bounds.size.y / 10f, feet - deltaFeet + Vector3.down * floorDetection);
    }


    public void DoMove(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
          rawVelocity = ctx.ReadValue<float>();
          break;
        case InputActionPhase.Canceled:
          rawVelocity = 0;
          break;
      }

      moving = Mathf.Abs(rawVelocity) > Mathf.Epsilon;
      lantern.SetLock(moving);
    }


    public void DoRun(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
        case InputActionPhase.Canceled:
          running = ctx.ReadValueAsButton();
          break;
      }
    }


    public void DoJump(CallbackContext ctx)
    {
      switch (ctx.phase)
      {
        case InputActionPhase.Performed:
          jumpHeld = true;
          Jump();
          break;
        case InputActionPhase.Canceled:
          jumpHeld = false;
          break;
      }
    }


    private void Jump()
    {
      if (jumping) return;
      float v0 = 2 * Mathf.Sqrt(2 * jumpGravity * jumpHeight * -Physics2D.gravity.y);
      rb.AddForce(Vector2.up * v0, ForceMode2D.Impulse);
      jumping = true;
      jumpFlag = true;
    }

    private void Flip(Direction newDirection)
    {
      Vector3 meshRot = mesh.rotation.eulerAngles;
      Vector3 lanternRot = lantern.transform.rotation.eulerAngles;

      meshRot.y = lanternRot.y = newDirection == Direction.RIGHT ? 0f : 180f;

      mesh.rotation = Quaternion.Euler(meshRot);
      lantern.transform.rotation = Quaternion.Euler(lanternRot);

      Vector3 lanternPos = lantern.transform.localPosition;
      lanternPos.x *= -1;
      lantern.transform.localPosition = lanternPos;

      vfx.SetFloat("Out Angle", newDirection == Direction.RIGHT ? vfxBaseAngle : (360 - vfxBaseAngle));

      currDirection = newDirection;
    }
  }

}

