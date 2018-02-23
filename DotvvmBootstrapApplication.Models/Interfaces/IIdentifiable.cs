using System;

namespace DotvvmBootstrapApplication.Interfaces
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}