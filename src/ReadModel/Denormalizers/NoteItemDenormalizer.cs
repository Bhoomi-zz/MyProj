using System;
using System.Linq;
using Events;
using Ncqrs.Eventing.ServiceModel.Bus;

namespace ReadModel.Denormalizers
{
    public class NoteItemDenormalizer : IEventHandler<NewNoteAdded>,
                                        IEventHandler<NoteTextChanged>
    {
        public void Handle(NewNoteAdded evnt)
        {
            using(var context = new MyNotesReadModelEntities())
            {
                var newItem = new NoteItem
                {
                    Id = evnt.NoteId,
                    Text = evnt.Text,
                    CreationDate = evnt.CreationDate
                };

                context.NoteItems.AddObject(newItem);
                context.SaveChanges();
            }
        }

        public void Handle(NoteTextChanged evnt)
        {
            using (var context = new MyNotesReadModelEntities())
            {
                var itemToUpdate = context.NoteItems.Single(item => item.Id == evnt.NoteId);
                itemToUpdate.Text = evnt.NewText;
            
                context.SaveChanges();
            }
        }
    }
}
