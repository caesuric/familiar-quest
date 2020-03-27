# v0.14

## Equipment updates

* Rehauled attribute system -- secondary attribute dependencies are now scaled completely differently
* Equipment of Masterwork (orange) quality or better is now generated with secondary stats in addition to primary stats!
* There is now a sample shop in the starting area at which you can buy and sell equipment
* Tooltips now show the quality type of the item (Normal - white, High Quality - red, Masterwork - orange, Enchanted - yellow, Rare - green, Artifact - blue, Legendary - indigo, Transcendent - purple)
* Equipment names now vary based on the item quality and the highest two stats of the item - there are more than 130 name modifiers!
* Dropped gear is no longer necessarily tied to the player's primary stat. This is a test to see if I have made other stats valuable enough and may be reverted eventually.
* Fixed a race condition with character loading

### Known Issues

* Attributes effects on character creation screen are vaguely correct regarding what they do, but the exact numerical results may vary from the displayed results. Once you start the game, the secondary stats on the character creation pane are accurate.
* Potions will continue to display on your hotbar even after you have used them all.

# v0.13.1

## Minor bug fixes

* Character sheet and inventory windows no longer overlap by default when opened
* Canceling character creation now properly resets the stat displays on the stat selection screen
* Energy Wisplets have been disabled for now until I update the AI that used to make them flee the player
* Scrolling the mouse wheel no longer zooms the minimap when the cursor is over UI elements
* The drops panel in the upper-left hand corner now properly clears when the inventory or abilities windows are open
* The Save & Exit button in the settings window now works as intended

# v0.13

## Procedural boss fights

* Fixed procedural boss fights so they work again with the new AI system
* Fixed an error where the "delete character" button on the character creation screen didn't work.

# v0.12

## Miscellaneous Refactoring

* Released code in public repo!
* Various bits of code cleanup and restructuing related to public release
* Separated project into git submodules to avoid EULA violations
* Updated to Unity 2019.3
* Bosses no longer spawn old cube monsters as minions
* Individual dungeon rooms are now smaller
* Health bars have been updated to look nicer and provide better information - the color of the level badge now indicates monster quality, with black indicating normal, silver indicating strong, gold indicating elite, red indicating a miniboss, and purple indicating a boss
* Fixed infinite loop in level generation
* Update item tooltips
* Started reimplementing networking (not complete)
* Started working on goal-based AI system (not complete)
* Added system for managing characters
* Added in-game menu for managing game resolution and graphics settings
* Numerous bugfixes

## Known Issues

* Bosses do not currently engage in boss behaviors
* Network play is currently incomplete and not recommended

# v0.11.2

## Bug fixes

* Can no longer see torch fire through walls
* Fixed some graphical artifacts with walls
* Fixed exit game button that appears upon death
* Fixed an issue with goblins and the new lighting that made them flash in a bright, strobing fashion

# v0.11.1

## Beginnings of new map generation system

* Game now generates enemy castles instead of generic block dungeons!
* You will no longer get the same dungeon layout repeatedly when exiting and entering dungeons
* The player HUD should run much better now, as it is persisted between scenes rather than re-initializing for each scene
* Switching floors should be much faster, as the game no longer changes scenes to do so.
* Minor details of floors will no longer be rerolled when going up/down stairs, as the random seeds used to store them are now saved and reused
* Tripled size of HUD

# v0.11

## Gameplay adjustments

* Bosses now have 20x the health they did previously
* Items now automatically add some amount of constitution if they have stats
* Monster scaling adjusted
* Adjust Fighter and Paladin ability balance
* Abilities no longer generate with a description that indicates level 1 when they are of a higher level
* Ability fusion results are now capped at your current level
* You can no longer generate paralyzing attacks that have no cooldown
* Fused ability MP costs should now be correct for the final level of the created ability

# v0.10

## Bosses

* Bosses now always drop a piece of equipment that is Enchanted quality (yellow) or better
* Bosses now spawn an exit portal when defeated
* Bosses play a fanfare when they appear!
* Minibosses play a smaller fanfare when they appear!
* Bosses now have a full set of randomized mechanics!
* Fixed an issue with going back downstairs after leaving a dungeon from the third floor

# v0.9

## The Big Monster Revamp

* Added many new monsters and rebalanced and changed levels on all the old ones!
* Most monsters now have elemental strengths/weaknesses that affect damage taken
* Knockback is much much less likely to knock you through a wall now
* Fixed an issue where fused abilities would end up with a default AOE animation instead of the proper animation for the element
* Fixed a ton (hopefully all) of the monster animations that were broken
* Ranged enemies now properly run away or strafe while attacking you

# v0.8

## Game breaking bug fixes & QoL improvements

