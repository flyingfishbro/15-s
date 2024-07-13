using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunInfo", menuName = "Scriptable/GunInfo")]
public class GunInfoScriptableObject : ScriptableObject
{
    public List<GunInfo> gunInfo;

    public GunInfo FindGunInfo(string id)
    {
        id = id.ToLower();
        foreach (GunInfo info in gunInfo)
        {
            if (info.id.Equals(id)) return info;
        }
        return null;
    }


}
