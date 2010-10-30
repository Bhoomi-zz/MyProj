﻿using System;
using Ncqrs;
using Events;
using Ncqrs.Domain;

namespace Domain
{
    public class Note : AggregateRootMappedByConvention
    {
        public Guid Id
        {
            get{ return EventSourceId; }
            set{ EventSourceId = value; }
        }

        private String _text;
        private DateTime _creationDate;

        private Note()
        {
            // Need a default ctor for Ncqrs.
        }

        public Note(String text)
        {
            var clock = NcqrsEnvironment.Get<IClock>();

            // Apply a NewNoteAdded event that reflects the
            // creation of this instance. The state of this
            // instance will be update in the handler of 
            // this event (the OnNewNoteAdded method).
            ApplyEvent(new NewNoteAdded
            {
                NoteId = Id,
                Text = text,
                CreationDate = clock.UtcNow()
            });
        }

        public void ChangeText(String newText)
        {
            // Apply a NoteTextChanged event that reflects
            // the occurence of a text change. The state of this
            // instance will be update in the handler of 
            // this event (the NoteTextChanged method).
            ApplyEvent(new NoteTextChanged
            {
                NoteId = Id,
                NewText = newText
            });
        }

        // Event handler for the NewNoteAdded event. This method
        // is automaticly wired as event handler based on convension.
        protected void OnNewNoteAdded(NewNoteAdded e)
        {
            Id = e.NoteId;
            _text = e.Text;
            _creationDate = e.CreationDate;
        }

        // Event handler for the NoteTextChanged event. This method
        // is automaticly wired as event handler based on convension.
        protected void OnNoteTextChanged(NoteTextChanged e)
        {
            _text = e.NewText;
        }
    }
}
