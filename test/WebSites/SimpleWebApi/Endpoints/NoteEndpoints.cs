using ExplicitlyImpl.AspNetCore.Mvc.FluentEndpoints;

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
                .UsingModelFromBody<NoteItem>()
                .HandledBy((noteService, note) => noteService.Add(note)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Get)
                .UsingService<INoteService>()
                .UsingUrlParameter<int>("noteId")
                .HandledBy((noteService, noteId) => noteService.Get(noteId)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Put)
                .UsingService<INoteService>()
                .UsingUrlParameter<int>("noteId")
                .UsingModelFromBody<NoteItem>()
                .HandledBy((noteService, noteId, note) => noteService.Update(noteId, note)),

            new Endpoint("/api/notes/{noteId}", HttpMethod.Delete)
                .UsingService<INoteService>()
                .UsingUrlParameter<int>("noteId")
                .HandledBy((noteService, noteId) => noteService.Remove(noteId)),
        };
    }
}
