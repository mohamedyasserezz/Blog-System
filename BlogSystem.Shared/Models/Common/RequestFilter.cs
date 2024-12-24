﻿namespace BlogSystem.Shared.Models.Common
{
    public record RequestFilter
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
