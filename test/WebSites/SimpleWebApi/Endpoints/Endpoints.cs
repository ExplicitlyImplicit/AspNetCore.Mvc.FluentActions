using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class FluentActions
    {
        public static FluentActionCollection AllExternal => new FluentActionCollection(new[] 
        {
            FluentUserActions.AllInline,
            FluentNoteActions.AllInline
        });

        public static FluentActionCollection AllInline => new FluentActionCollection
        {
            new FluentAction("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .HandledBy(userService => userService.List()),

            new FluentAction("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .HandledBy((userService, user) => userService.Add(user)),

            new FluentAction("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Get(userId)),

            new FluentAction("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .HandledBy((userService, userId, user) => userService.Update(userId, user)),

            new FluentAction("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Remove(userId)),
        };
    }
}
