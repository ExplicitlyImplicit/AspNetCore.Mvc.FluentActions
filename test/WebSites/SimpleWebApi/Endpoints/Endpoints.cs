using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class FluentActions
    {
        public static FluentActionCollection AllExternal => FluentActionCollection.DefineActions(actions =>
        {
            actions.Add(FluentUserActions.All);
            actions.Add(FluentNoteActions.All);
        });

        public static FluentActionCollection All => FluentActionCollection.DefineActions(actions =>
        {
            actions
                .Route("/api/users", HttpMethod.Get)
                .UsingService<IUserService>()
                .To(userService => userService.List());

            actions
                .Route("/api/users", HttpMethod.Post)
                .UsingService<IUserService>()
                .UsingBody<UserItem>()
                .To((userService, user) => userService.Add(user));

            actions
                .Route("/api/users/{userId}", HttpMethod.Get)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Get(userId));

            actions
                .Route("/api/users/{userId}", HttpMethod.Put)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .UsingBody<UserItem>()
                .To((userService, userId, user) => userService.Update(userId, user));

            actions
                .Route("/api/users/{userId}", HttpMethod.Delete)
                .UsingService<IUserService>()
                .UsingRouteParameter<int>("userId")
                .To((userService, userId) => userService.Remove(userId));
        });
    }
}
