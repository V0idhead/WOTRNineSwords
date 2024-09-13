# Nine Swords
A mod for Pathfinder: Wrath of the Righteous implementing the classes found in the Tome of Battle: Nine Swords supplement (https://dndtools.net/rulebooks/supplementals-35--5/tome-of-battle-the-book-of-nine-swords--88/)
There were some changes necessary both to adapt to the Pathfinder ruleset as well as accommodate Wrath of the Righteous

## Contents
 - Warblade class
 - 50 Maneuvers (including a tiny number of Boosts) and 10 Stances from the Diamond Mind, Iron Heart, Stone Dragon, Tiger Claw and White Raven disciplines

## Deviations from Rules as Written
### Spontaneous preparation
Instead of having to prepare maneuvers ahead of time as a wizard would, all classes will have all known maneuvers available at all times as a sorcerer does, the number of known and available maneuvers was not changed. This is way less work to implement and makes the classes more flexible.

### Recovering Maneuvers
Warblades recover maneuvers with a move action ability that HAS to hit an enemy and recovers two uses of maneuvers.
Swordsages recover all their maneuvers by spending a single full round actions.
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
 - All charge related to charge do not directly cause you to charge a target, instead they give you a buff that modifies your charge. This is rather annoying but I didn't find a way to make it work properly otherwise
 - Lions Roar triggers directly when used, but requires a move action

## Roadmap
 - Swordsage class
 - Disciplines available to Swordsage
 - Prestige Classes from the Tome of Battle (probably as Archetypes for the two classes in order to Pathfinderfy them)
 - Classes and Maneuvers from the Path of War (https://www.d20pfsrd.com/alternative-rule-systems/path-of-war/), probably I'll fold the Maneuvers into the disciples from Path of War
 - Homebrew Maneuvers to fill out any remaining gaps in the Maneuver-List because some Maneuvers in the books cannot be implemented or would not be useful in WOTR

# Contributing
This is my first mod and I learned a lot while working on it, but I'm sure there is a lot that could be improved.
If you have any suggestions for improvements please feel free to create a pull request or an issue on github.

# Special Thanks
to WittleWolfie for the excellent https://github.com/WittleWolfie/WW-Blueprint-Core