
using System.Collections.Generic;


namespace RepositoryPatternWithUnitOfWork.Core.Model
{
  public  class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public List<Book> books { get; set; } = new List<Book>();
    }
}
