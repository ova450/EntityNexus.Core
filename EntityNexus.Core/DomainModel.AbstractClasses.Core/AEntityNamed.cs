namespace EntityNexus.DomainModel.AbstractClasses.Core
{
    public abstract class AEntityNamed(string name) : AEntity
    {
        public string Name { get; private set; } 
            = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be empty");

        public void Rename(string name)
            => Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be empty");
    }
}