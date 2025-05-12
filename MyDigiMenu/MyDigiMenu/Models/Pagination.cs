
namespace MyDigiMenu.Models
{
    public class Pagination
    {
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public SearchData data { get; set; }
    }
    public class SearchData
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}