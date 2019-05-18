using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcWithSwagger
{
    public class UserItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface IUserService
    {
        IList<UserItem> List();
        Task<IList<UserItem>> ListAsync();

        EntityAddResultItem Add(UserItem user);
        Task<EntityAddResultItem> AddAsync(UserItem user);

        UserItem Get(int userId);
        Task<UserItem> GetAsync(int userId);

        EntityUpdateResultItem Update(int userId, UserItem user);
        Task<EntityUpdateResultItem> UpdateAsync(int userId, UserItem user);

        EntityRemoveResultItem Remove(int userId);
        Task<EntityRemoveResultItem> RemoveAsync(int userId);
    }

    public class UserService : IUserService
    {
        public IList<UserItem> List()
        {
            return new List<UserItem>
            {
                new UserItem
                {
                    Id = 1,
                    Name = "Henge Hengberg"
                },
                new UserItem
                {
                    Id = 2,
                    Name = "Yrke Yrkesson"
                },
                new UserItem
                {
                    Id = 3,
                    Name = "Ulla Ullakvist"
                }
            };
        }

        public async Task<IList<UserItem>> ListAsync()
        {
            await Task.Delay(200);
            return List();
        }

        public EntityAddResultItem Add(UserItem user)
        {
            return new EntityAddResultItem
            {
                Id = DateTimeOffset.Now.Millisecond,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityAddResultItem> AddAsync(UserItem user)
        {
            await Task.Delay(200);
            return Add(user);
        }

        public UserItem Get(int userId)
        {
            return new UserItem
            {
                Id = 1,
                Name = "Henge Hengberg"
            };
        }

        public async Task<UserItem> GetAsync(int userId)
        {
            await Task.Delay(200);
            return Get(userId);
        }

        public EntityUpdateResultItem Update(int userId, UserItem user)
        {
            return new EntityUpdateResultItem
            {
                Id = userId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityUpdateResultItem> UpdateAsync(int userId, UserItem user)
        {
            await Task.Delay(200);
            return Update(userId, user);
        }

        public EntityRemoveResultItem Remove(int userId)
        {
            return new EntityRemoveResultItem
            {
                Id = userId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityRemoveResultItem> RemoveAsync(int userId)
        {
            await Task.Delay(200);
            return Remove(userId);
        }
    }

    public class EntityAddResultItem
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class EntityRemoveResultItem
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public class EntityUpdateResultItem
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
