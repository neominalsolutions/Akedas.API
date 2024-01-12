using Akedas.API.Data.Contexts;
using Akedas.API.Features.Posts.Requests;
using FluentValidation;

namespace Akedas.API.Features.Posts.Validators
{
  public class CreatePostRequestValidator:AbstractValidator<CreatePostRequest>
  {
    private readonly AppDbContext db;

    public CreatePostRequestValidator(AppDbContext db)
    {
      this.db = db;

      RuleFor(x => x.Title)
        .NotEmpty()
        .WithMessage("Title Boş Geçilemez")
        .NotNull()
        .WithMessage("Title Null geçilemez")
        .NotEqual("Makale")
        .WithMessage("Sadece makale ismi verilemez")
        .MaximumLength(20)
        .WithMessage("Title için 20 karakter sınırını aşamayız");

      RuleFor(x => x.Title).Must(TitleMustUnique).WithMessage("Title Unique olmalıdır");
    }

    public bool TitleMustUnique(string title)
    {
      return db.Posts.Any(x => x.Title == title) ? false : true;
    }
  }
}
