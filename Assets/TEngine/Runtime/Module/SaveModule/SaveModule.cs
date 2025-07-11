using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class SaveModule : Module, ISaveModule
    {
        public override void OnInit()
        {
        }

        public override void Shutdown()
        {
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string GetString(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetInt(string key, int value = 0)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloat(string key, float value = 0)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetString(string key, string value = null)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetBool(string key, bool value = false)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}