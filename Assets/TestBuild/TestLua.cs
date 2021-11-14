using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLua : MonoBehaviour
{
    LuaState lua = null;


    // Start is called before the first frame update
    void Start()
    {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);
        //  InitalizeLua();
        TryDoFile("test");
        if (IntPtr.Size == 4)
        {
            //TryDoFile("test_origin_luajit_win32");
            TryDoFile("test_win32");
            //TryDoFile("test_win32");
        }
        else
        {
            //TryDoFile("test_origin_luajit_win64");
            TryDoFile("test_win64");
            TryDoFile("test_mac64");
        }
    }

    void InitalizeLua()
    {
        lua = new LuaState();
        lua.Start();

    }
    void TryDoFile(string file)
    {
        try
        {
            Debug.Log("----- Begin lua: " + file);
            DoFile(file);
            lua.DoFile(file);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        } 
        Debug.Log("----- End");
    }

    void DoFile(string fileName)
    {
        byte[] buffer = null;

        string path = "Lua/" + fileName + ".lua";
        var txt = Resources.Load<TextAsset>(path);
        if (txt)
            buffer = txt.bytes;

        if (buffer == null)
        {
            string prefix = fileName;
            if (fileName.EndsWith(".lua"))
            {
                prefix = fileName.Substring(0, fileName.Length - 4);
            }
            prefix = prefix.Replace('.', '/');
            txt = Resources.Load<TextAsset>("Lua/" + fileName);
            if (txt)
                buffer = txt.bytes;
            if (buffer == null)
            {
                txt = Resources.Load<TextAsset>("Lua/" + prefix);
                if (txt)
                    buffer = txt.bytes;
            }
        }


        lua.LuaLoadBuffer(buffer , fileName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
