
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public float hp = 14;
    float _hp;
    public float spawnTime = 20f;
    float curSpawnTime;

    private void OnEnable()
    {
        _hp = hp;
        curSpawnTime = 0;
    }

    private void Update()
    {
        curSpawnTime += Time.deltaTime;
        if (curSpawnTime >= spawnTime)
        {
            curSpawnTime = 0;
            SpawnUnit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hit event by player's missile
        if (collision.tag == "PlayerMissile")
        {
            _hp -= collision.GetComponent<MissileController>().damage;
            Debug.Log(_hp);
            if (_hp <= 0)
            {
                SoundManager.instance.PlaySound("slimeSpawnerBreak");
                this.transform.parent.parent.parent.GetComponent<NodeMap>().GetNodeMap();
                this.gameObject.SetActive(false);
            }
        }
    }

    void SpawnUnit()
    {
        Vector2 spawnPosition = Vector2.zero;

        // ������ �������� �̸��� �������� ��ġ�� ����
        string spawnerName = gameObject.name;

        // SpawnerDown�̶�� y ��ǥ�� -1�� ����
        if (spawnerName == "SpawnerDown")
        {
            spawnPosition = Vector2.down;
        }
        // SpawnerUp�̶�� y ��ǥ�� 1�� ����
        else if (spawnerName == "SpawnerUp")
        {
            spawnPosition = Vector2.up;
        }
        // SpawnerRight�̶�� x ��ǥ�� 1�� ����
        else if (spawnerName == "SpawnerRight")
        {
            spawnPosition = Vector2.right;
        }
        // SpawnerLeft�̶�� x ��ǥ�� -1�� ����
        else if (spawnerName == "SpawnerLeft")
        {
            spawnPosition = Vector2.left;
        }
        GameObject slime = ObjectManager.instance.Activate("slime");
        SoundManager.instance.PlaySound("slimeSpawn");
        slime.transform.parent = this.transform.parent.parent.parent;
        slime.transform.localPosition = this.transform.localPosition + (Vector3)spawnPosition;
    }
}
