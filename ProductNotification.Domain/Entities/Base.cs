using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProductNotification.Domain.Entities
{
    public abstract class Base
    {
        private const int partitionCharCount = 2;
        private string _id;
        public Base()
        {
            this.Id = ObjectId.GenerateNewId().ToString();
            this.DataExecucao = DateTime.Now;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string Id
        {
            get => string.IsNullOrEmpty(_id) ? ObjectId.GenerateNewId().ToString() : _id.ToString();
            set
            {
                this.partition = value.Length < partitionCharCount ? "00" : value.Substring(value.Length - partitionCharCount);
                _id = value.ToString();
            }
        }
        public DateTime DataExecucao { get; }
        public string partition { get; private set; }
        public abstract bool Valido();
    }
}