using TEngine;

namespace GameLogic
{
    [EventInterface(EEventGroup.GroupUI)]
    public interface ILoginUI
    {
        void ShowLoginUI();

        void CloseLoginUI();
    }
}