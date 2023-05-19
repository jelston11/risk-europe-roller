using System.Diagnostics;

namespace BattleRoller
{
    public class Roller
    {
        private int trace;

        private int attacking_siege;
        private int attacking_archers;
        private int attacking_cavalry;
        private int attacking_footmen;

        private int defending_siege;
        private int defending_archers;
        private int defending_cavalry;
        private int defending_footmen;
        private bool on_castle;
        private bool has_reroll;
        private bool attackers_set;
        private bool defenders_set;

        // random.Next(1, 7); to simulate dice roll
        private readonly Random random;

        public int Rank { get; private set; }
        public int Rolls { get; private set; }
        public int AttackingArmySize { get; private set; }
        public int AttackingOriginalArmySize { get; private set; }
        public int DefendingArmySize { get; private set; }
        public int DefendingOriginalArmySize { get; private set; }
        public int[] AttackerLosses { get; private set; }
        public int[] DefenderLosses { get; private set; }
        public char Victor { get; private set; }

        public Roller(int trace_level) 
        {
            attackers_set = false;
            defenders_set = false;

            // 0 is siege, 1 is archers, 2 is cavalry, 3 is footmen
            AttackerLosses = new int[4];
            DefenderLosses = new int[4];

            Victor = 'n';

            Rank = -1;

            random = new();

            trace = trace_level;
        }

        public void SetAttackingArmy(int siege, int archers, int cavalry, int footmen)
        {
            attacking_siege = siege;
            attacking_archers = archers;
            attacking_cavalry = cavalry;
            attacking_footmen = footmen;
            AttackingOriginalArmySize = siege + archers + cavalry + footmen;
            AttackingArmySize = AttackingOriginalArmySize;
            if (AttackingOriginalArmySize > 0)
            {
                attackers_set = true;
            }

            if (trace > 0)
            {
                Console.WriteLine($"\nATTACKERS\nSiege: {attacking_siege} | Archers: {attacking_archers}" +
                    $" | Cavalry: {attacking_cavalry} | Footmen: {attacking_footmen}\n");
            }
        }

        public void SetDefendingArmy(int siege, int archers, int cavalry, int footmen, bool castle)
        {
            defending_siege = siege;
            defending_archers = archers;
            defending_cavalry = cavalry;
            defending_footmen = footmen;
            DefendingOriginalArmySize = siege + archers + cavalry + footmen;
            DefendingArmySize = DefendingOriginalArmySize;
            on_castle = castle;
            has_reroll = castle;
            if (DefendingOriginalArmySize > 0)
            {
                defenders_set = true;
            }

            if (trace > 0)
            {
                Console.WriteLine($"\nDEFENDERS\nSiege: {defending_siege} | Archers: {defending_archers}" +
                    $" | Cavalry: {defending_cavalry} | Footmen: {defending_footmen}\n");
            }
        }

