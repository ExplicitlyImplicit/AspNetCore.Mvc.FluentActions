using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleApi
{
    public static class NoteActions
    {
        public static FluentActionCollection All => FluentActionCollection.DefineActions(
            actions =>
            {
                actions.Configure(config =>
                {
                    config.GroupBy("NoteActions");
                });

                actions
                    .Route("/notes", HttpMethod.Get)
                    .UsingService<INoteService>()
                    .To(noteService => noteService.List());

                actions
                    .Route("/notes", HttpMethod.Post)
                    .UsingService<INoteService>()
                    .UsingBody<NoteItem>()
                    .To((noteService, note) => noteService.Add(note));

                actions
                    .Route("/notes/{noteId}", HttpMethod.Get)
                    .UsingService<INoteService>()
                    .UsingRouteParameter<int>("noteId")
                    .To((noteService, noteId) => noteService.Get(noteId));

                actions
                    .Route("/notes/{noteId}", HttpMethod.Put)
                    .UsingService<INoteService>()
                    .UsingRouteParameter<int>("noteId")
                    .UsingBody<NoteItem>()
                    .To((noteService, noteId, note) => noteService.Update(noteId, note));

                actions
                    .Route("/notes/{noteId}", HttpMethod.Delete)
                    .UsingService<INoteService>()
                    .UsingRouteParameter<int>("noteId")
                    .To((noteService, noteId) => noteService.Remove(noteId));
            }
        );
    }
}
