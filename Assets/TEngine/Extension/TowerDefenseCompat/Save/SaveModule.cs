using UnityEngine;

namespace TEngine
{
    public class SaveModule : Module, ISaveModule
    {
        private SQLiteHelper _sqliteHelper;

        public override void OnInit()
        {
            var dbPath = "URI=file:" + Application.persistentDataPath + "/save.db";
            _sqliteHelper = new SQLiteHelper(dbPath);

            var reader = _sqliteHelper.ExecuteQuery("SELECT name FROM sqlite_master WHERE type='table' AND name='SaveTable';");
            if (!reader.HasRows)
            {
                _sqliteHelper.CreateTable(
                    "SaveTable",
                    new[] { "Key", "Value", "Type" },
                    new[] { "TEXT PRIMARY KEY", "TEXT", "TEXT" });
                Debug.Log("SaveTable created.");
            }

            reader.Close();
        }

        public override void Shutdown()
        {
            if (_sqliteHelper != null)
            {
                _sqliteHelper.CloseConnection();
                _sqliteHelper = null;
            }
        }

        public void RemoveAllKey()
        {
            _sqliteHelper.ExecuteQuery("DELETE FROM SaveTable");
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            var value = GetValue(key);
            return int.TryParse(value, out var result) ? result : defaultValue;
        }

        public float GetFloat(string key, float defaultValue)
        {
            var value = GetValue(key);
            return float.TryParse(value, out var result) ? result : defaultValue;
        }

        public string GetString(string key, string defaultValue)
        {
            var value = GetValue(key);
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        public bool GetBool(string key, bool defaultValue)
        {
            var value = GetValue(key);
            if (value == null)
            {
                return defaultValue;
            }

            return value == "1" || value.ToLower() == "true";
        }

        public void SetInt(string key, int value)
        {
            SetValue(key, value.ToString(), "int");
        }

        public void SetFloat(string key, float value)
        {
            SetValue(key, value.ToString(), "float");
        }

        public void SetString(string key, string value)
        {
            SetValue(key, value, "string");
        }

        public void SetBool(string key, bool value)
        {
            SetValue(key, value ? "1" : "0", "bool");
        }

        public void RemoveKey(string key)
        {
            _sqliteHelper.ExecuteQuery($"DELETE FROM SaveTable WHERE key='{key}'");
        }

        public int GetIntLocal(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float GetFloatLocal(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string GetStringLocal(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public bool GetBoolLocal(string key, bool defaultValue)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetIntLocal(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloatLocal(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetStringLocal(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetBoolLocal(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public void RemoveKeyLocal(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        private void SetValue(string key, string value, string type)
        {
            var reader = _sqliteHelper.ExecuteQuery($"SELECT key FROM SaveTable WHERE key='{key}'");
            var exists = reader.Read();
            reader.Close();

            if (exists)
            {
                _sqliteHelper.ExecuteQuery($"UPDATE SaveTable SET value='{value}', type='{type}' WHERE key='{key}'");
            }
            else
            {
                _sqliteHelper.ExecuteQuery($"INSERT INTO SaveTable (key, value, type) VALUES ('{key}', '{value}', '{type}')");
            }
        }

        private string GetValue(string key)
        {
            var reader = _sqliteHelper.ExecuteQuery($"SELECT value FROM SaveTable WHERE key='{key}'");
            string result = null;
            if (reader.Read())
            {
                result = reader["value"].ToString();
            }

            reader.Close();
            return result;
        }
    }
}
