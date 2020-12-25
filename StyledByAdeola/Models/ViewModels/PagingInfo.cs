using System;

namespace StyledByAdeola.Models.ViewModels
{
    public class PagingInfo
    {
        public int TotalIteams { get; set; }
        public int IteamsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalIteams / IteamsPerPage);
    }
}
