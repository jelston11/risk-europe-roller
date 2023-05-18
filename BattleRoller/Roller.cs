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
            defenders_set = true;
        }

        public void Roll()
        {

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