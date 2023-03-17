# TowerDefense (Unity StarterKit)
<img src="https://img.shields.io/github/license/frangam/towerdefense.svg"> [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.7741254.svg)](https://doi.org/10.5281/zenodo.7741254)
 <img src="https://img.shields.io/github/release/frangam/towerdefense.svg"/> [![GitHub all releases](https://img.shields.io/github/downloads/frangam/towerdefense/total)](https://github.com/frangam/TowerDefense/releases/download/1.0/TowerDefenseTest_Web.zip)

A scalable Tower Defense game framework made with [Unity3D] in C# with a basic AI avoids turrets.
That is like a starter kit for creating your own Tower Defense game.

All the code is commented to understand how it works and is well structured in order to be readable.

## Download last release
Support us downloading our last release
- Click on [TowerDefense](https://github.com/frangam/TowerDefense/releases/download/1.0/TowerDefenseTest_Web.zip) or on Downloads counter [![GitHub all releases](https://img.shields.io/github/downloads/frangam/towerdefense/total)](https://github.com/frangam/TowerDefense/releases/download/1.0/TowerDefenseTest_Web.zip)
- Also, you could support us on ZENODO site: [![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.7741254.svg)](https://doi.org/10.5281/zenodo.7741254)

# Game Scene
The Game scene is called "Demo" and it is localed in “Scenes” folder.

# Controls
- W, A, S, D: move camera.
- Right Mouse: rotate camera.
- Scroll Mouse: zoom camera.
- ESC: reset view.

# Screenshots
<img src="http://i.imgur.com/2Hgjq4V.png" width="800"/>

# Grid Generation
There are too many ways to implement this. We decided to do it by instantiating prefabs that represent each cell with a "Quad" component 
attached. We chose this because it is simple and does not require a higher performance. However, the best way could be to apply the voxel 
technique, but it takes a very more time to implement and for our purpose the chosen form is valid.

We have registered all instantiated cells in an array to access whenever I want to do something with them.

## GridGenerator
We apply singleton pattern to having a unique instance of this manager.
In the demo scene we find “GridGenerator” game object handles this process. We can modify in the inspector some attributes I present in 
the following table:

| Attribute | Description |
| :---         |     :--- |   
| Width   | This is the width of the grid.    |   
| Height    | This is the height of the grid.     |
|Cells Pbs|This is an array with all of the different cells we have (prefabs).|
|Enemies Spawner Coords|This is an array with all of the cells coordinates when we want to enemy forces spawn.|
|Crystals Coords|This is an array with all of the cells coordinates when we want to there are crystals.|

We have a personalized exception (“RepeatedCoordsException”) to control enemy and crystal cells whose coordinates are not equal.

## Cells
Every tile prefab has two materials, one for render a grid around it and other one for the own color of cell.
We have four types of cells:
1. Normal: a basic cell where we can put turrets.
2. Bound: represents a bound of the grid and we cannot put any turret on this.
3. Enemy: where enemies are spawned.
4. Crystal: where crystals are located.

We can see the class hierarchy in the following UML diagram:
<img src="http://i.imgur.com/DpzMcI7.png" width="700"/>

We have considered a cell like a node that has two coordinates to locate it easily in the grid array of GridGenerator. 
Also, we can know if it is walkable, that is, an enemy can walk through it. This is useful for the Pathfinding implemented, 
the same way it is important to know neighbors nodes so we have an array with them.

We use delegates and events in Cell class to notify when we put a turret because enemies need to recalculate their path to continue 
moving and skip this cell.

We use delegates in Crystal Cell to notify enemies when a crystal cell has no more crystals and they recalculate their path again 
choosing a new target crystal cell.

# Turrets
This is the turrets class hierarchy:
<img src="http://i.imgur.com/xWCwm1W.png" width="700"/>

A Turret game object has a sphere collider component that is fired when an enemy collides with it, then it is added to the turret’s target. 
Enemies that are in range are put in a target’s pool sorted by natural order criterion of Enemy class. Also, turrets has a price to buy 
them.

Shot method is called in the Unity’s Update every frame. To control that each type of turret shot with an specific rate it has “shotRate” 
parameter. In Laser Turret case, this attribute is set to zero to apply damage to target enemy in every frame. Cannon Turret shots bullets 
with a specific delay that if they impact to the target, then they apply the damage and, if not, bullets are destroyed when are out of the 
turret’s range.

# Units
We present class diagram of units handling:
<img src="http://i.imgur.com/Gqx6xBe.png" width="700"/>

Enemy class implements “CompareTo” method from System.IComparable to establish the natural order criterion for an enemy, that is, which 
enemy is closer to catch its crystal. This is useful when a turret add target’s enemies to its pool attribute, because it sorts its list 
with this criterion to shot the nearest enemy that gets its crystal.

## Waves
We have implemented a Waves system to spawn units:
<img src="http://i.imgur.com/jRKpB7Q.png" width="700"/>

## Pathfinding with BFS Algorithm
We have used [BFS algorithm](http://en.wikipedia.org/wiki/Breadth-first_search#Pseudocode) pseudo code to implement pathfinding.

I register every previous non-visited node to go from it to its neighbour, so when finish BFS in target node, I have to retrieve them 
inverted. At the finish, I revert the final path to be prepared in the correct order just to enemy starts moving.



 [Unity3D]: <https://unity3d.com>
