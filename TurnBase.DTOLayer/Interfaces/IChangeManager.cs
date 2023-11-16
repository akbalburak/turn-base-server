
namespace TurnBase.DTOLayer.Interfaces
{
    public interface IChangeManager
    {
        public void Add(IChangeItem item);
        public void SendAll();
    }
}
