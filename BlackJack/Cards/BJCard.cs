namespace BlackJack.Cards
{
    internal interface BJCard
    {
        string name();
        int value();
        int value(int count);
    }
}
