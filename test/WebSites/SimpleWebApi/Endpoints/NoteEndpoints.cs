using ExplicitlyImpl.AspNetCore.Mvc.FluentActions;
using System;

namespace SimpleWebApi
{
    public static class FluentNoteActions
    {
        public static Action<FluentActionCollection> All => actions =>
        {
            actions
                .Map("/api/notes", HttpMethod.Get)
                .UsingService<INoteService>()
                .To(noteService => noteService.List());

            actions
                .Map("/api/notes", HttpMethod.Post)
                .UsingService<INoteService>()
                .UsingBody<NoteItem>()
                .To((noteService, note) => noteService.Add(note));

            actions
                .Map("/api/notes/{noteId}", HttpMethod.Get)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .To((noteService, noteId) => noteService.Get(noteId));

            actions
                .Map("/api/notes/{noteId}", HttpMethod.Put)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .UsingBody<NoteItem>()
                .To((noteService, noteId, note) => noteService.Update(noteId, note));

            actions
                .Map("/api/notes/{noteId}", HttpMethod.Delete)
                .UsingService<INoteService>()
                .UsingRouteParameter<int>("noteId")
                .To((noteService, noteId) => noteService.Remove(noteId));
        };
    }
}
