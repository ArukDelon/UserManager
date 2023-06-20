using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ArukWpfApp.MVVM.Model
{
    public class User
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public uint Age { get; set; }

        public bool ActiveStatus { get; set; }

        public string Email { get; set; }

        public string Info { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
