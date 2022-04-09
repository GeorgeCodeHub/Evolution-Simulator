# Evolution Simulator in Unity3D

Genetic algorithms belong to the field of computer science and are a method of searching for optimal solutions in systems that can be described as a mathematical problem. They are useful in problems that contain many parameters and there is no analytical method that can find the optimal combination of values for the variables so that the system in question reacts as much as possible in the desired way.

Tools that were used:
- Blender 3D
- Visual Studio
- Unity3D

Finite State Machines, NavMesh, GOAP and Behavior Designer logics were applied for the AI agents.

Note that the user navigation in the project followed the logic of RTS games.

1. WASD - Navigation
2. Mouse - Navigation and selection in space

## Description

The objective was the implementation of genetic algorithms at the synchronous and asynchronous level. Technologies such as artificial intelligence and search algorithms were applied to 3D environments where they enabled the following functions:

- Features for each agent
- The choice of mutations of these characteristics at a synchronous or asynchronous level
- The monitoring of the above in graphs
- The influence of the environment towards the agents

## Video:

[![video-project](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/1.png)](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/LivePresentation.flv "Live Presentation")

---

Below a few images showcasing how the project looks and works:

For starters the application starts from a main menu in which the user can change the settings, or choose to start / end the application.

![mainmenu](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/1.png)

The user has the option of selecting two different environments. In the "LAB" world, agents mutate after specific intervals at the same time. In the "FOREST" world, agents mutate asynchronously and in real time.

![worldselect](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/3.png)

## LAB World

When the user selects to the "LAB" world, a window appears in which they can specify the configuration settings. 
The following items affect the following features:
1. Character Count - The total amount of agents
2. Food Count - The total amount of food available per day
3. Energy Limit - The life time limit of each agent
4. Start Size - The initial detection range / size of each agent
5. Start Speed - The initial speed of each agent
6. Start Quality - The initial level of value of the food
7. Randomness - A numerical means for the influence of randomness

![labmenu](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/4.png)

As soon as the user presses the start button, the corresponding agents and food are created based on their choices.

![labworld](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/5.png)

The user has the option to stop the simulation at any time, to see through graphs how the features of the agents are affected and to change the time speed.

### Speed Graph

![speedgraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/6.png)

### Range / Size Graph

![rangegraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/7.png)
 
### Quality Graph

![qualitygraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/8.png)

## FOREST World

When the user navigates to the "FOREST" world, they will have a corresponding initial window in which they will be able to enter the following features:
1. Character Count - The total amount of agents
2. Food Count - The total amount of food available per day
3. Start Size - The initial detection range / size of each agent
4. Start Speed - The initial speed of each agent
5. Randomness - A numerical indicator for the influence of randomness

![forestmenu](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/10.png)

Once the simulation starts, the agents and food are created in the enviroment.

**Agents also have the ability to mate.**

![forestworld](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/11.png)

### Speed Graph

![speedgraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/12.png)
 
### Range / Size Graph

![rangegraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/13.png)
  
### Total population Graph

![totalgraph](https://github.com/GeorgeCodeHub/Evolution-Simulator/blob/main/Screenshots/14.png)
