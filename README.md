# Vestiges Of The Empire - City Builder Game
---
## <a href="https://lucasgarcia002.itch.io/vestiges-of-the-empire"> Play now </a>

<img src="./TitlesforREADME.png" width=50%>

## Description
Create and manage your own thriving city, ensuring your citizens’ well-being by meeting all their needs.

> Inspired by other types of games such as such as the Tropic saga, Cities Skylines or the Imperivm saga.

## Principal features established

### Procedural Generation
 Code in: /Assets/Scripts/Map

  * -Generate a grid with n^n cells.
  * -Generate a random mesh with terrains for water and for grass via seeds (an island).
  * -Generate different heatmaps using the cells in the grid, such as Tropico does. Extractive Structures will use this heatmaps for producing resources.
  * -Displays rivers that crosses the island using the A* algorithm.
  * -Displays trees throughout the island.
  
---

### Structures/Buildings Logic
 Code in: /Assets/Scripts/Structures

#### Roads
 Code in: /Assets/Scripts/Structures/Road

  * -Roads uses A* search algorithm when the player uses drag for placing a road from one point to another. The A* searches the shortest path and then setups the roads in the world.
  * -The Road Fixer script changes the orientation for all the roads prefab placed, in runtime or already placed. Also it sets three or four ways for roads whose "neighbours" are more than two.
  * -Generate different heatmaps using the cells in the grid, such as Tropico does. Extractive Structures will use this heatmaps for producing resources.
  * -It allows IA behaviour walking.
  
#### Elaborated/Extractive Resources Buildings
 Code in: /Assets/Scripts/Structures/Elaborated Resources
 Code in: /Assets/Scripts/Structures/Extractive Resources

  * -All this buildings search workers every "x" seconds in the near radius defined by code. The workers need to fulfill the demands set by the building (primarly the social status).
  * -Satisfies all the demands in the near buildings which needs the resource elaborated.
  * -The elaborated resources buildings need an amount of primary resource for performing its function.
  * -It allows IA behaviour walking.
  * -Can't work if they are not connected to a road in the cell which is placed the door for the building.

#### Houses
 Code in: /Assets/Scripts/Structures/Houses

  * -Allows different houses prefabs which produces different types of people and costs different prices.
  * -Satisfies the demands of the people who are living under the same roof.
  * -Sets the names and surnames using a list of possible options.
  * -Can't work if they are not connected to a road in the cell which is placed the door for the building.
  
---

### People Logic
 Code in: /Assets/Scripts/People

  * -Sets and enum for the three different types of people: plebeian, freedman and patricians.
  * -Sets an employ for the people where they have to go to the job buildings.
  * -Also they have the logic for satysfing demands.
  * -Here it is all the logic for walking, animations, searching targets and rotating the gameobject.
  
---

### UI
 Code in: /Assets/Scripts/UI
 Code in: /Assets/Scripts/Interfaces

  * -The script IShopInterface defines the interfaces/contracts for buying items from the shop.
  * -The radial menu is generated from zero via code (RingMenu.cs) with scriptableObjects used for every slice. This slices can contain a path to other radial menu that can be interactable and selected with input from the player.
  * -The radial menu is updated in runtime showing the hierarchy of radialmenus. F.example: when the extractive resources slice is clicked, then this radial menu is hidden and the extractive resources radial menu is visible now.
  * -In /Assets/Scripts/UI/InfoMissions and /Assets/Scripts/UI/ShowStats are all the logicfor the windows that show the mission objectives and actual stats. They are a simple refresh for all the things that are available in the GameManager
  * -Also it is implemented a simple inspector that shows the description of the building which is going to be built.

---

### Principal Directories 

* **AstarPathfindingProject:** External asset for generating the rivers in the procedural generation.
* **Characters:** Contains the prefabs for the characters.
* **ExternalAssets:** Contains the external 3D models used for all the game.
* **Resources:** Folder that is load via code. 
* **Scripts:** All the scripts commented above.
* **Shaders:** All the shaders generated for the game.

---

### TODO

* **IA:** The IA for the characters need to be updated and completed.
* **Enter of the houses:** All the buildings need to have the enter door selected correctly.
* **Optimization of the code:** ALL the code needs to be more legible and updated.Also the scipts need to be shorter and the variables can't be all public.

---
