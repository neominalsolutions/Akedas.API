using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Akedas.API.RouteModels
{
  /*
   * {"ProductName":P-1,}
   * {"productName":"p-1"}
   * 
   */
  public class FilterRequest
  {
    [DefaultValue(10)] // swagger yansıması için yaptık
    public int Page { get; set; } = 10;

    [DefaultValue(10)]
    public int Limit { get; set; } = 10;

    [DefaultValue("")]
    public string? SearchText { get; set; } = string.Empty;

  }
}
