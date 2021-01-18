using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassGenerator : MonoBehaviour
{

    public float radius = 1f;
    public int prefabsAmount = 10;

    public GameObject prefab;

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 0.5f);
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.up, radius);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Gizmos.DrawLine(transform.position, hit.point);
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(hit.point, hit.normal, radius);
        }

    }

    public void Draw()
    {

        if (!prefab)
        {
            Debug.Log("Prefab is null!");
            return;
        } 

        for (int i = 0; i < prefabsAmount; i++)
        {
            var t = Random.Range(0f, 1f);
            var phi = Random.Range(0f, 2f * Mathf.PI);

            var x = radius * t * Mathf.Cos(phi);
            var y = radius * t * Mathf.Sin(phi);

            var origin = transform.position + new Vector3(x, 0f, y);

            RaycastHit hit;

            if (Physics.Raycast(origin, -transform.up, out hit))
            {
                var obj = Instantiate(prefab, hit.point, Random.rotation);
                obj.transform.up = hit.normal;
            }
        }

    }

}
