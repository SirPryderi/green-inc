namespace Organisations
{
    public class Company : Organisation
    {
        public readonly CompanyType Type;

        public Company(string name, CompanyType type, int startingMoney = 0) : base(name, startingMoney)
        {
            Type = type;
        }
    }

    public enum CompanyType
    {
        ELECTRICAL,
        FOOD,
        GENERIC
    }
}