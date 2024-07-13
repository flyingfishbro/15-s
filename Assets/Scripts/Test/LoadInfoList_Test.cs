using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoadInfoList_Test : MonoBehaviour
{
    public List<UnitLoader.UnitModelType> teamModelTypes;
    public List<Unit.HandleDirection> teamHandleTypes;
    public List<string> teamGunCodes;


    public List<UnitLoader.UnitModelType> GetModelTypeList(int size)
    {
        List<UnitLoader.UnitModelType> ret = new List<UnitLoader.UnitModelType>();

        for (int i = 0; i < size; ++i)
        {
            ret.Add(teamModelTypes[i]);
        }

        return ret;
    }


    public List<Unit.HandleDirection> GetHandleList(int size)
    {
        List<Unit.HandleDirection> ret = new List<Unit.HandleDirection>();

        for (int i = 0; i < size; ++i)
        {
            ret.Add(teamHandleTypes[i]);
        }

        return ret;
    }


    public List<string> GetGunCodeList(int size)
    {
        List<string> ret = new List<string>();
        for (int i = 0; i < size; ++i)
        {
            ret.Add(teamGunCodes[i]);
        }

        return ret;
    }


}
