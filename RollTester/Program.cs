using BattleRoller;

Roller roller = new(2);

roller.SetAttackingArmy(1, 3, 3, 5);
roller.SetDefendingArmy(1, 3, 3, 5, false);

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