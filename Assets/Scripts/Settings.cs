using UnityEngine;

namespace GGJ2021
{

  [CreateAssetMenu(fileName = "Settings", menuName = "GGJ2021/Settings", order = 1)]
  public class Settings : ScriptableObject
  {

    public const string PATH = "GGJ2021/Settings";

    [Header("Movement")]
    [Range(1f, 10f)]
    public float walkSpeedBoost = 2f;

    [Range(10f, 100f)]
    public float walkSpeedCap = 10f;

    [Range(1f, 10f)]
    public float runSpeedBoost = 3f;

    [Range(10f, 100f)]
    public float runSpeedCap = 16f;

    [Header("Jump")]
    [Range(.5f, 8f)]
    public float jumpHeight = 4f;

    [Range(1f, 10f)]
    public float jumpGravity = 2.5f;

  }
}
