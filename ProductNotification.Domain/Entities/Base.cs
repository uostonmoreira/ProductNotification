using System;

namespace ProductNotification.Domain.Entities
{
    public abstract class Base
    {
        public Base()
        {
            this.Id = Guid.NewGuid();
            this.DataExecucao = DateTime.Now;
        }

        public Guid Id { get; }
        public DateTime DataExecucao { get; }

        public abstract bool Valido();
    }
}
