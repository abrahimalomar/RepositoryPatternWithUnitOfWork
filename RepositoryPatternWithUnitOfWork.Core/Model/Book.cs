

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryPatternWithUnitOfWork.Core.Model
{
 public   class Book
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public string ImageSrc { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public int AuthoreId { get; set; }
        public Author Authore { get; set; }

        public int categoryId { get; set; }
        public Category category { get; set; }
    }
}
