﻿namespace projetStage.DTO
{
    public class PaginatedResponse<T>
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public List<T> Items { get; set; }
    }
}
