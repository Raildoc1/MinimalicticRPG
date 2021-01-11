using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLineMover : MonoBehaviour
{

    public float dessapearTime = 2f;
    public float flySpeed = 200f;
    public float dessapearSpeed = 100f;

    private IEnumerator Start()
    {

        var tmp = GetComponent<TextMeshProUGUI>();

        tmp.alpha = 1f;

        while (dessapearTime > 0f)
        {
            dessapearTime -= Time.deltaTime;

            var pos = transform.position;

            pos.y += Time.deltaTime * flySpeed;

            transform.position = pos;
            
            tmp.alpha -= Time.deltaTime * dessapearSpeed;

            yield return null;
        }

        Destroy(gameObject);
    }

}
