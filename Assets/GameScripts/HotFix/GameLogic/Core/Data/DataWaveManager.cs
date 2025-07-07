using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class DataWaveManager : Singleton<DataWaveManager>
    {
        private Dictionary<int, WaveDataBase> dicWaveData;
        private Dictionary<int, WaveElementDataBase> dicWaveElementData;

        public void OnInit()
        {
        }

        public void OnLoad()
        {
            // dtWave = GameEntry.DataTable.GetDataTable<DRWave>();
            // if (dtWave == null)
            //     throw new System.Exception("Can not get data table Item");
            //
            // dtWaveElement = GameEntry.DataTable.GetDataTable<DRWaveElement>();
            // if (dtWaveElement == null)
            //     throw new System.Exception("Can not get data table ItemGroup");

            dicWaveData = new Dictionary<int, WaveDataBase>();
            dicWaveElementData = new Dictionary<int, WaveElementDataBase>();

            List<WaveElementData> dRWaveElements = new List<WaveElementData>(WaveElementDataLoader.Instance.GetAllItemConfig());
            foreach (var dRWaveElement in dRWaveElements)
            {
                if (dicWaveElementData.ContainsKey(dRWaveElement.Id))
                {
                    Log.Error("WaveElement id duplicate:{0}.", dRWaveElement.Id);
                    continue;
                }

                dicWaveElementData.Add(dRWaveElement.Id, new WaveElementDataBase(dRWaveElement));
            }

            List<WaveData> dRWaves = new List<WaveData>(WaveDataLoader.Instance.GetAllItemConfig());
            foreach (var dRWave in dRWaves)
            {
                List<int> waveElementRange = new List<int>(dRWave.WaveElements);
                if (waveElementRange.Count != 2)
                    throw new System.Exception(string.Format("Wave data 'WaveElements' length error,current is '{0}', should be 2", waveElementRange.Count));

                int startIndex = waveElementRange[0];
                int endIndex = waveElementRange[1];

                if (endIndex < startIndex)
                    throw new System.Exception("Wave element index invaild,EndIndex should smaller than StartIndex.");

                WaveElementDataBase[] waveElementDatas = new WaveElementDataBase[endIndex - startIndex + 1];

                int index = 0;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    WaveElementDataBase waveElementData = null;
                    if (!dicWaveElementData.TryGetValue(i, out waveElementData))
                    {
                        throw new System.Exception("Can not find WaveElementDat id :" + i);
                    }

                    waveElementDatas[index++] = waveElementData;
                }

                WaveDataBase tempWaveData = new WaveDataBase(dRWave, waveElementDatas);
                dicWaveData.Add(dRWave.Id, tempWaveData);
            }
        }

        public WaveDataBase GetWaveData(int id)
        {
            if (dicWaveData.ContainsKey(id))
            {
                return dicWaveData[id];
            }

            return null;
        }

        public WaveDataBase[] GetAllWaveData()
        {
            int index = 0;
            WaveDataBase[] results = new WaveDataBase[dicWaveData.Count];
            foreach (var waveData in dicWaveData.Values)
            {
                results[index++] = waveData;
            }

            return results;
        }

        public WaveElementDataBase GetWaveElementData(int id)
        {
            if (dicWaveData.ContainsKey(id))
            {
                return dicWaveElementData[id];
            }

            return null;
        }

        public WaveElementDataBase[] GetAllWaveElementData()
        {
            int index = 0;
            WaveElementDataBase[] results = new WaveElementDataBase[dicWaveElementData.Count];
            foreach (var waveElementData in dicWaveElementData.Values)
            {
                results[index++] = waveElementData;
            }

            return results;
        }

        public void OnUnload()
        {
            dicWaveData = null;
            dicWaveElementData = null;
        }

        public void OnShutdown()
        {
        }
    }
}