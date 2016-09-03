using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class UserActions
    {
        public static FluentActionCollection All => FluentActionCollection.DefineActions(actions =>
        {
            actions
                .Route("/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .To(userService => userService.List());

            actions
                .Route("/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user));

            actions
                .Route("/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId));

            actions
                .Route("/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user));

            actions
                .Route("/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId));
        });
    }
}
