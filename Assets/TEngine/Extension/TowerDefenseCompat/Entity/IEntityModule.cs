using System.Collections.Generic;

namespace TEngine
{
    public interface IEntityModule
    {
        int GenerateSerialId();
        void HideEntity(int serialId);
        void HideAllEntity();
        void HideEntity(Entity entity);
        void AddToDic(int serialId, Entity entity);
        void RemoveFromDic(int serialId);
        IEnumerable<Entity> GetAllEntities();
        void Clear();
    }
}
