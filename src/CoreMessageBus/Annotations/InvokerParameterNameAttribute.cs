using System;

namespace CoreMessageBus.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class InvokerParameterNameAttribute : Attribute
    {
    }
}