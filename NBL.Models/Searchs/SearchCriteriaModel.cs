namespace NBL.Models.Searchs
{
    public class SearchCriteriaModel
    {
        public int DisplayLength { get; set; }
        public int DisplayStart { get; set; }
        public int SortColomnIndex { get; set; }
        public string SortDirection { get; set; }
        public string Search { get; set; }
    }
}
