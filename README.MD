# Familiar Quest

## Credits

All music and sound effects written, composed, and created by the excellent [James Thorley](https://thorleymusic.co.uk/). Various libraries from third parties are fully attributed in LICENSE.MD

## Description

Familiar Quest is a fast-paced multiplayer action roguelite adventure featuring procedurally generated abilities and cats! The game is still very much a work in progress, but I've been working on it on and off for several years. At some point I acknowledged that this was becoming a "forever project" with near-infinite scope creep, so I decided to open source all of the parts I'm legally entitled to make available to the public in the hopes that others can learn from my mistakes.

### Features

1. Fast-paced action roguelite with a top-down pseudo-isometric perspective. The game is controlled via WASD and allows the player to aim abilities independently of movement by using the mouse.
2. A system wherein new player skills/spells are generated procedurally and can be obtained by killing the enemies that possess them.
3. A procedurally generated boss system, which creates boss patterns based on a pool of boss mechanics. (this is temporarily disabled pending some AI refactoring)
4. Encounter building system that attempts to procedurally scale difficulty based on level, so the game in theory becomes harder as you progress further into the game (this needs a lot of testing still)
5. GOAP (Goal Oriented Action Planning) AI system that uses multithreading and A* to efficiently make goal-based decisions for monsters. (note: system itself is complete, but the full complement of AI Actions and Goals have not yet been added)
6. Custom built, server-based networking using gRPC + Protocol Buffers. (works, but not 100% final)
7. Tons of mistakes made in advance for you, so you don't have to make them!

## Requirements

Runs on Windows, Linux, and Mac, with Android builds confirmed working and coming when I feel like fiddling with game controller controls. Not sure of the exact specs required but you'd probably want a vaguely modern graphics card.

Running the server requires Python 3.x. To install the requirements, navigate to the `server` folder and run `pip install -r requirements.txt`

The game files are compatible with Unity 2019.3.0f1+

## Cloning the repository

This repository uses Git LFS (Large File Storage) and submodules, so there are a few steps involved in getting started:

1. Obtain and install [Git LFS](https://git-lfs.github.com/)
2. Run `git clone https://github.com/caesuric/familiar-quest.git`
3. `cd familiar-quest`
4. Run `git submodule update --init --remote server` to get the server code

The other submodules are unfortunately not available outside of binary releases for EULA/copyright reasons.

## Installing and Running

Download the appropriate binary from [the releases page](https://github.com/caesuric/familiar-quest/releases) and run the executable to get going!

## Playing the Game

* Create a character and set its stats! You can mouse over any stat to see what it affects.
* Choose from a set of procedurally generated abilities that are (mostly) unique to your character!
* Select your character and hit play
* WASD to move character
* Space bar to use items in world (like portals or doors)
* Left/right click to aim and use primary/secondary abilities
* Left/right click on hotbar to reassign primary/secondary abilities
* Number keys to use other abilities
* Mouse scroll to zoom minimap in/out
* Enter a portal to generate a dungeon of the level printed by the portal (strongly recommend level 1 at first :) )
* You will acquire new procedurally generated abilities by defeating enemies with a blue particle effect -- this indicates that they are special "miniboss" enemies that have unique abilities
* At the end of each dungeon is a boss. Normally the boss would draw from a mish-mash of preset mechanics and arrange them in a series of phases, but this is disabled at the moment after a major AI refactor
* Similarly, defeating the boss should open a portal out of the dungeon, but you can avoid walking all the way back at the moment by opening the settings menu and clicking "I'm Stuck!"

## Folder Structure

```
/Art                   -    The few art assets I made myself are here.
/FamiliarQuestMainGame -    The main Unity project
/server                -    The backend Python server
```

### Unity Project Assets Folder Breakdown

```
AnimatorControllers     -   Animator controllers for various characters
External                -   All external assets from the Unity Asset Store live here. Only the Unity Standard assets are included
Music                   -   Music for the game (supplied by James Thorley)
Physics Materials       -   Physics materials
Plugins                 -   Bundled external networking dependencies
PostProcessing          -   Unity Standard Assets visual PostProcessing stack
Postprocessing Profiles -   Postprocessing profiles for various environ types
Resources/Prefabs       -   Various prefabs used by the game
Resources/SFX           -   Sound effects for the game (supplied by James Thorley)
Resources/Text          -   Text assets (mainly tables for rolling random attributes)
Scenes                  -   All Unity scenes are stored here
Scripts                 -   <---- ACTUAL CODE GOES HERE (see below)
Tests                   -   Tests for the code (coverage is currently minimal)
```

### Scripts Breakdown

```
AI                      -   GOAP (Goal Oriented Action Planning) code
Camera                  -   Camera related code
Characters              -   Character related code (PC and NPC)
Characters/Abilities    -   Code related to the ability generation system
Characters/Addons       -   Code that attaches a component to a character at runtime
Characters/Base         -   Components that all characters have
Characters/Caches       -   Caches that store references to elements -- this will be phased out in the future for a central cache instance
Characters/Hitboxes     -   Character hitbox components
Characters/Monsters     -   Monster only scripts
Characters/Saveable     -   Serialization classes for characters
Dungeon                 -   Dungeon related code
Dungeon/LevelGen        -   The main level generation code
Dungeon/Objects         -   Code for specific generated dungeon objects
Items                   -   Code for items and equipment
Objects                 -   Code for objects in the game world
OverlordMode            -   Code for unfinished asymmetric "dungeon master" mode
Overworld               -   Overworld related code (will be folded into another folder in the future)
Proto                   -   Protobuf generated code and the main network communication client class
UI                      -   User interface code
UI/Game Elements        -   Code for UI objects that appear in the game world's 3D space
UI/Game Menus           -   Code for game menus
UI/HUD                  -   Code for the player heads up display
UI/Input                -   Input controller code
UI/Screens              -   Code for various game screens outside the main game (like character creation)
Utils                   -   Various utilities
Utils/Astar             -   A* implementations (both for grid and GOAP)
Utils/PQ                -   PriorityQueue implementation by Daniel "BlueRaja" Pflughoeft
World                   -   World related scripts (like the World Auto Saver)
```

[PriorityQueue implementation by Daniel "BlueRaja" Pflughoeft](https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp)

## Known Issues

* Procedural boss fights are disabled at the moment until I integrate them with the new AI infrastructure
* Similarly, bosses no longer spawn exit portals or guaranteed loot drops
* Multiplayer is disabled temporarily until the system is finished
* Dungeon generation is fairly stripped down at this moment pending some serious thought about how I want it to work
* Abilities cannot be fused to create new abilities at the moment, as the interface for this is not yet integrated into the new UI
* Bits of monsters can be seen on the minimap even around corners
* Item names sometimes have multiple grammatically incompatible modifiers (e.g. "Bracelet of Power of Endurance")
* "Exit Game" button after dying may not work as intended - game may require restart after death
* Balance is somewhat off as health no longer regenerates out of combat. This is working as intended but needs better health restoration items to rebalance it.

## License

MIT licensed for the bulk of the code. See LICENSE.MD for licensing details on bundled libraries.
