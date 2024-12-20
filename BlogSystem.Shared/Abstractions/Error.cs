﻿namespace BlogSystem.Shared.Abstractions
{
    public record Error(string code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }
}
