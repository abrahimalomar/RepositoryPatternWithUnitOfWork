
using System.Collections.Generic;


namespace RepositoryPatternWithUnitOfWork.Core.Model
{
   public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }

        public List<Book> books { get; set; } = new List<Book>();
    }
}