* You can now walk through fountains again
* Ability crafting guide should display properly
* Doors no longer push you as they open
* Stat scaling is no longer based on equipment
* Minimap should work properly across different levels now
* Added a "Save & Exit" button to the Settings menu
* You can now filter items by "primary stat upgrade"
* You can now filter abilities by description text

# v0.7

## Misc. fixes

* Fixed a bug where monsters could only drop level 1 equipment. Prepare to be even more overpowered until I address scaling!
* It is no longer possible to fuse an ability with itself
* Dust now uses text list instead of icons
* The same status effect should no longer stack, with the exception of DOTs, HOTs, and mp over time effects. If the new status effect has the same or greater degree of effect, it will replace the old one.
* Settings menu has a basic guide to ability crafting
* Fusion menu now lets you clear fusion slots by clicking them
* Floating text will now move faster the longer it is on the screen (we'll see how sane this system looks in practice)
* Trap floating text now displays in orange
* There is now an in-game version number and changelog! (it looks sorta like this)
* Pressing ESC now clears mouseover panes as well
* Resist floating text will now tell you what kind of condition was resisted
* Mimics and animated statues are now better at being sneaky. They don't look around or engage in idle behaviors until you close to a certain distance. However, they still currently investigate noises (this may change)
* Mimic and animated statue healthbars now appear while they are in combat
* You now choose when to exit after dying (instead of leaving the game after 5 seconds)
* Knockback aoes now work correctly
* Fixed an issue from the new world object usage system that made it so secret doors no longer disappeared
* Made monster LOS and object hiding code LOS more consistent. Should fix a lot of "seeing things through walls" bugs.

# v0.6

## Fixes from last update + world updates + monster fixes!

* Fusion toggles should now reset properly after changing fusion circumstances
* Radius now affects fusion output
* Should now be able to enhance abilities from hotbar
* Fixed issue where merged abilities always reset to level 1 (but didn't lose stats)
* Changed how luck scales
* Changed how items are used - they now use an action button, with a prompt when you're nearby
* Fixed the occasional empty chest bug (probably)
* Starting room now has a brief explanation of the game scenario in the form of a lore item
* Added a system for reading in-game text
* It should no longer be possible to start in an open room. Note that it is still possible for monsters to open the door to the starting room, however, so beware!
* Spider hitbox size reduced by about half
* Fixed issue where imp level/quality text would display above their health bars rather than on them
* Doubled the physics calculations per frame (6 solver iterations -> 12 in physics settings) to try to prevent players from being knocked through walls. Note that this may cause a performance hit on slower systems. Needs some testing.

# v0.5

## Ability fixes!

* Fixed sneaky issue where melee attacks weren't using the attack's damage at all. They were using the character's base weapon damage (unused anywhere else in the game at this time). As a consequence, things like backstab multipliers also weren't working.
* Toned down stealth visual effect
* Attack abilities now scale with the level of the monster that dropped them or the level they dropped on
* Most utility effects now scale based on the points the ability generation algorithm spent on them
* Abilities with cooldowns are much better now
* Upgraded the fusion system to allow player options during fusion
* Added a "dusting" system that allows players to dust their abilities and apply that dust to other abilities

# v0.4

## UI/UX fixes!

* Fixed issue where an invisible box on the left side of the screen blocked ability clicks
* Renamed Ochre Jellies to Slimes because the model being used isn't ochre
* Fixed issue where Slimes didn't display their level/quality on their health bars
* Improved UI scaling
* Added a controls reference to the settings menu
* Weapons should no longer show a green arrow when they are downgrades
* Minimap mapping should be much improved!
* Dragging abilities to/from ability menu no longer uses your active left-click ability constantly
* Utility abilities should now display the numeric amounts that they heal/restore MP/shield for.
* You can now swap bracelet slots by dragging them.
* Intro menus should work so that you can keep playing after death without having to close and restart the game
* Intro menus return you to the initial screen after death
* World selection button text is now centered better

# v0.3

## More critical bugs and new monster models

* Gave savefile streaming its own dedicated network channel
* Added new monster models/animations to the game
* Removed placeholder LOS lines for cubes
* Removed debug text above monsters

# v0.2

## Critical bugs

* Added game log notification when the game autosaves your character
* Game now autosaves on death
* Clicking on minimap elements should no longer block ability usage
* Dragging to equip bracelets in slots where one is already equipped should now work properly
* Fixed issue where monsters would stop spawning after going up and down stairs some number of times
* Fixed issue where blank or useless utility abilities would spawn
* Fixed issue where game log would get too long and glitch out
* Fixed issues with Ochre Jellies - they will no longer split on dot or damage zone tick, they have a cooldown of 0.5s before they can split after spawning, and generated Ochre Jellies can no longer drop items or abilities.
* Added stat box text to equipped items so they can be read more easily.
* Teleport traps should no longer teleport you off the map
* Pit traps should no longer drop you multiple floors, or to impossible floors
