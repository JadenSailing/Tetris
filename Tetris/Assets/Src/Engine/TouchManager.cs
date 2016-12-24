using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TouchManager
{
    private static TouchManager _instance;

    public delegate void OnKeyDown(KeyCode key);

    private Dictionary<KeyCode, List<OnKeyDown>> _keyDownDict;

    public static TouchManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new TouchManager();
            }
            return _instance;
        }
    }

    public void Init()
    {
        _keyDownDict = new Dictionary<KeyCode, List<OnKeyDown>>();
    }
    public void Update()
    {
        if(!Input.anyKeyDown)
        {
            return;
        }
        foreach(var key in _keyDownDict.Keys)
        {
            if(Input.GetKey(key))
            {
                List<OnKeyDown> list = _keyDownDict[key];
                for (int i = 0; i < list.Count; i++)
                {
                    list[i](key);
                }
                return;
            }
        }
    }

    public void Register(KeyCode key, OnKeyDown callback)
    {
        if(!_keyDownDict.ContainsKey(key))
        {
            _keyDownDict[key] = new List<OnKeyDown>();
        }
        List<OnKeyDown> keyList = _keyDownDict[key];
        for (int i = 0; i < keyList.Count; i++)
        {
            if(keyList[i] == callback)
            {
                return;
            }
        }
        keyList.Add(callback);
    }
}
