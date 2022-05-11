namespace KesselRunFramework.DataAccess.Domain
{
    public interface INamedEntity : IEntity
    {
        string Name { get; set; }
    }
}
