using System;

namespace RingDownConsole.Interfaces
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}