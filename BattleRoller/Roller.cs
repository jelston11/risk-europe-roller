namespace BattleRoller
{
    public class Roller
    {
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

        public Roller() 
        {
            attackers_set = false;
            defenders_set = false;

            // 0 is siege, 1 is archers, 2 is cavalry, 3 is footmen
            AttackerLosses = new int[4];
            DefenderLosses = new int[4];

            Victor = 'n';

            Rank = -1;

            random = new();
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
        }

        public bool Roll(bool reroll)
        {
            if (defenders_set && attackers_set) 
            {
                return false;
            }

            // Increment rank and get rank to use for this roll
            if (on_castle && reroll)
            {
                if (!has_reroll)
                {
                    return false;
                }
                has_reroll = false;
            }
            else
            {
                Rank++;
                if (Rank == 4)
                {
                    Rank = 0;
                    if (on_castle)
                    {
                        has_reroll = true;
                    }
                }
                if (Rank == 0 && attacking_siege == 0 && defending_siege == 0)
                {
                    Rank = 1;
                }
                if (Rank == 1 && attacking_archers == 0 && defending_archers == 0)
                {
                    Rank = 2;
                }
                if (Rank == 2 && attacking_cavalry == 0 && defending_cavalry == 0)
                {
                    Rank = 3;
                }
                if (Rank == 3 && attacking_footmen == 0 && defending_footmen == 0)
                {
                    return false;
                }
            }

            int attack_hits = 0;
            int defense_hits = 0;

            if (Rank == 0)
            {
                // Siege Attack (hit on 3+)
                if (!reroll)
                {
                    for (int i = 0; i < attacking_siege; i++)
                    {
                        // 2 rolls for each siege weapon
                        if (random.Next(1, 7) >= 3)
                        {
                            attack_hits++;
                        }
                        if (random.Next(1, 7) >= 3)
                        {
                            attack_hits++;
                        }
                    }
                }

                for (int i = 0; i < defending_siege; i++)
                {
                    // 2 rolls for each siege weapon
                    if (random.Next(1, 7) >= 3)
                    {
                        defense_hits++;
                    }
                    if (random.Next(1, 7) >= 3)
                    {
                        defense_hits++;
                    }
                }
            }
            else if (Rank == 1)
            {
                // Archer Volley (hit on 5+)
                if (!reroll)
                {
                    for (int i = 0; i < attacking_archers; i++)
                    {
                        if (random.Next(1, 7) >= 5)
                        {
                            attack_hits++;
                        }
                    }
                }

                for (int i = 0; i < defending_archers; i++)
                {
                    if (random.Next(1, 7) >= 5)
                    {
                        defense_hits++;
                    }
                }
            } 
            else if (Rank == 2)
            {
                // Cavalry Charge (hit on 3+)
                if (!reroll)
                {
                    for (int i = 0; i < attacking_cavalry; i++)
                    {
                        if (random.Next(1, 7) >= 3)
                        {
                            attack_hits++;
                        }
                    }
                }

                for (int i = 0; i < defending_cavalry; i++)
                {
                    if (random.Next(1, 7) >= 3)
                    {
                        defense_hits++;
                    }
                }
            }
            else
            {
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
                if (AttackingArmySize == 1)
                {
                    attack_roll_one = random.Next(1, 7);
                }
                
                if (DefendingArmySize > 1)
                {
                    defense_roll_two = random.Next(1, 7);
                }
                if (DefendingArmySize == 1)
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
                    attack_hits++;
                } 
                else
                {
                    defense_hits++;
                }

                if (attack_roll_two != -1 && defense_roll_two != -1)
                {
                    if (attack_roll_two > defense_roll_two)
                    {
                        attack_hits++;
                    }
                    else
                    {
                        defense_hits++;
                    }
                }
            }

            char battle_result = EliminateUnits(attack_hits, defense_hits);
            if (battle_result == 'c')
            {
                return false;
            }
            else
            {
                Victor = battle_result;
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
            if (defender_hits >= DefendingArmySize && attacker_hits >= AttackingArmySize)
            {
                // i for inconclusive (draw)
                return 'i';
            }
            else if (defender_hits >= DefendingArmySize)
            {
                // a for attacker win
                return 'a';
            }
            else if (attacker_hits >= AttackingArmySize)
            {
                // d for defender win
                return 'd';
            }

            // Determine new army sizes after hits
            DefendingArmySize -= attacker_hits;
            AttackingArmySize -= defender_hits;

            // Eliminating attackers
            if (attacking_footmen < defender_hits)
            {
                AttackerLosses[3] += attacking_footmen;
                attacking_footmen = 0;
                defender_hits -= attacking_footmen;
                if (attacking_archers < defender_hits)
                {
                    AttackerLosses[1] += attacking_archers;
                    attacking_archers = 0;
                    defender_hits -= attacking_archers;
                    if (attacking_cavalry < defender_hits)
                    {
                        AttackerLosses[2] += attacking_cavalry;
                        attacking_cavalry = 0;
                        defender_hits -= attacking_cavalry;
                        if (attacking_siege <= defender_hits)
                        {
                            AttackerLosses[0] += attacking_siege;
                            attacking_siege = 0;
                        }
                        else
                        {
                            AttackerLosses[0] += defender_hits;
                            attacking_siege -= defender_hits;
                        }
                    }
                    else
                    {
                        AttackerLosses[2] += defender_hits;
                        attacking_cavalry -= defender_hits;
                    }
                } 
                else
                {
                    AttackerLosses[1] += defender_hits;
                    attacking_archers -= defender_hits;
                }
            }
            else
            {
                AttackerLosses[3] += defender_hits;
                attacking_footmen -= defender_hits;
            }

            // Eliminating defenders
            if (defending_footmen < attacker_hits)
            {
                DefenderLosses[3] += defending_footmen;
                defending_footmen = 0;
                attacker_hits -= defending_footmen;
                if (defending_archers < attacker_hits)
                {
                    DefenderLosses[1] += defending_archers;
                    defending_archers = 0;
                    attacker_hits -= defending_archers;
                    if (defending_cavalry < attacker_hits)
                    {
                        DefenderLosses[2] += defending_cavalry;
                        defending_cavalry = 0;
                        attacker_hits -= defending_cavalry;
                        if (defending_siege <= attacker_hits)
                        {
                            DefenderLosses[0] += defending_siege;
                            defending_siege = 0;
                        }
                        else
                        {
                            DefenderLosses[0] += attacker_hits;
                            defending_siege -= attacker_hits;
                        }
                    }
                    else
                    {
                        DefenderLosses[2] += attacker_hits;
                        defending_cavalry -= attacker_hits;
                    }
                }
                else
                {
                    DefenderLosses[1] += attacker_hits;
                    defending_archers -= attacker_hits;
                }
            }
            else
            {
                DefenderLosses[3] += attacker_hits;
                defending_footmen -= attacker_hits;
            }

            // c for continue
            return 'c';
        }
    }
}