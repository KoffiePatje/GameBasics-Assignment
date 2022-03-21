using System;

namespace PouleSimulator
{
    public class SimulationResultViewer
    {
        private readonly ScoreboardSimulationView scoreboardView;
        private readonly StatisticsSimulationView statisticsView;

        private ISimulationResultView currentView;

        public SimulationResultViewer(Scoreboard[] results) {
            scoreboardView = new ScoreboardSimulationView(results);
            statisticsView = new StatisticsSimulationView(results);

            currentView = scoreboardView;
        }

        public void Display() {
            bool exit = false;

            ConsoleKeyInfo keyPress = new ConsoleKeyInfo();

            while(!exit) {
                Console.Clear();

                currentView.Display(keyPress);

                if(currentView == scoreboardView) {
                    Console.WriteLine("Press 'O' for the Overall Statistics Screen");
                } else if(currentView == statisticsView) {
                    Console.WriteLine("Press 'S' for the Scoreboard Screen");
                }

                keyPress = Console.ReadKey();

                switch(keyPress.Key) {
                    case ConsoleKey.Escape: {
                        exit = true;
                        break;
                    }

                    case ConsoleKey.O: {
                        currentView = statisticsView;
                        break;
                    }

                    case ConsoleKey.S: {
                        currentView = scoreboardView;
                        break;
                    }
                }
            }
        }
    }
}
