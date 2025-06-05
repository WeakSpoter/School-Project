using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Door : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 1f;
    public bool isOpen = false;
    bool flag = false;
    public DoorPos pos;
    public Sprite open;
    public Sprite close;
    SpriteRenderer render;
    Collider2D col;
    Camera cam;
    GameObject minimapCam;
    GameObject player;


    private void Awake()
    {
        render = this.GetComponent<SpriteRenderer>();
        col = this.GetComponent<Collider2D>();
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        minimapCam = GameObject.Find("MinimapCam");
    }

    private void Update()
    {
        if (flag && Input.GetKeyDown(KeyCode.E))
        {
            UseDoor(player);
            SoundManager.instance.PlaySound("DoorOpen");
        }
}

    public void DoorOpen()
    {
        isOpen = true;
        render.sprite = open;
        col.isTrigger = true;
    }

    public void DoorClose()
    {
        isOpen = false;
        render.sprite = close;
        col.isTrigger = false;
    }

    public void fade()
    {
        {
            StartCoroutine(FadeFlow());
        }

        IEnumerator FadeFlow()
        {
            yield return new WaitForSeconds(0f);
            Panel.gameObject.SetActive(true);
            time = 0f;
            Color alpha = Panel.color;
            while (alpha.a < 1f)
            {
                time += Time.deltaTime / F_time;
                alpha.a = Mathf.Lerp(0, 1, time);
                Panel.color = alpha;
                yield return null;
            }
            time = 0f;



            while (alpha.a > 0f)
            {
                time += Time.deltaTime / F_time;
                alpha.a = Mathf.Lerp(1, 0, time);
                Panel.color = alpha;
                yield return null;
            }
            Panel.gameObject.SetActive(false);
            yield return null;

        }
    }


    public void UseDoor(GameObject player)
    {
      
        Player pLogic = player.GetComponent<Player>();
        Room rInfo = pLogic.curRoom.GetComponent<Room>();
        int idx = rInfo.conDoors.IndexOf(pos);
        Transform nextRoom = rInfo.conRooms[idx].transform;
        if (isOpen)
        {
            switch (pos)
            {
                case DoorPos.Up:
                    player.transform.position += Vector3.up * 6;
                    break;
                case DoorPos.Down:
                    player.transform.position += Vector3.down * 6;
                    break;
                case DoorPos.Left:
                    player.transform.position += Vector3.left * 6;
                    break;
                case DoorPos.Right:
                    player.transform.position += Vector3.right * 6;
                    break;
            }
            nextRoom.gameObject.SetActive(true);
            player.transform.parent = nextRoom;
            pLogic.minimapMask.transform.position = pLogic.curRoom.position;
            cam.transform.position = nextRoom.position + new Vector3(0.5f, 1.5f, -1);
            minimapCam.transform.position = new Vector3(nextRoom.position.x, nextRoom.position.y, minimapCam.transform.position.z);
            if (GameManager.instance.playState != PlayState.InPlay) SceneChanger.ToPlay();

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        player = collision.gameObject;
        flag = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        flag = false;
    }
}
