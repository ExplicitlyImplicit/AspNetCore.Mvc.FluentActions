using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleWebApi
{
    public class NoteItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public interface INoteService
    {
        IList<NoteItem> List();
        Task<IList<NoteItem>> ListAsync();

        EntityAddResultItem Add(NoteItem note);
        Task<EntityAddResultItem> AddAsync(NoteItem note);

        NoteItem Get(int noteId);
        Task<NoteItem> GetAsync(int noteId);

        EntityUpdateResultItem Update(int noteId, NoteItem note);
        Task<EntityUpdateResultItem> UpdateAsync(int noteId, NoteItem note);

        EntityRemoveResultItem Remove(int noteId);
        Task<EntityRemoveResultItem> RemoveAsync(int noteId);
    }

    public class NoteService : INoteService
    {
        public IList<NoteItem> List()
        {
            return new List<NoteItem>
            {
                new NoteItem
                {
                    Id = 1,
                    Name = "Henge Hengberg"
                },
                new NoteItem
                {
                    Id = 2,
                    Name = "Yrke Yrkesson"
                },
                new NoteItem
                {
                    Id = 3,
                    Name = "Ulla Ullakvist"
                }
            };
        }

        public async Task<IList<NoteItem>> ListAsync()
        {
            await Task.Delay(200);
            return List();
        }

        public EntityAddResultItem Add(NoteItem note)
        {
            return new EntityAddResultItem
            {
                Id = DateTimeOffset.Now.Millisecond,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityAddResultItem> AddAsync(NoteItem note)
        {
            await Task.Delay(200);
            return Add(note);
        }

        public NoteItem Get(int noteId)
        {
            return new NoteItem
            {
                Id = 1,
                Name = "Henge Hengberg"
            };
        }

        public async Task<NoteItem> GetAsync(int noteId)
        {
            await Task.Delay(200);
            return Get(noteId);
        }

        public EntityUpdateResultItem Update(int noteId, NoteItem note)
        {
            return new EntityUpdateResultItem
            {
                Id = noteId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityUpdateResultItem> UpdateAsync(int noteId, NoteItem note)
        {
            await Task.Delay(200);
            return Update(noteId, note);
        }

        public EntityRemoveResultItem Remove(int noteId)
        {
            return new EntityRemoveResultItem
            {
                Id = noteId,
                Timestamp = DateTimeOffset.Now
            };
        }

        public async Task<EntityRemoveResultItem> RemoveAsync(int noteId)
        {
            await Task.Delay(200);
            return Remove(noteId);
        }
    }
}
