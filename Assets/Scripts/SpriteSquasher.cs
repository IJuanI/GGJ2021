using System.Collections.Generic;
using UnityEngine;

namespace GGJ2021
{
  [ExecuteInEditMode]
  public class SpriteSquasher : MonoBehaviour
  {
    public Vector2 targetSize;

#if UNITY_EDITOR
    private SpriteRenderer[] rends;

    void OnEnable()
    {
      rends = GetComponentsInChildren<SpriteRenderer>();
    }
    void Update()
    {
      Vector2 scale = transform.localScale;
      Vector2 realScale = targetSize / scale;
      for (int i = 0; i < rends.Length; ++i)
      {
        SpriteRenderer rend = rends[i];
        rend.transform.localScale = realScale;
        rend.size = scale;
      }
    }
#endif
  }
}

