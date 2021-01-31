using System.Collections;
using UnityEngine;

namespace GGJ2021
{
  public enum SpawnMode
  {
    AUTO, MANUAL
  }

  public class ObjectSpawner : MonoBehaviour
  {

    public Transform spawnPos;
    public GameObject prefab;

    public SpawnMode mode = SpawnMode.AUTO;

    public int spawnLimit = -1;

    public float spawnRate = .2f;

    public float burstSize = 4f;

    public float burstCooldown = 0f;

    private int spawnCount;

    void Start()
    {
      StartCoroutine(RunLoop());
    }

    private IEnumerator RunLoop()
    {
      int count = 0;
      while (count++ < burstSize)
      {
        yield return new WaitForSeconds(1 / spawnRate);
        Spawn();
      }
      yield return new WaitForSeconds(burstCooldown);
      StartCoroutine(RunLoop());
    }

    private void Spawn()
    {
      if (spawnLimit < 0 || spawnCount < spawnLimit)
      {
        spawnCount++;
        GameObject newObject = Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        Spawnable spawnable;
        if (newObject.TryGetComponent<Spawnable>(out spawnable))
          spawnable.spawner = this;
      }
    }

    public void OnObjectDespawn() { spawnCount--; }
  }
}