using ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints;

namespace SimpleWebApi
{
    public static class Endpoints
    {
        public static EndpointCollection AllExternal => new EndpointCollection(new[] 
        {
            UserEndpoints.AllInline,
            NoteEndpoints.AllInline
        });

        public static EndpointCollection AllInline => new EndpointCollection
        {
            new Endpoint("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .HandledBy(userService => userService.List()),

            new Endpoint("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingModelFromBody<UserItem>()
                .HandledBy((userService, user) => userService.Add(user)),

            new Endpoint("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingUrlParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Get(userId)),

            new Endpoint("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingUrlParameter<int>("userId")
                .UsingModelFromBody<UserItem>()
                .HandledBy((userService, userId, user) => userService.Update(userId, user)),

            new Endpoint("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingUrlParameter<int>("userId")
                .HandledBy((userService, userId) => userService.Remove(userId)),
        };
    }
}
