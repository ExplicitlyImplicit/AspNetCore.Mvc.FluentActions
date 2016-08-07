using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System;

namespace SimpleWebApi
{
    public static class FluentActions
    {
        public static Action<FluentActionCollection> AllExternal => actions =>
        {
            FluentUserActions.All(actions);
            FluentNoteActions.All(actions);
        };

        public static Action<FluentActionCollection> All => actions =>
        {
            actions
                .Map("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .To(userService => userService.List());

            actions
                .Map("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user));

            actions
                .Map("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId));

            actions
                .Map("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user));

            actions
                .Map("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId));
        };
    }
}
