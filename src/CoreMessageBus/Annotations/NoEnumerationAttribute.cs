using System;

namespace CoreMessageBus.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class NoEnumerationAttribute : Attribute
    {
    }
}