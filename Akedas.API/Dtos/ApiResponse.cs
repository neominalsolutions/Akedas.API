namespace Akedas.API.Dtos
{
  public class ErrorResponse
  {
    public string Name { get; set; }
    public string Description { get; set; }
  }

  public class ApiResponse<T> where T:class
  {
    public T Data { get; set; } // {id:"234234",title:"data"}
    public List<ErrorResponse> Errors { get; set; } // Entity Not Found
    public int Status { get; set; } // 200,201,204

    public string SuccessMessage { get; set; } // Kayıt girildi, silindi


  }
}
