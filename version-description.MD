# v0.17.1 - Bugfixes

* Added a number in skill tree window showing how many skill points a gem has available to spend
* Negative ability attributes should no longer appear as latent
* Reduced brightness of physical ranged attacks by 50% to reduce the distraction they cause
* Abilities can no longer be generated with two copies of the knockback attribute
* There is now a back button on the character skill selection screen. Note that the skills will not change unless you cancel character creation entirely.
* Monsters should no longer be visible on the minimap
* Abilities with the damage shield attribute will now tell you how much damage it prevents
* During character creation, abilities that cost more MP than the character will have to begin will no longer be generated
* Boost stat, reduce elemental damage, and increase elemental damage skill tree nodes will now tell you what element they will affect
* Fixed a bug where the player couldn't drag abilities to hotbar slots that hadn't been filled before
* Fixed a bug where inactive passive abilities didn't level up
* Charge attribute was not displaying its description correctly in skill trees
* Grappling Hook attribute was not displaying its description correctly in skill trees
* Burning Aura passive is now working as intended
* Tooltips should now correctly update upon activating a skill tree node
* Boss will now correctly reset back to the boss room if you kite them too far away
* Reduce Mana Cost skill tree node should no longer cause MP usage to go up for certain edge cases
* When abilities are leveled, damage should no longer go down for certain edge cases
* The tooltip for the skill tree ability drop slot should now display correctly
* Disabled and invisible UI elements should no longer block mouse clicks when clicking to use abilities
* Fixed an issue where some hotbar buttons (2/5/-) could not be assigned to a mouse button
* Attributes should no longer disappear upon leveling a skill
* Tooltips for healing effects should now be accurate
* Scaling for passive abilities upon leveling should be more reasonable now
* Fixed an issue where the monster fade-out effect on death was not working properly in builds

## Known Issues

* You cannot fuse passive abilities yet
* Fusing attack and utility abilities together may give you strange results
