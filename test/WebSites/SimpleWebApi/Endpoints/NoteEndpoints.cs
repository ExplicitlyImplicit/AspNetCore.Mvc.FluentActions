using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;

namespace SimpleWebApi
{
    public static class NoteEndpoints
    {
        public static EndpointCollection AllInline => new EndpointCollection
        {
            new Endpoint("/api/notes", HttpMethod.Get)
                .UsingService<INoteService>()
                .HandledBy(noteService => noteService.List()),

            new Endpoint("/api/notes", HttpMethod.Post)
                .UsingService<INoteService>()
                .UsingBody<NoteItem>()
                .HandledBy((noteService, note) => noteService.Add(note)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Get)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .HandledBy((noteService, noteId) => noteService.Get(noteId)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Put)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .UsingBody<NoteItem>()
                .HandledBy((noteService, noteId, note) => noteService.Update(noteId, note)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Delete)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .HandledBy((noteService, noteId) => noteService.Remove(noteId)),
        };
    }
}
