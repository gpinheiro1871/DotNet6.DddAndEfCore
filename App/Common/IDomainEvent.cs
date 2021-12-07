using App.Models;

namespace App.Common;

public interface IDomainEvent
{
}

// An event is a fact that occurred in the past and is naturally immutable,
// so it can only contain immutable data, such as primitive types or value objects
// events cannot contain entities, because entities are mutable
public sealed class StudentEmailChangedEvent : IDomainEvent
{
    public long StudentId { get; }
    public Email NewEmail { get; }

    public StudentEmailChangedEvent(long studentId, Email newEmail)
    {
        StudentId = studentId;
        NewEmail = newEmail;
    }
}

public interface IBus
{
    void Send(string message);
}

public class Bus : IBus
{
    public void Send(string message)
    {
        // Put the message on a bus instead

        Console.WriteLine($"Message sent: '{message}'");
    }
}

public class MessageBus
{
    private readonly IBus _bus;

    public MessageBus(IBus bus)
    {
        _bus = bus;
    }

    public void SendEmailChangedMessage(long studentId, string newEmail)
    {
        _bus.Send("Type: STUDENT_EMAIL_CHANGED; " +
            $"Id: {studentId}; " +
            $"NewEmail: {newEmail}");
    }
}

public class EventDispatcher
{
    private readonly MessageBus _messageBus;

    public EventDispatcher(MessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public void Dispatch(IEnumerable<IDomainEvent> events)
    {
        foreach (IDomainEvent ev in events)
        {
            Dispatch(ev);
        }
    }

    private void Dispatch(IDomainEvent ev)
    {
        switch (ev)
        {
            case StudentEmailChangedEvent emailChangedEvent:
                _messageBus.SendEmailChangedMessage(
                    emailChangedEvent.StudentId, emailChangedEvent.NewEmail);
                break;

            default:
                throw new Exception($"Unknown event type: '{ev.GetType()}'");
        }
    }
}