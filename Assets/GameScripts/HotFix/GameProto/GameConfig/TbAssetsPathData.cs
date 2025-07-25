
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
public partial class TbAssetsPathData
{
    private readonly System.Collections.Generic.Dictionary<int, AssetsPathData> _dataMap;
    private readonly System.Collections.Generic.List<AssetsPathData> _dataList;
    
    public TbAssetsPathData(ByteBuf _buf)
    {
        _dataMap = new System.Collections.Generic.Dictionary<int, AssetsPathData>();
        _dataList = new System.Collections.Generic.List<AssetsPathData>();
        
        for(int n = _buf.ReadSize() ; n > 0 ; --n)
        {
            AssetsPathData _v;
            _v = global::GameConfig.AssetsPathData.DeserializeAssetsPathData(_buf);
            _dataList.Add(_v);
            _dataMap.Add(_v.Id, _v);
        }
    }

    public System.Collections.Generic.Dictionary<int, AssetsPathData> DataMap => _dataMap;
    public System.Collections.Generic.List<AssetsPathData> DataList => _dataList;

    public AssetsPathData GetOrDefault(int key) => _dataMap.TryGetValue(key, out var v) ? v : null;
    public AssetsPathData Get(int key) => _dataMap[key];
    public AssetsPathData this[int key] => _dataMap[key];

    public void ResolveRef(Tables tables)
    {
        foreach(var _v in _dataList)
        {
            _v.ResolveRef(tables);
        }
    }

}

}

