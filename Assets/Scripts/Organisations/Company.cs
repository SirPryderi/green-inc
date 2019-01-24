namespace Organisations
{
    public class Company : Organisation
    {
        public readonly CompanyType Type;

        public Company(string name, CompanyType type) : base(name)
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