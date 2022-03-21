using System;

namespace PouleSimulator
{
    public interface ISimulationResultView
    {
        void Display(ConsoleKeyInfo keyPress);
    }
}
