﻿
using System;

// ReSharper disable once CheckNamespace
namespace JetBrains.Annotations
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter |
        AttributeTargets.Property | AttributeTargets.Delegate |
        AttributeTargets.Field)]
    internal sealed class NotNullAttribute : Attribute
    {
    }
}