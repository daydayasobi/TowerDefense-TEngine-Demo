
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;


namespace GameConfig
{
public partial class TbLevelData
{
    private readonly System.Collections.Generic.Dictionary<int, LevelData> _dataMap;
    private readonly System.Collections.Generic.List<LevelData> _dataList;
    
    public TbLevelData(ByteBuf _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, LevelData>();
        _dataList = new System.Collections.Generic.List<LevelData>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            LevelData _v;
            _v = global::GameConfig.LevelData.DeserializeLevelData(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, LevelData> DataMap => _dataMap;
    public System.Collections.Generic.List<LevelData> DataList => _dataList;

    public LevelData GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public LevelData Get(int key) => _dataMap[key];
    public LevelData this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

