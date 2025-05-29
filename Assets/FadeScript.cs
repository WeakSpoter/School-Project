using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FadeScript : MonoBehaviour
{
    public Image Panel;
    float time = 0f;
    float F_time = 1f;
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
}
