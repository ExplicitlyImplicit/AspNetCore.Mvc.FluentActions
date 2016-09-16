using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class UserActions
    {
        public static FluentActionCollection All => FluentActionCollection.DefineActions(actions =>
        {
            actions.GroupBy("UserActions");

            actions
                .RouteGet("/users", "ListUsers")
                .WithTitle("List Users")
                .UsingService<IUserService>()
                .To(userService => userService.List());

            actions
                .RoutePost("/users", "AddUser")
                .WithTitle("Add User")
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user));

            actions
                .RouteGet("/users/{userId}", "GetUser")
                .WithTitle("Get User")
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId));

            actions
                .RoutePut("/users/{userId}", "UpdateUser")
                .WithTitle("Update User")
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user));

            actions
                .RouteDelete("/users/{userId}", "RemoveUser")
                .WithTitle("Remove User")
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId));
        });
    }
}