        public bool Roll(bool reroll)
        {
            if (!defenders_set || !attackers_set) 
            {
                if (trace > 0)
                {
                    Console.WriteLine("No roll. Armies not set.");
                }
                return false;
            }

            // Increment rank and get rank to use for this roll
            if (on_castle && reroll)
            {
                if (!has_reroll)
                {
                    if (trace > 0)
                    {
                        Console.WriteLine("No reroll available.");
                    }
                    return false;
                }
                has_reroll = false;
                if (trace > 0)
                {
                    Console.WriteLine("Reroll.");
                }
            }
            else
            {
                Rank++;
                if (Rank == 4)
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("Reset rank.");
                    }
                    Rank = 0;
                    if (on_castle)
                    {
                        if (trace > 1)
                        {
                            Console.WriteLine("Reset reroll availability.");
                        }
                        has_reroll = true;
                    }
                }
                if (Rank == 0 && attacking_siege == 0 && defending_siege == 0)
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("No siege weapons. Skip rank 0.");
                    }
                    Rank = 1;
                }
                if (Rank == 1 && attacking_archers == 0 && defending_archers == 0)
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("No archers. Skip rank 1.");
                    }
                    Rank = 2;
                }
                if (Rank == 2 && attacking_cavalry == 0 && defending_cavalry == 0)
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("No cavalry. Skip rank 2.");
                    }
                    Rank = 3;
                }
                if (Rank == 3 && AttackingArmySize == 0 && DefendingArmySize == 0)
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("No footmen. No battle.");
                    }
                    return false;
                }
            }

            int attack_hits = 0;
            int defense_hits = 0;

            if (Rank == 0)
            {
                if (trace > 0)
                {
                    Console.WriteLine("\nSIEGE ATTACK\n");
                }
                // Siege Attack (hit on 3+)
                if (!reroll)
                {
                    if (trace > 0)
                    {
                        Console.WriteLine($"{attacking_siege} attacking siege weapons.");
                    }
                    for (int i = 0; i < attacking_siege; i++)
                    {
                        // 2 rolls for each siege weapon
                        if (random.Next(1, 7) >= 3)
                        {
                            if (trace > 0)
                            {
                                Console.WriteLine($"Attacking siege {i + 1} hits.");
                            }
                            attack_hits++;
                        }
                        if (random.Next(1, 7) >= 3)
                        {
                            if (trace > 0)
                            {
                                Console.WriteLine($"Attacking siege {i + 1} hits.");
                            }
                            attack_hits++;
                        }
                    }
                }

                if (trace > 0)
                {
                    Console.WriteLine($"{defending_siege} defending siege weapons.");
                }
                for (int i = 0; i < defending_siege; i++)
                {
                    // 2 rolls for each siege weapon
                    if (random.Next(1, 7) >= 3)
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Defending siege {i + 1} hits.");
                        }
                        defense_hits++;
                    }
                    if (random.Next(1, 7) >= 3)
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Defending siege {i + 1} hits.");
                        }
                        defense_hits++;
                    }
                }
            }
            else if (Rank == 1)
            {
                if (trace > 0)
                {
                    Console.WriteLine("\nARCHER VOLLEY\n");
                }
                // Archer Volley (hit on 5+)
                if (!reroll)
                {
                    if (trace > 0)
                    {
                        Console.WriteLine($"{attacking_archers} attacking archers.");
                    }
                    for (int i = 0; i < attacking_archers; i++)
                    {
                        if (random.Next(1, 7) >= 5)
                        {
                            if (trace > 0)
                            {
                                Console.WriteLine($"Attacking archer {i + 1} hits.");
                            }
                            attack_hits++;
                        }
                    }
                }

                if (trace > 0)
                {
                    Console.WriteLine($"{defending_archers} defending archers.");
                }
                for (int i = 0; i < defending_archers; i++)
                {
                    if (random.Next(1, 7) >= 5)
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Defending archer {i + 1} hits.");
                        }
                        defense_hits++;
                    }
                }
            } 
            else if (Rank == 2)
            {
                if (trace > 0)
                {
                    Console.WriteLine("\nCAVALRY CHARGE\n");
                }
                // Cavalry Charge (hit on 3+)
                if (!reroll)
                {
                    if (trace > 0)
                    {
                        Console.WriteLine($"{attacking_cavalry} attacking cavalry.");
                    }
                    for (int i = 0; i < attacking_cavalry; i++)
                    {
                        if (random.Next(1, 7) >= 3)
                        {
                            if (trace > 0)
                            {
                                Console.WriteLine($"Attacking cavalry {i + 1} hits.");
                            }
                            attack_hits++;
                        }
                    }
                }

                if (trace > 0)
                {
                    Console.WriteLine($"{defending_cavalry} defending cavalry.");
                }
                for (int i = 0; i < defending_cavalry; i++)
                {
                    if (random.Next(1, 7) >= 3)
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Defending cavalry {i + 1} hits.");
                        }
                        defense_hits++;
                    }
                }
            }
            else
            {
                if (trace > 0)
                {
                    Console.WriteLine("\nGENERAL ATTACK\n");
                }
                // Determine how many rolls attacker/defender gets
                int attack_roll_one = -1;
                int attack_roll_two = -1;
                int attack_roll_three = -1;
                int defense_roll_one = -1;
                int defense_roll_two = -1;

                if (AttackingArmySize > 2)
                {
                    attack_roll_three = random.Next(1, 7);
                }
                if (AttackingArmySize > 1)
                {
                    attack_roll_two = random.Next(1, 7);
                }
                if (AttackingArmySize > 0)
                {
                    attack_roll_one = random.Next(1, 7);
                }
                
                if (DefendingArmySize > 1)
                {
                    defense_roll_two = random.Next(1, 7);
                }
                if (DefendingArmySize > 0)
                {
                    defense_roll_one = random.Next(1, 7);
                }

                // Sort attack rolls in order
                // Three should hold the smallest, one the biggest
                int temp;
                if (attack_roll_three > attack_roll_two)
                {
                    // Swap
                    temp = attack_roll_three;
                    attack_roll_three = attack_roll_two;
                    attack_roll_two = temp;
                }
                if (attack_roll_two > attack_roll_one)
                {
                    // Swap
                    temp = attack_roll_one;
                    attack_roll_one = attack_roll_two;
                    attack_roll_two = temp;
                    if (attack_roll_three > attack_roll_two)
                    {
                        // Attack roll three not needed so don't put value into it here
                        //temp = attack_roll_two;
                        attack_roll_two = attack_roll_three;
                        //attack_roll_three = temp;
                    }
                }

                // Sort defense rolls
                if (defense_roll_two > defense_roll_one)
                {
                    // Swap
                    temp = defense_roll_one;
                    defense_roll_one = defense_roll_two;
                    defense_roll_two = temp;
                }

                // Compare rolls
                if (attack_roll_one > defense_roll_one)
                {
                    if (trace > 0)
                    {
                        Console.WriteLine($"Attacker hits with a {attack_roll_one} vs a {defense_roll_one}");
                    }
                    attack_hits++;
                } 
                else
                {
                    if (trace > 0)
                    {
                        Console.WriteLine($"Defender hits with a {defense_roll_one} vs a {attack_roll_one}");
                    }
                    defense_hits++;
                }

                if (attack_roll_two != -1 && defense_roll_two != -1)
                {
                    if (attack_roll_two > defense_roll_two)
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Attacker hits with a {attack_roll_two} vs a {defense_roll_two}");
                        }
                        attack_hits++;
                    }
                    else
                    {
                        if (trace > 0)
                        {
                            Console.WriteLine($"Defender hits with a {defense_roll_two} vs a {attack_roll_two}");
                        }
                        defense_hits++;
                    }
                }
                else
                {
                    if (trace > 1)
                    {
                        Console.WriteLine("Both players do not have 2 dice.");
                    }
                }
            }

            char battle_result = EliminateUnits(attack_hits, defense_hits);
            if (battle_result == 'c')
            {
                if (trace > 0)
                {
                    Console.WriteLine("\nBattle Continues. Roll again.\n");
                    Console.WriteLine($"\nDEFENDERS\nSiege: {defending_siege} | Archers: {defending_archers}" +
                    $" | Cavalry: {defending_cavalry} | Footmen: {defending_footmen}\n");
                    Console.WriteLine($"\nATTACKERS\nSiege: {attacking_siege} | Archers: {attacking_archers}" +
                    $" | Cavalry: {attacking_cavalry} | Footmen: {attacking_footmen}\n");
                }

                return false;
            }
            else
            {
                Victor = battle_result;
                if (trace > 0)
                {
                    Console.WriteLine($"\nBattle ends. Result: {Victor}.\n");
                }
                return true;
            }
        }

        public char Resolve()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            attackers_set = true;
            defenders_set = false;
        }

        private char EliminateUnits(int attacker_hits, int defender_hits)
        {
            // Check if battle is over
            if (defender_hits >= AttackingArmySize && attacker_hits >= DefendingArmySize)
            {
                // i for inconclusive (draw)
                if (trace > 0)
                {
                    Console.WriteLine("\nBattle is over. Result inconclusive.\n");
                }
                return 'i';
            }
            else if (defender_hits >= AttackingArmySize)
            {
                // d for defender win
                if (trace > 0)
                {
                    Console.WriteLine("\nBattle is over. Defender wins.\n");
                }
                return 'd';
            }
            else if (attacker_hits >= DefendingArmySize)
            {
                // a for attacker win
                if (trace > 0)
                {
                    Console.WriteLine("\nBattle is over. Attacker wins.\n");
                }
                return 'a';
            }

            // Determine new army sizes after hits
            DefendingArmySize -= attacker_hits;
            if (trace > 0)
            {
                Console.WriteLine($"Defenders lose {attacker_hits} units.");
            }
            AttackingArmySize -= defender_hits;
            if (trace > 0)
            {
                Console.WriteLine($"Attackers lose {defender_hits} units.");
            }

            // Eliminating attackers
            if (attacking_footmen < defender_hits)
            {
                AttackerLosses[3] += attacking_footmen;
                if (trace > 1)
                {
                    Console.WriteLine($"Attackers lose {attacking_footmen} footmen.");
                }
                defender_hits -= attacking_footmen;
                attacking_footmen = 0;
                if (attacking_archers < defender_hits)
                {
                    AttackerLosses[1] += attacking_archers;
                    if (trace > 1)
                    {
                        Console.WriteLine($"Attackers lose {attacking_archers} archers.");
                    }
                    defender_hits -= attacking_archers;
                    attacking_archers = 0;
                    if (attacking_cavalry < defender_hits)
                    {
                        AttackerLosses[2] += attacking_cavalry;
                        if (trace > 1)
                        {
                            Console.WriteLine($"Attackers lose {attacking_cavalry} cavalry.");
                        }
                        defender_hits -= attacking_cavalry;
                        attacking_cavalry = 0;
                        if (attacking_siege <= defender_hits)
                        {
                            AttackerLosses[0] += attacking_siege;
                            if (trace > 1)
                            {
                                Console.WriteLine($"Attackers lose {attacking_siege} siege weapons.");
                            }
                            attacking_siege = 0;
                        }
                        else
                        {
                            AttackerLosses[0] += defender_hits;
                            if (trace > 1)
                            {
                                Console.WriteLine($"Attackers lose {defender_hits} siege weapons.");
                            }
                            attacking_siege -= defender_hits;
                        }
                    }
                    else
                    {
                        AttackerLosses[2] += defender_hits;
                        if (trace > 1)
                        {
                            Console.WriteLine($"Attackers lose {defender_hits} cavalry.");
                        }
                        attacking_cavalry -= defender_hits;
                    }
                } 
                else
                {
                    AttackerLosses[1] += defender_hits;
                    if (trace > 1)
                    {
                        Console.WriteLine($"Attackers lose {defender_hits} archers.");
                    }
                    attacking_archers -= defender_hits;
                }
            }
            else
            {
                AttackerLosses[3] += defender_hits;
                if (trace > 1)
                {
                    Console.WriteLine($"Attackers lose {defender_hits} footmen.");
                }
                attacking_footmen -= defender_hits;
            }

            // Eliminating defenders
            if (defending_footmen < attacker_hits)
            {
                DefenderLosses[3] += defending_footmen;
                if (trace > 1)
                {
                    Console.WriteLine($"Defenders lose {defending_footmen} footmen.");
                }
                attacker_hits -= defending_footmen;
                defending_footmen = 0;
                if (defending_archers < attacker_hits)
                {
                    DefenderLosses[1] += defending_archers;
                    if (trace > 1)
                    {
                        Console.WriteLine($"Defenders lose {defending_archers} archers.");
                    }
                    attacker_hits -= defending_archers;
                    defending_archers = 0;
                    if (defending_cavalry < attacker_hits)
                    {
                        DefenderLosses[2] += defending_cavalry;
                        if (trace > 1)
                        {
                            Console.WriteLine($"Defenders lose {defending_cavalry} cavalry.");
                        }
                        attacker_hits -= defending_cavalry;
                        defending_cavalry = 0;
                        if (defending_siege <= attacker_hits)
                        {
                            DefenderLosses[0] += defending_siege;
                            if (trace > 1)
                            {
                                Console.WriteLine($"Defenders lose {defending_siege} siege weapons.");
                            }
                            defending_siege = 0;
                        }
                        else
                        {
                            DefenderLosses[0] += attacker_hits;
                            if (trace > 1)
                            {
                                Console.WriteLine($"Defenders lose {attacker_hits} siege weapons.");
                            }
                            defending_siege -= attacker_hits;
                        }
                    }
                    else
                    {
                        DefenderLosses[2] += attacker_hits;
                        if (trace > 1)
                        {
                            Console.WriteLine($"Defenders lose {attacker_hits} cavalry.");
                        }
                        defending_cavalry -= attacker_hits;
                    }
                }
                else
                {
                    DefenderLosses[1] += attacker_hits;
                    if (trace > 1)
                    {
                        Console.WriteLine($"Defenders lose {attacker_hits} archers.");
                    }
                    defending_archers -= attacker_hits;
                }
            }
            else
            {
                DefenderLosses[3] += attacker_hits;
                if (trace > 1)
                {
                    Console.WriteLine($"Defenders lose {attacker_hits} footmen.");
                }
                defending_footmen -= attacker_hits;
            }

            // c for continue
            return 'c';
        }
    }
}