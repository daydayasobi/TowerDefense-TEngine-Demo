using Luban;
using GameConfig;
using TEngine;
using UnityEngine;
using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine;


namespace GameLogic
{
    /// <summary>
    /// 配置加载器。
    /// </summary>
    public class ConfigSystem : MonoBehaviour
    {
        public static ConfigSystem _instance;
        public static ConfigSystem Instance => _instance ??= new ConfigSystem();
    
        private bool _init = false;

        private Tables _tables;

        public Tables Tables
        {
            get
            {
                if (!_init)
                {
                    Load();
                }

                return _tables;
            }
        }

        /// <summary>
        /// 加载配置。
        /// </summary>
        public void Load()
        {
            _tables = new Tables(LoadByteBuf);
            _init = true;
        }

        /// <summary>
        /// 加载二进制配置。
        /// </summary>
        /// <param name="file">FileName</param>
        /// <returns>ByteBuf</returns>
        private ByteBuf LoadByteBuf(string file)
        {
            TextAsset textAsset = GameModule.Resource.LoadAsset<TextAsset>(file);
            byte[] bytes = textAsset.bytes;
            GameModule.Resource.UnloadAsset(textAsset);
            return new ByteBuf(bytes);
        }
    }
}
