using System;

namespace CoreMessageBus
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.Delegate |
        AttributeTargets.Field)]
    internal sealed class CanBeNullAttribute : Attribute
    {
    }
}