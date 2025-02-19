# Nine Swords
A mod for Pathfinder: Wrath of the Righteous implementing the classes found in the Tome of Battle: Nine Swords supplement (https://dndtools.net/rulebooks/supplementals-35--5/tome-of-battle-the-book-of-nine-swords--88/)
There were some changes necessary both to adapt to the Pathfinder ruleset as well as accommodate Wrath of the Righteous

## Contents
 - Warblade class
 - 60+ Maneuvers (including a tiny number of Boosts and 6 Counters) and 10 Stances from the Diamond Mind, Iron Heart, Stone Dragon, Tiger Claw and White Raven disciplines

## Deviations from Rules as Written
### Spontaneous preparation
Instead of having to prepare maneuvers ahead of time as a wizard would, all classes will have all known maneuvers available at all times as a sorcerer does, the number of known and available maneuvers was not changed. This is way less work to implement and makes the classes more flexible.

### Recovering Maneuvers
Warblades recover maneuvers with a move action ability that HAS to hit an enemy and recovers two uses of maneuvers.
Swordsages recover all their maneuvers by spending a single full round action.
Both changes where made to balance maneuver recovery, as written Swordsage recovery was comically bad

### Individual Maneuvers
 - Maneuvers that should give your allies an attack bonus against an individual enemy, instead lowers the targets natural armor
 - Stances that say they end when moving more than 5 feet do not have this limitation
 - Hunter's Sense: gives blindsense analogous to other WOTR abilities that are described as "Scent"
 - Steel Wind: doesn't let you choose targets, the game will choose two targets and attack those
 - Flesh Ripper: penalties last for 1 minute instead of being dynamic
 - Bounding Assault: Teleports to target as a swift action
 - Overwhelming Mountain Strike and Irresistible Mountain Strike: staggers target instead of disabling move action
 - Hearing the Air: giving blindsight to distinguish it more from the lvl1 Hunter's Sense
 - Scything Blade: doesn't require having made a successful attack
 - All maneuvers related to charging do not directly cause you to charge a target, instead they give you a buff that modifies your charge. This is rather annoying but I didn't find a way to make it work properly otherwise
 - Lions Roar triggers directly when used, but requires a move action
 
### Counters
Counters had to be changed to a simple on/off toggle to compensate for the fact that I can't interrupt events to ask the player if a counter should be used. Additionally:

 - Wall of Blades and Manticore Parry triggers on being attacked, costs at most 2 maneuver points per round and tries to parry all attacks for 1 round, each successful parry adds a -1 penalty to subsequent parries in the same round. If maneuver points where spent on Wall of Blades Manticore Parry will be free and vice versa
 - Iron Heart Focus triggers on failing a save and costs at most 2 maneuver points per round and retries all failed saves for 1 round
 - Lightning Recovery triggers on failing an attack roll and retries them at a cost of 1 maneuver point per fail
 - Diamond Defense was changed to a simple toggle and gives a +9 bonus to all saves at a cost of 1 maneuver point per round

### Homebrew
 - Boomerang Throw and Vicious Throw as lower level options for the level 8 Lightning Throw
 - White Raven Call as a replacement for the level 9 White Raven maneuver that cannot be implemented into WOTR; White Raven Call gives massive bonuses to self and allies for 1 round
 - Several feats including Maneuver Focus, Maneuver Focus (Mythic) and a specific feat for each discipline with unique effects

## Roadmap
 - Swordsage class
 - Disciplines available to Swordsage
 - Prestige Classes from the Tome of Battle (probably as Archetypes for the two classes in order to Pathfinderfy them)
 - Classes and Maneuvers from the Path of War (https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/), probably I'll fold the Maneuvers into the disciples from Path of War
 - Homebrew Maneuvers to fill out any remaining gaps in the Maneuver-List because some Maneuvers in the books cannot be implemented or would not be useful in WOTR

# Contributing
This is my first mod and I learned a lot while working on it, but I'm sure there is a lot that could be improved.
If you have any suggestions for improvements please feel free to open a discussion thread or an issue on github.

# Special Thanks
to WittleWolfie for the excellent https://github.com/WittleWolfie/WW-Blueprint-Core