﻿// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace SkyBlueSoftware.Framework
{
    public interface ILookup<in TKey, out TValue>
    {
        TValue this[TKey key] { get; }
    }
}