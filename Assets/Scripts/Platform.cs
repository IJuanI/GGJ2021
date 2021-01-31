using System;
using System.Collections;
using UnityEngine;

namespace GGJ2021
{
  public enum PlatformMode { STATIC, PERIODIC, MANUAL, AUTO, AUTO_RETURN }
  public enum PlatformDirection { IN, OUT }
  public enum PlatformInitPos { SOURCE, TARGET }

  [Serializable]
  public struct PathEntry
  {
    [Range(0, 1)]
    public float weight;
    public Transform target;
  }

  public class Platform : MonoBehaviour
  {
    public PathEntry[] path;

    public float moveDuration = 1f;
    public float initialDelay = 0f;
    public float moveDelay = 1f;
    public PlatformMode mode = PlatformMode.STATIC;
    public PlatformInitPos initPos = PlatformInitPos.SOURCE;

    private PlatformDirection direction = PlatformDirection.OUT;

    private int moveIdx = 0;
    private int playerCount = 0;
    private float alpha = 0;
    private bool moving = false;

    void OnEnable()
    {
      alpha = initPos == PlatformInitPos.SOURCE ? 0 : 1;
    }

    void Start()
    {
      transform.position = CalcPosition(alpha);
      if (mode == PlatformMode.PERIODIC)
        DOMove();
    }

    void FixedUpdate()
    {
      if (moving)
      {
        float delta = Time.fixedDeltaTime / moveDuration * (direction == PlatformDirection.IN ? 1 : -1);
        alpha += delta;
        alpha = Mathf.Clamp(alpha, 0f, 1f);
        transform.position = CalcPosition(alpha);

        if (alpha == 0f || alpha == 1f)
        {
          moving = false;
          AfterMove();
        }
      }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
      PlayerEnt player;
      if (other.collider.TryGetComponent<PlayerEnt>(out player))
        if (playerCount++ == 0 && (mode == PlatformMode.AUTO || mode == PlatformMode.AUTO_RETURN))
          DOMove();
    }

    void OnCollisionExit2D(Collision2D other)
    {
      PlayerEnt player;
      if (other.collider.TryGetComponent<PlayerEnt>(out player))
        if (--playerCount == 0 && (mode == PlatformMode.AUTO_RETURN))
          DOMove();
    }

    Vector3 CalcPosition(float alfa)
    {
      PathEntry? last = null, next = null;
      foreach (PathEntry entry in path)
      {
        next = entry;
        if (entry.weight > alfa) break;
        last = entry;
      }

      if (last.HasValue && next.HasValue)
      {
        float? subAlpha = (alfa - last?.weight) / (next?.weight - last?.weight);
        return (last?.target.position * (1 - subAlpha) + next?.target.position * subAlpha).Value;
      }
      else if (next.HasValue)
        return next.Value.target.position;
      return transform.position;
    }

    public void DOMove()
    {
      if (mode != PlatformMode.STATIC && !moving)
      {
        if (initialDelay > 0)
          StartCoroutine(DelayMove());
        else RealMove();
      }
    }

    private IEnumerator DelayMove()
    {
      yield return new WaitForSeconds(initialDelay);
      RealMove();
    }

    private void RealMove()
    {
      direction = direction == PlatformDirection.IN ? PlatformDirection.OUT : PlatformDirection.IN;
      moving = true;
      ++moveIdx;
    }


    void AfterMove()
    {
      StartCoroutine(DoNext());
    }

    IEnumerator DoNext()
    {
      int currMove = moveIdx;
      if (mode == PlatformMode.PERIODIC || mode == PlatformMode.AUTO_RETURN)
        yield return new WaitForSeconds(moveDelay);
      if (mode == PlatformMode.PERIODIC
          || (playerCount == 0 && mode == PlatformMode.AUTO_RETURN && moveIdx == currMove && direction == PlatformDirection.IN))
        DOMove();

      yield return null;
    }
  }
}