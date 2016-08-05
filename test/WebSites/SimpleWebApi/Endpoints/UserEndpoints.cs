using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class FluentUserActions
    {
        public static FluentActionCollection AllInline => new FluentActionCollection
        {
            new FluentAction("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .To(userService => userService.List()),

            new FluentAction("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user)),

            new FluentAction("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId)),

            new FluentAction("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user)),

            new FluentAction("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId)),
        };
    }
}
