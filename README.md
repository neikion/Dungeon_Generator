# Dungeon_Generator
My Dungeon Generator implementation in Unity.

it used bowyer_watson + prim + a*.

and you can also use <a href="https://github.com/nol1fe/delaunator-sharp">delaunator-sharp</a> instead of my bowyer_watson.

<img src="https://github.com/neikion/Dungeon_Generator/assets/91080792/786e4a03-4fee-4bc5-99d5-e2db0092d571">


# How it works?

The main idea used this project is: <a href="https://www.reddit.com/r/gamedev/comments/1dlwc4/procedural_dungeon_generation_algorithm_explained/"> TinyKeep’s algorithm</a>

1. Generate Rooms
   
    Random Size Rooms Create
   <br/><br/>
3. Separate Rooms

   Separate rooms so they don't overlap
<br/><br/>
5. Set up virtual Hallway
   
   Set up Hallways to connect to the nearest room
<br/><br/>
7. Create Hallway
   
   Create hallways without hallways connecting rooms.<br/>
   And add a few random hallways for fun.
