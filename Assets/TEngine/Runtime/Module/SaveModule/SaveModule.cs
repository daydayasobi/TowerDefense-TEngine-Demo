using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class SaveModule : Module, ISaveModule
    {
        /// <summary>
        /// SQLite数据库辅助类
        /// </summary>
        private SQLiteHelper sql;
        
        public override void OnInit()
        {
            string dbPath = "URI=file:" + Application.persistentDataPath + "/save.db";
            sql = new SQLiteHelper(dbPath);

            // 检查是否存在表
            var reader = sql.ExecuteQuery("SELECT name FROM sqlite_master WHERE type='table' AND name='SaveTable';");
            if (!reader.HasRows)
            {
                sql.CreateTable(
                    "SaveTable",
                    new string[] { "Key", "Value", "Type" },
                    new string[] { "TEXT PRIMARY KEY", "TEXT", "TEXT" }
                );
                Debug.Log("✅ 创建数据表 SaveTable 成功");
            }
            else
            {
                Debug.Log("✅ 数据表 SaveTable 已存在，跳过创建");
            }
            reader.Close();
        }

        public override void Shutdown()
        {
            if (sql != null)
            {
                sql.CloseConnection();
                sql = null;
            }
        }
        
        private void SetValue(string key, string value, string type)
        {
            // 先检查是否存在
            var reader = sql.ExecuteQuery($"SELECT key FROM SaveTable WHERE key='{key}'");
            bool exists = reader.Read();
            reader.Close();

            if (exists)
            {
                sql.ExecuteQuery($"UPDATE SaveTable SET value='{value}', type='{type}' WHERE key='{key}'");
            }
            else
            {
                sql.ExecuteQuery($"INSERT INTO SaveTable (key, value, type) VALUES ('{key}', '{value}', '{type}')");
            }
        }

        private string GetValue(string key)
        {
            var reader = sql.ExecuteQuery($"SELECT value FROM SaveTable WHERE key='{key}'");
            string result = null;
            if (reader.Read())
            {
                result = reader["value"].ToString();
            }
            reader.Close();
            return result;
        }

        public void RemoveAllKey()
        {
            // 清空数据库
            sql.ExecuteQuery("DELETE FROM SaveTable");

            // 清空 PlayerPrefs
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
        
        public int GetInt(string key, int defaultValue = 0)
        {
            string v = GetValue(key);
            if (int.TryParse(v, out int result)) return result;
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0)
        {
            string v = GetValue(key);
            if (float.TryParse(v, out float result)) return result;
            return defaultValue;
        }

        public string GetString(string key, string defaultValue = null)
        {
            string v = GetValue(key);
            return string.IsNullOrEmpty(v) ? defaultValue : v;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            string v = GetValue(key);
            if (v == null) return defaultValue;
            return v == "1" || v.ToLower() == "true";
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
            sql.ExecuteQuery($"DELETE FROM SaveTable WHERE key='{key}'");
        }


        public int GetIntLocal(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }

        public float GetFloatLocal(string key, float defaultValue = 0)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public string GetStringLocal(string key, string defaultValue = null)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }

        public bool GetBoolLocal(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        }

        public void SetIntLocal(string key, int value = 0)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetFloatLocal(string key, float value = 0)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetStringLocal(string key, string value = null)
        {
            PlayerPrefs.SetString(key, value);
        }

        public void SetBoolLocal(string key, bool value = false)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }

        public void RemoveKeyLocal(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
}
