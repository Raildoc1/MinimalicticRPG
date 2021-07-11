using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataBase : ScriptableObject {

    public T[] Load<T>(string LanguageName) where T : ScriptableObject {
        return Resources.LoadAll<T>("Items/Items");
    }

}
