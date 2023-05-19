using BattleRoller;

Roller roller = new(2);

roller.SetAttackingArmy(1, 1, 1, 1);
roller.SetDefendingArmy(1, 1, 1, 1, false);

while (true)
{
    string? input = Console.ReadLine();
    if (input == "r" || input == "roll")
    {
        if (roller.Roll(false))
        {
            break;
        }
    }
    else
    {
        break;
    }

}