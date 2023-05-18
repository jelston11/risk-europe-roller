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

        private int rank;

        private bool attackers_set;
        private bool defenders_set;

        // random.Next(1, 7); to simulate dice roll
        private Random random;

        public int Rolls { get; private set; }
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

            rank = -1;

            random = new();
        }

        public void SetAttackingArmy(int siege, int archers, int cavalry, int footmen)
        {
            attacking_siege = siege;
            attacking_archers = archers;
            attacking_cavalry = cavalry;
            attacking_footmen = footmen;
            attackers_set = true;
        }

        public void SetDefendingArmy(int siege, int archers, int cavalry, int footmen, bool castle)
        {
            defending_siege = siege;
            defending_archers = archers;
            defending_cavalry = cavalry;
            defending_footmen = footmen;
            on_castle = castle;
            has_reroll = castle;
            defenders_set = true;
        }

        public bool Roll(bool reroll)
        {
            // Increment rank and get rank to use for this roll
            if (reroll)
            {
                if (!has_reroll)
                {
                    return false;
                }
                has_reroll = false;
            }
            else
            {
                int new_rank = rank++;
                if (new_rank == 0 && attacking_siege == 0 && defending_siege == 0)
                {
                    new_rank = 1;
                }
                if (new_rank == 1 && attacking_archers == 0 && defending_archers == 0)
                {
                    new_rank = 2;
                }
                if (new_rank == 2 && attacking_cavalry == 0 && defending_cavalry == 0)
                {
                    new_rank = 3;
                }
                if (new_rank == 3 && attacking_footmen == 0 && defending_footmen == 0)
                {
                    return false;
                }
                if (new_rank == 4)
                {
                    // reset reroll ability
                }
            }
            /*if (rank == 0 && on_castle)
            {
                // reset the reroll ability
                has_reroll = true;
            }

            if (reroll)
            {
                // do not increment rank on rerolls
                if (!has_reroll)
                {
                    return;
                }

                if (rank == 0)
                {
                    current_rank = 4;
                }
                else
                {
                    current_rank = rank - 1;
                }
                has_reroll = false;
            }
            else
            {
                rank++;
                if (rank == 4)
                {
                    rank = 0;
                }
                current_rank = rank;
            }*/

            int attack_hits = 0;
            int defense_hits = 0;

            if (current_rank == 0)
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
            else if (current_rank == 1)
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
            else if (current_rank == 2)
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
                // General Attack
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
            bool attacker_depleted = false;
            bool defender_depleted = false;

            // Eliminating attackers
            if (attacking_footmen < defender_hits)
            {
                attacking_footmen = 0;
                defender_hits -= attacking_footmen;
                if (attacking_archers < defender_hits)
                {
                    attacking_archers = 0;
                    defender_hits -= attacking_archers;
                    if (attacking_cavalry < defender_hits)
                    {
                        attacking_cavalry = 0;
                        defender_hits -= attacking_cavalry;
                        if (attacking_siege <= defender_hits)
                        {
                            attacking_siege = 0;
                            attacker_depleted = true;
                        }
                        else
                        {
                            attacking_siege -= defender_hits;
                        }
                    }
                    else
                    {
                        attacking_cavalry -= defender_hits;
                    }
                } 
                else
                {
                    attacking_archers -= defender_hits;
                }
            }
            else
            {
                attacking_footmen -= defender_hits;
            }

            // Eliminating defenders
            if (defending_footmen < attacker_hits)
            {
                defending_footmen = 0;
                attacker_hits -= defending_footmen;
                if (defending_archers < attacker_hits)
                {
                    defending_archers = 0;
                    attacker_hits -= defending_archers;
                    if (defending_cavalry < attacker_hits)
                    {
                        defending_cavalry = 0;
                        attacker_hits -= defending_cavalry;
                        if (defending_siege <= attacker_hits)
                        {
                            defending_siege = 0;
                            defender_depleted = true;
                        }
                        else
                        {
                            defending_siege -= attacker_hits;
                        }
                    }
                    else
                    {
                        defending_cavalry -= attacker_hits;
                    }
                }
                else
                {
                    defending_archers -= attacker_hits;
                }
            }
            else
            {
                defending_footmen -= attacker_hits;
            }

            if (defender_depleted && attacker_depleted)
            {
                return 'i';
            } 
            else if (defender_depleted)
            {
                return 'a';
            }
            else if (attacker_depleted)
            {
                return 'd';
            }
            else
            {
                return 'c';
            }
        }
    }
}