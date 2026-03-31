namespace TEngine
{
    public interface ISaveModule
    {
        void RemoveAllKey();
        int GetInt(string key, int defaultValue = 0);
        float GetFloat(string key, float defaultValue);
        string GetString(string key, string defaultValue);
        bool GetBool(string key, bool defaultValue);
        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetString(string key, string value);
        void SetBool(string key, bool value);
        void RemoveKey(string key);
        int GetIntLocal(string key, int defaultValue = 0);
        float GetFloatLocal(string key, float defaultValue);
        string GetStringLocal(string key, string defaultValue);
        bool GetBoolLocal(string key, bool defaultValue);
        void SetIntLocal(string key, int value);
        void SetFloatLocal(string key, float value);
        void SetStringLocal(string key, string value);
        void SetBoolLocal(string key, bool value);
        void RemoveKeyLocal(string key);
    }
}
