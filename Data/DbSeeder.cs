using dnd_srd.Models;

namespace dnd_srd.Data;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Editions.Any()) return;

        // =====================
        // EDITIONS
        // =====================
        var edition5e = new Edition
        {
            Name = "D&D 5th Edition",
            ReleaseYear = 2014,
            Publisher = "Wizards of the Coast",
            LicenseType = "CC BY 4.0"
        };
        var edition35e = new Edition
        {
            Name = "D&D 3.5 Edition",
            ReleaseYear = 2003,
            Publisher = "Wizards of the Coast",
            LicenseType = "OGL 1.0a"
        };
        var editionPf1 = new Edition
        {
            Name = "Pathfinder 1e",
            ReleaseYear = 2009,
            Publisher = "Paizo",
            LicenseType = "OGL 1.0a"
        };
        var editionPf2 = new Edition
        {
            Name = "Pathfinder 2e",
            ReleaseYear = 2019,
            Publisher = "Paizo",
            LicenseType = "ORC License"
        };

        context.Editions.AddRange(edition5e, edition35e, editionPf1, editionPf2);

        // =====================
        // MONSTER TYPES
        // =====================
        var dragon = new MonsterType { Name = "Dragon", Description = "Large reptilian creatures of ancient power" };
        var undead = new MonsterType { Name = "Undead", Description = "Creatures animated by dark magic" };
        var beast = new MonsterType { Name = "Beast", Description = "Natural animals and creatures" };
        var humanoid = new MonsterType { Name = "Humanoid", Description = "Human-like creatures" };
        var fiend = new MonsterType { Name = "Fiend", Description = "Evil creatures from lower planes" };
        var celestial = new MonsterType { Name = "Celestial", Description = "Creatures from the upper planes" };
        var aberration = new MonsterType { Name = "Aberration", Description = "Alien creatures of strange origin" };
        var elemental = new MonsterType { Name = "Elemental", Description = "Creatures composed of elemental forces" };

        context.MonsterTypes.AddRange(dragon, undead, beast, humanoid, fiend, celestial, aberration, elemental);

        // =====================
        // MONSTERS
        // =====================
        var goblin = new Monster
        {
            Name = "Goblin",
            Description = "Small, black-hearted humanoids that lair in caves and forests.",
            HitPoints = 7,
            ArmorClass = 15,
            WalkSpeed = 30,
            ChallengeRating = 0.25,
            Size = "Small",
            Alignment = "Neutral Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = humanoid
        };
        var skeleton = new Monster
        {
            Name = "Skeleton",
            Description = "Animated bones held together by dark magic.",
            HitPoints = 13,
            ArmorClass = 13,
            WalkSpeed = 30,
            ChallengeRating = 0.25,
            Size = "Medium",
            Alignment = "Lawful Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = undead
        };
        var zombie = new Monster
        {
            Name = "Zombie",
            Description = "A reanimated corpse driven by hunger.",
            HitPoints = 22,
            ArmorClass = 8,
            WalkSpeed = 20,
            ChallengeRating = 0.25,
            Size = "Medium",
            Alignment = "Neutral Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = undead
        };
        var youngDragon = new Monster
        {
            Name = "Young Red Dragon",
            Description = "A fearsome dragon wreathed in fire.",
            HitPoints = 178,
            ArmorClass = 18,
            WalkSpeed = 40,
            FlySpeed = 80,
            ChallengeRating = 10,
            Size = "Large",
            Alignment = "Chaotic Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = dragon
        };
        var troll = new Monster
        {
            Name = "Troll",
            Description = "A large, regenerating humanoid monster.",
            HitPoints = 84,
            ArmorClass = 15,
            WalkSpeed = 30,
            ChallengeRating = 5,
            Size = "Large",
            Alignment = "Chaotic Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = humanoid
        };
        var imp = new Monster
        {
            Name = "Imp",
            Description = "A small devil that serves as a familiar.",
            HitPoints = 10,
            ArmorClass = 13,
            WalkSpeed = 20,
            FlySpeed = 40,
            ChallengeRating = 1,
            Size = "Tiny",
            Alignment = "Lawful Evil",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = fiend
        };
        var giantSpider = new Monster
        {
            Name = "Giant Spider",
            Description = "A large spider that lurks in dark places.",
            HitPoints = 26,
            ArmorClass = 14,
            WalkSpeed = 30,
            ChallengeRating = 1,
            Size = "Large",
            Alignment = "Unaligned",
            IsSRD = true,
            Edition = edition5e,
            MonsterType = beast
        };
        var orcPf1 = new Monster
        {
            Name = "Orc",
            Description = "A savage humanoid driven by bloodlust.",
            HitPoints = 15,
            ArmorClass = 13,
            WalkSpeed = 30,
            ChallengeRating = 0.5,
            Size = "Medium",
            Alignment = "Chaotic Evil",
            IsSRD = true,
            Edition = editionPf1,
            MonsterType = humanoid
        };

        context.Monsters.AddRange(goblin, skeleton, zombie, youngDragon, troll, imp, giantSpider, orcPf1);

        // =====================
        // ABILITY SCORES
        // =====================
        context.AbilityScores.AddRange(
            new AbilityScores { Monster = goblin, Strength = 8, Dexterity = 14, Constitution = 10, Intelligence = 10, Wisdom = 8, Charisma = 8 },
            new AbilityScores { Monster = skeleton, Strength = 10, Dexterity = 14, Constitution = 15, Intelligence = 6, Wisdom = 8, Charisma = 5 },
            new AbilityScores { Monster = zombie, Strength = 13, Dexterity = 6, Constitution = 16, Intelligence = 3, Wisdom = 6, Charisma = 5 },
            new AbilityScores { Monster = youngDragon, Strength = 23, Dexterity = 10, Constitution = 21, Intelligence = 14, Wisdom = 11, Charisma = 19 },
            new AbilityScores { Monster = troll, Strength = 18, Dexterity = 13, Constitution = 20, Intelligence = 7, Wisdom = 9, Charisma = 7 },
            new AbilityScores { Monster = imp, Strength = 6, Dexterity = 17, Constitution = 13, Intelligence = 11, Wisdom = 12, Charisma = 14 },
            new AbilityScores { Monster = giantSpider, Strength = 14, Dexterity = 16, Constitution = 12, Intelligence = 2, Wisdom = 11, Charisma = 4 },
            new AbilityScores { Monster = orcPf1, Strength = 17, Dexterity = 11, Constitution = 16, Intelligence = 7, Wisdom = 11, Charisma = 10 }
        );

        // =====================
        // ACTIONS
        // =====================
        context.Actions.AddRange(
            new Models.Action { Monster = goblin, Name = "Scimitar", Description = "Melee Weapon Attack", AttackBonus = 4, DamageDice = "1d6+2", DamageType = "Slashing", ActionType = "Action" },
            new Models.Action { Monster = goblin, Name = "Shortbow", Description = "Ranged Weapon Attack", AttackBonus = 4, DamageDice = "1d6+2", DamageType = "Piercing", ActionType = "Action" },
            new Models.Action { Monster = skeleton, Name = "Shortsword", Description = "Melee Weapon Attack", AttackBonus = 4, DamageDice = "1d6+2", DamageType = "Piercing", ActionType = "Action" },
            new Models.Action { Monster = zombie, Name = "Slam", Description = "Melee Weapon Attack", AttackBonus = 3, DamageDice = "1d6+1", DamageType = "Bludgeoning", ActionType = "Action" },
            new Models.Action { Monster = youngDragon, Name = "Bite", Description = "Melee Weapon Attack", AttackBonus = 10, DamageDice = "2d10+6", DamageType = "Piercing", ActionType = "Action" },
            new Models.Action { Monster = youngDragon, Name = "Fire Breath", Description = "The dragon exhales fire in a 30-foot cone.", AttackBonus = null, DamageDice = "16d6", DamageType = "Fire", ActionType = "Action" },
            new Models.Action { Monster = troll, Name = "Claw", Description = "Melee Weapon Attack", AttackBonus = 7, DamageDice = "2d6+4", DamageType = "Slashing", ActionType = "Action" },
            new Models.Action { Monster = imp, Name = "Sting", Description = "Melee Weapon Attack", AttackBonus = 5, DamageDice = "1d4+3", DamageType = "Piercing", ActionType = "Action" },
            new Models.Action { Monster = giantSpider, Name = "Bite", Description = "Melee Weapon Attack", AttackBonus = 5, DamageDice = "1d8+3", DamageType = "Piercing", ActionType = "Action" },
            new Models.Action { Monster = orcPf1, Name = "Greataxe", Description = "Melee Weapon Attack", AttackBonus = 5, DamageDice = "1d12+3", DamageType = "Slashing", ActionType = "Action" }
        );

        // =====================
        // TRAITS
        // =====================
        context.Traits.AddRange(
            new Trait { Monster = goblin, Name = "Nimble Escape", Description = "The goblin can take the Disengage or Hide action as a bonus action on each of its turns." },
            new Trait { Monster = skeleton, Name = "Damage Vulnerabilities", Description = "Bludgeoning damage." },
            new Trait { Monster = zombie, Name = "Undead Fortitude", Description = "If damage reduces the zombie to 0 hit points, it must make a Constitution saving throw." },
            new Trait { Monster = youngDragon, Name = "Legendary Resistance", Description = "If the dragon fails a saving throw, it can choose to succeed instead." },
            new Trait { Monster = troll, Name = "Regeneration", Description = "The troll regains 10 hit points at the start of its turn." },
            new Trait { Monster = imp, Name = "Devil's Sight", Description = "Magical darkness doesn't impede the imp's darkvision." },
            new Trait { Monster = giantSpider, Name = "Spider Climb", Description = "The spider can climb difficult surfaces, including upside down on ceilings." },
            new Trait { Monster = orcPf1, Name = "Ferocity", Description = "Once per day, the orc can remain conscious below 0 hit points for one round." }
        );

        // =====================
        // ENVIRONMENTS
        // =====================
        context.Environments.AddRange(
            new Models.Environment { Monster = goblin, Name = "Forest" },
            new Models.Environment { Monster = goblin, Name = "Cave" },
            new Models.Environment { Monster = skeleton, Name = "Dungeon" },
            new Models.Environment { Monster = zombie, Name = "Dungeon" },
            new Models.Environment { Monster = youngDragon, Name = "Mountain" },
            new Models.Environment { Monster = youngDragon, Name = "Volcano" },
            new Models.Environment { Monster = troll, Name = "Swamp" },
            new Models.Environment { Monster = imp, Name = "Nine Hells" },
            new Models.Environment { Monster = giantSpider, Name = "Forest" },
            new Models.Environment { Monster = giantSpider, Name = "Cave" },
            new Models.Environment { Monster = orcPf1, Name = "Plains" }
        );

        // =====================
        // CLASSES
        // =====================
        var wizard5e = new Class
        {
            Name = "Wizard",
            Description = "A scholarly magic-user capable of manipulating the structures of reality.",
            HitDie = "d6",
            PrimaryAbility = "Intelligence",
            SavingThrows = "Intelligence, Wisdom",
            ArmorProficiencies = "None",
            WeaponProficiencies = "Daggers, darts, slings, quarterstaffs, light crossbows",
            IsSRD = true,
            Edition = edition5e
        };
        var fighter5e = new Class
        {
            Name = "Fighter",
            Description = "A master of martial combat, skilled with a variety of weapons and armor.",
            HitDie = "d10",
            PrimaryAbility = "Strength or Dexterity",
            SavingThrows = "Strength, Constitution",
            ArmorProficiencies = "All armor, shields",
            WeaponProficiencies = "Simple weapons, martial weapons",
            IsSRD = true,
            Edition = edition5e
        };
        var cleric5e = new Class
        {
            Name = "Cleric",
            Description = "A priestly champion who wields divine magic in service of a higher power.",
            HitDie = "d8",
            PrimaryAbility = "Wisdom",
            SavingThrows = "Wisdom, Charisma",
            ArmorProficiencies = "Light armor, medium armor, shields",
            WeaponProficiencies = "Simple weapons",
            IsSRD = true,
            Edition = edition5e
        };
        var rogue5e = new Class
        {
            Name = "Rogue",
            Description = "A scoundrel who uses stealth and trickery to overcome obstacles.",
            HitDie = "d8",
            PrimaryAbility = "Dexterity",
            SavingThrows = "Dexterity, Intelligence",
            ArmorProficiencies = "Light armor",
            WeaponProficiencies = "Simple weapons, hand crossbows, longswords, rapiers, shortswords",
            IsSRD = true,
            Edition = edition5e
        };

        // Subclasses
        var evocationWizard = new Class
        {
            Name = "School of Evocation",
            Description = "Wizards of the School of Evocation channel magical energy to create devastating effects.",
            HitDie = "d6",
            PrimaryAbility = "Intelligence",
            SavingThrows = "Intelligence, Wisdom",
            ArmorProficiencies = "None",
            WeaponProficiencies = "Daggers, darts, slings, quarterstaffs, light crossbows",
            IsSRD = true,
            Edition = edition5e,
            ParentClass = wizard5e
        };
        var championFighter = new Class
        {
            Name = "Champion",
            Description = "The Champion focuses on the development of raw physical power.",
            HitDie = "d10",
            PrimaryAbility = "Strength or Dexterity",
            SavingThrows = "Strength, Constitution",
            ArmorProficiencies = "All armor, shields",
            WeaponProficiencies = "Simple weapons, martial weapons",
            IsSRD = true,
            Edition = edition5e,
            ParentClass = fighter5e
        };

        context.Classes.AddRange(wizard5e, fighter5e, cleric5e, rogue5e, evocationWizard, championFighter);

        // =====================
        // SPELLS
        // =====================
        context.Spells.AddRange(
            new Spell
            {
                Name = "Fireball",
                Description = "A bright streak flashes from your pointing finger to a point you choose within range, then blossoms with a low roar into an explosion of flame.",
                Level = 3,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "150 feet",
                Duration = "Instantaneous",
                Concentration = false,
                Ritual = false,
                Components = "V, S, M",
                IsSRD = true,
                Edition = edition5e,
                Class = wizard5e
            },
            new Spell
            {
                Name = "Magic Missile",
                Description = "You create three glowing darts of magical force.",
                Level = 1,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "120 feet",
                Duration = "Instantaneous",
                Concentration = false,
                Ritual = false,
                Components = "V, S",
                IsSRD = true,
                Edition = edition5e,
                Class = wizard5e
            },
            new Spell
            {
                Name = "Invisibility",
                Description = "A creature you touch becomes invisible until the spell ends.",
                Level = 2,
                School = "Illusion",
                CastingTime = "1 action",
                Range = "Touch",
                Duration = "1 hour",
                Concentration = true,
                Ritual = false,
                Components = "V, S, M",
                IsSRD = true,
                Edition = edition5e,
                Class = wizard5e
            },
            new Spell
            {
                Name = "Cure Wounds",
                Description = "A creature you touch regains a number of hit points equal to 1d8 + your spellcasting ability modifier.",
                Level = 1,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "Touch",
                Duration = "Instantaneous",
                Concentration = false,
                Ritual = false,
                Components = "V, S",
                IsSRD = true,
                Edition = edition5e,
                Class = cleric5e
            },
            new Spell
            {
                Name = "Sacred Flame",
                Description = "Flame-like radiance descends on a creature that you can see within range.",
                Level = 0,
                School = "Evocation",
                CastingTime = "1 action",
                Range = "60 feet",
                Duration = "Instantaneous",
                Concentration = false,
                Ritual = false,
                Components = "V, S",
                IsSRD = true,
                Edition = edition5e,
                Class = cleric5e
            },
            new Spell
            {
                Name = "Shield",
                Description = "An invisible barrier of magical force appears and protects you.",
                Level = 1,
                School = "Abjuration",
                CastingTime = "1 reaction",
                Range = "Self",
                Duration = "1 round",
                Concentration = false,
                Ritual = false,
                Components = "V, S",
                IsSRD = true,
                Edition = edition5e,
                Class = wizard5e
            },
            new Spell
            {
                Name = "Detect Magic",
                Description = "For the duration, you sense the presence of magic within 30 feet of you.",
                Level = 1,
                School = "Divination",
                CastingTime = "1 action",
                Range = "Self",
                Duration = "10 minutes",
                Concentration = true,
                Ritual = true,
                Components = "V, S",
                IsSRD = true,
                Edition = edition5e,
                Class = wizard5e
            },
            new Spell
            {
                Name = "Healing Word",
                Description = "A creature of your choice that you can see within range regains hit points equal to 1d4 + your spellcasting ability modifier.",
                Level = 1,
                School = "Evocation",
                CastingTime = "1 bonus action",
                Range = "60 feet",
                Duration = "Instantaneous",
                Concentration = false,
                Ritual = false,
                Components = "V",
                IsSRD = true,
                Edition = edition5e,
                Class = cleric5e
            }
        );

        // =====================
        // RACES
        // =====================
        context.Races.AddRange(
            new Race
            {
                Name = "Human",
                Description = "Humans are the most adaptable and ambitious people among the common races.",
                Speed = 30,
                Size = "Medium",
                AbilityScoreBonus = "+1 to all ability scores",
                Traits = "Extra Language, Extra Skill",
                Languages = "Common, one extra language",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Elf",
                Description = "Elves are a magical people of otherworldly grace.",
                Speed = 30,
                Size = "Medium",
                AbilityScoreBonus = "+2 Dexterity",
                Traits = "Darkvision, Keen Senses, Fey Ancestry, Trance",
                Languages = "Common, Elvish",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Dwarf",
                Description = "Bold and hardy, dwarves are known as skilled warriors and craftspeople.",
                Speed = 25,
                Size = "Medium",
                AbilityScoreBonus = "+2 Constitution",
                Traits = "Darkvision, Dwarven Resilience, Stonecunning",
                Languages = "Common, Dwarvish",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Halfling",
                Description = "The comforts of home are the goals of most halflings lives.",
                Speed = 25,
                Size = "Small",
                AbilityScoreBonus = "+2 Dexterity",
                Traits = "Lucky, Brave, Halfling Nimbleness",
                Languages = "Common, Halfling",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Gnome",
                Description = "A gnome's energy and enthusiasm for living shines through every inch of their tiny body.",
                Speed = 25,
                Size = "Small",
                AbilityScoreBonus = "+2 Intelligence",
                Traits = "Darkvision, Gnome Cunning",
                Languages = "Common, Gnomish",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Half-Elf",
                Description = "Half-elves combine what some say are the best qualities of their elf and human parents.",
                Speed = 30,
                Size = "Medium",
                AbilityScoreBonus = "+2 Charisma, +1 to two other ability scores",
                Traits = "Darkvision, Fey Ancestry, Skill Versatility",
                Languages = "Common, Elvish, one extra language",
                IsSRD = true,
                Edition = edition5e
            },
            new Race
            {
                Name = "Dragonborn",
                Description = "Dragonborn look very much like dragons standing erect in humanoid form.",
                Speed = 30,
                Size = "Medium",
                AbilityScoreBonus = "+2 Strength, +1 Charisma",
                Traits = "Draconic Ancestry, Breath Weapon, Damage Resistance",
                Languages = "Common, Draconic",
                IsSRD = true,
                Edition = edition5e
            }
        );

        // =====================
        // RULE ENTRIES
        // =====================
        context.RuleEntries.AddRange(
            new RuleEntry
            {
                Name = "Longsword",
                Category = "Weapon",
                Description = "A versatile martial melee weapon.",
                Cost = "15 gp",
                Weight = 3,
                DamageDice = "1d8",
                DamageType = "Slashing",
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Shortbow",
                Category = "Weapon",
                Description = "A simple ranged weapon.",
                Cost = "25 gp",
                Weight = 2,
                DamageDice = "1d6",
                DamageType = "Piercing",
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Chain Mail",
                Category = "Equipment",
                Description = "Made of interlocking metal rings, chain mail includes a layer of quilted fabric worn underneath.",
                Cost = "75 gp",
                Weight = 55,
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Leather Armor",
                Category = "Equipment",
                Description = "The breastplate and shoulder protectors of this armor are made of leather.",
                Cost = "10 gp",
                Weight = 10,
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Alert",
                Category = "Feat",
                Description = "Always on the lookout for danger. You gain +5 to initiative, cannot be surprised, and hidden creatures gain no advantage against you.",
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Great Weapon Master",
                Category = "Feat",
                Description = "You have learned to put the weight of a weapon to your advantage.",
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Dagger",
                Category = "Weapon",
                Description = "A simple light melee weapon that can also be thrown.",
                Cost = "2 gp",
                Weight = 1,
                DamageDice = "1d4",
                DamageType = "Piercing",
                IsSRD = true,
                Edition = edition5e
            },
            new RuleEntry
            {
                Name = "Backpack",
                Category = "Equipment",
                Description = "A backpack can hold one cubic foot or 30 pounds of gear.",
                Cost = "2 gp",
                Weight = 5,
                IsSRD = true,
                Edition = edition5e
            }
        );

        context.SaveChanges();
    }
}