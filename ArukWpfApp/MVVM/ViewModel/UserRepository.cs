using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArukWpfApp.MVVM.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ArukWpfApp.MVVM.ViewModel
{
    public class UserRepository
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserRepository(IMongoDatabase database)
        {
            _userCollection = database.GetCollection<User>("users");
        }

        public void AddUser(User user)
        {
            _userCollection.InsertOne(user);
        }


        public void UpdateUser(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            _userCollection.ReplaceOne(filter, user);
        }



        public void DeleteUser(ObjectId userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            _userCollection.DeleteOne(filter);
        }

        public List<User> GetAllUsers()
        {
            return _userCollection.Find(new BsonDocument()).ToList();
        }


    }
}
