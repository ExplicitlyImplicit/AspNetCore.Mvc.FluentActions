using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System;

namespace SimpleWebApi
{
    public static class FluentUserActions
    {
        public static Action<FluentActionCollection> All => actions =>
        {
            actions
                .AddRoute("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .To(userService => userService.List());

            actions
                .AddRoute("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user));

            actions
                .AddRoute("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId));

            actions
                .AddRoute("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user));

            actions
                .AddRoute("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId));
        };
    }
}
