using ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints;

namespace SimpleWebApi
{
    public static class UserEndpoints
    {
        public static EndpointCollection AllInline => new EndpointCollection
        {
            new Endpoint("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .HandledBy(userService => userService.List()),

            new Endpoint("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .HandledBy((userService, user) => userService.Add(user)),

            new Endpoint("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Get(userId)),

            new Endpoint("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .HandledBy((userService, userId, user) => userService.Update(userId, user)),

            new Endpoint("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Remove(userId)),
        };
    }
}
