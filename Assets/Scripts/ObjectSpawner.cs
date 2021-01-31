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
    public GameObject[] prefabs;

    public SpawnMode mode = SpawnMode.AUTO;

    public bool spawnDestroyables = true;

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
      while (count++ < burstSize || burstSize < 0)
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
        GameObject newObject = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPos.position, spawnPos.rotation);
        if (spawnDestroyables)
          newObject.AddComponent<Destroyable>();
        Spawnable spawnable;
        if (newObject.TryGetComponent<Spawnable>(out spawnable))
          spawnable.spawner = this;
      }
    }

    public void OnObjectDespawn() { spawnCount--; }
  }
}