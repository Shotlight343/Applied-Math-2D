using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public RunnerPlayer player;
    public Transform parent;
  private List<Item> items = new List<Item>();

    public float speed = 12f;
    public float acceleration = 0.2f;
    public float maxSpeed = 30f;
    public float spawnGap = 10f;
    public float spawnZ = 60f;
    public float despawnZ = -5f;

    public float obstacleHeight = 1f;
    public float hitDepth = 0.5f;
    public float hitWidth = 1f;

    private float distanceSinceSpawn;

    private void Update()
    {
        var dt = Time.deltaTime;
        speed = Mathf.Min(speed + acceleration * dt, maxSpeed);

        var step = speed * dt;
        distanceSinceSpawn = distanceSinceSpawn + step;
        if (distanceSinceSpawn >= spawnGap)
        {
            distanceSinceSpawn = distanceSinceSpawn - spawnGap;
            Spawn();
        }

        for (int i = items.Count - 1; i >= 0; i--)
        {
            var item = items[i];
            if (item == null)
            {
                items.RemoveAt(i);
                continue;
            }

            item.itemPosition = item.itemPosition - Vector3.forward * step;
            CheckHit(item);

            if (item.itemPosition.z < despawnZ)
            {
                items.RemoveAt(i);
                Destroy(item.gameObject);
            }
        }
    }

    private void Spawn()
    {
        var lane = Random.Range(0, 3);
        var spawned = Instantiate(itemToSpawn, parent);
        spawned.itemPosition = new Vector3(player.LaneToX(lane), player.groundY, spawnZ);
        items.Add(spawned);
    }

    private void CheckHit(Item item)
    {
        if (item.hasHit)
        {
            return;
        }

        var delta = player.playerPosition - item.itemPosition;
        var closeInDepth = Mathf.Abs(delta.z) <= hitDepth;
        var sameLane = Mathf.Abs(delta.x) <= hitWidth;
        var belowTop = delta.y < obstacleHeight;

        if (closeInDepth && sameLane && belowTop)
        {
            item.hasHit = true;
            player.TakeDamage();
        }
    }
}