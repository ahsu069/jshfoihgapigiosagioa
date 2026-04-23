using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace api.Models
{
    public class DataTableRequest
    {
        [DefaultValue(1)]
        public int Draw { get; set; } = 1;

        [DefaultValue(0)]
        public int Start { get; set; } = 0;
        
        [DefaultValue(25)]
        public int Length { get; set; } = 25;
        public SearchRequest? Search { get; set; }
        public List<OrderRequest>? Order { get; set; }
        public List<ColumnRequest>? Columns { get; set; }
    }

    public class SearchRequest
    {
        [DefaultValue("")]
        public string? Value { get; set; } = string.Empty;
    }

    public class OrderRequest
    {

        [DefaultValue(0)]
        public int Column { get; set; } = 0;
        [DefaultValue("asc")]
        public string Dir { get; set; } = "asc";
    }

    public class ColumnRequest
    {
        [DefaultValue("")]
        public string Data { get; set; } = string.Empty;
        // public string Name { get; set; } = "";
        [DefaultValue(true)]
        public bool Searchable { get; set; } = false;
        [DefaultValue(true)]
        public bool Orderable { get; set; } = false;
        [DefaultValue("")]
        public SearchRequest? Search { get; set; }
    }

}