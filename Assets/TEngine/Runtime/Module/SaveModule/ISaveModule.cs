using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public interface ISaveModule
    {
        public int GetInt(string key, int defaultValue = 0);
        public float GetFloat(string key, float defaultValue);
        public string GetString(string key, string defaultValue);
        public bool GetBool(string key, bool defaultValue);
        
        public void SetInt(string key, int value);
        public void SetFloat(string key, float value);
        public void SetString(string key, string value);
        public void SetBool(string key, bool value);
    }
}
