namespace BillingService.Domain.Common
{
    public class Entity
    {
        public Guid Id { get; protected set; }

        protected Entity(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Идентификатор не может быть пустым.", nameof(id));

            Id = id;
        }

        protected Entity() { }
    }
}
