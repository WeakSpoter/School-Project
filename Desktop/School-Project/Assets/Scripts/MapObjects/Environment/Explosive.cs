using System.Collections.Generic;
using UnityEngine;


public class Explosive : MonoBehaviour
{
    public float hp = 9; // OakJar ������Ʈ�� ü��
    public float radius;
    public float damage;
    public GameObject effect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� �̻��ϰ� �浹���� ��
        if (collision.gameObject.CompareTag("PlayerMissile"))
        {
            // OakJar�� ü�� ����
            MissileController mLogic = collision.GetComponent<MissileController>();
            hp -= mLogic.damage;
            Debug.Log("OakJar HP: " + hp);

            // OakJar�� ü���� 0 ������ ��
            if (hp <= 0)
            {
                Collider2D[] objects = Physics2D.OverlapCircleAll(this.transform.position, radius, LayerMask.GetMask("Wall"));
                foreach(Collider2D obj in objects)
                {
                    if (obj == null) continue;
                    Breakable objLogic = obj.GetComponent<Breakable>();
                    if(objLogic != null)
                    {
                        objLogic._hp -= damage;
                    }
                }
                Collider2D[] enemies = Physics2D.OverlapCircleAll(this.transform.position, radius, LayerMask.GetMask("Enemy"));
                foreach (Collider2D enemy in enemies)
                {
                    if (enemy == null) continue;
                    Enemy eLogic = enemy.GetComponent<Enemy>();
                    if (eLogic != null)
                    {
                        eLogic.Hit(damage);
                    }
                }

                Collider2D player = Physics2D.OverlapCircle(this.transform.position, radius, LayerMask.GetMask("Player"));
                if(player != null)
                {
                    player.GetComponent<Player>().Hit(2, this.gameObject);
                }
                // OakJar ������Ʈ �ı�
                Destroy(gameObject);
                Instantiate(effect, this.transform.position, Quaternion.identity);
                SoundManager.instance.PlaySound("explosion");
                this.transform.parent.parent.parent.GetComponent<NodeMap>().GetNodeMap();
            }
        }
    }
}
