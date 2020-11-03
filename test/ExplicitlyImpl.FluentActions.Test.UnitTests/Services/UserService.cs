using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExplicitlyImpl.FluentActions.Test.UnitTests
{
    public class UserItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is UserItem && ((UserItem)obj).Id == Id && ((UserItem)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return $"{Id}-{Name}".GetHashCode();
        }
    }

    public interface IUserService
    {
        IList<UserItem> ListUsers();
        Task<IList<UserItem>> ListUsersAsync();

        EntityAddResultItem AddUser(UserItem user);
        Task<EntityAddResultItem> AddUserAsync(UserItem user);

        UserItem GetUserById(int userId);
        Task<UserItem> GetUserByIdAsync(int userId);

        EntityUpdateResultItem UpdateUser(int userId, UserItem user);
        Task<EntityUpdateResultItem> UpdateUserAsync(int userId, UserItem user);

        EntityRemoveResultItem RemoveUser(int userId);
        Task<EntityRemoveResultItem> RemoveUserAsync(int userId);
    }

    public class UserService : IUserService
    {
        public IList<UserItem> ListUsers()
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

        public async Task<IList<UserItem>> ListUsersAsync()
        {
            await Task.Delay(1);
            return ListUsers();
        }

        public EntityAddResultItem AddUser(UserItem user)
        {
            return new EntityAddResultItem
            {
                Id = DateTimeOffset.Now.Millisecond,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityAddResultItem> AddUserAsync(UserItem user)
        {
            await Task.Delay(1);
            return AddUser(user);
        }

        public UserItem GetUserById(int userId)
        {
            return new UserItem
            {
                Id = 1,
                Name = "Henge Hengberg"
            };
        }

        public async Task<UserItem> GetUserByIdAsync(int userId)
        {
            await Task.Delay(1);
            return GetUserById(userId);
        }

        public EntityUpdateResultItem UpdateUser(int userId, UserItem user)
        {
            return new EntityUpdateResultItem
            {
                Id = userId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityUpdateResultItem> UpdateUserAsync(int userId, UserItem user)
        {
            await Task.Delay(1);
            return UpdateUser(userId, user);
        }

        public EntityRemoveResultItem RemoveUser(int userId)
        {
            return new EntityRemoveResultItem
            {
                Id = userId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityRemoveResultItem> RemoveUserAsync(int userId)
        {
            await Task.Delay(1);
            return RemoveUser(userId);
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
