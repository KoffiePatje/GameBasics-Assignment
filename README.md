# Controls

**Scoreboard View Only**

- Left Arrow - Previous Scoreboard
- Right Arrow - Next Scoreboard
- Up Arrow - Previous Team
- Down Arrow - Next Team 

- O - Switch to Statistics View

**Statistics View Only**

- S - Switch to Scoreboard View

# Configuration 

**Simulation Config (simconfig.json)**

- NumberOfTeams - The number of teams in a single poule
- NumberOfSimulations - The number of simulations to perform in a single application start
- MatchDistributionStrategy - The method of match distribution
  - 'RoundRobin' - Each team faces each other team once, wether they play 'Home' or 'Away' is chosen at random
  - 'DoubleRoundRobin' - Each team faces each other team twice, once in a 'Home' environment, once in an 'Away' environment
- Seed - The random seed used for the next application start, providing the same seed should yield the same results each time, if the seed is set to 0 or lower than the seed is ignored.
- LogSimulationReportsToDisk - Wether the simulator should write the simulation steps to disk (mostly for debugging purposes)

**Tweak Config**

This config contains a bunch of values that allows for tweaking the simulation.
The score values affect the probability that a player will perform a certain action.
The modifier values affect the influence of the players offensive or defensive skill in succesfully completing that action.
THe duration values affect by how much the match time will progress once the action is performed.