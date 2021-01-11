using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLineSpawner : MonoBehaviour
{
    #region Singleton

    public static DebugLineSpawner instance;

    private void InitSingleton()
    {
        if (instance)
        {
            Debug.LogError("More than one instance of PlayerInventory found!");
            Destroy(this);
            return;
        }

        instance = this;
    }

    #endregion

    public GameObject linePrefab;

    [Header("Debug")]
    public bool show = false;

    private void Awake()
    {
        InitSingleton();
    }

    private void Update()
    {
        if (show)
        {
            show = false;

            ShowMessage("TEST");
        
        }
    }

    public void ShowMessage(string message)
    {
        var line = Instantiate(linePrefab);

        line.transform.SetParent(transform);

        var rectTransform = line.GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(0, -200, 0);

        var tmp = line.GetComponent<TextMeshProUGUI>();

        tmp.alpha = 1;

        tmp.text = message;
    }
}
