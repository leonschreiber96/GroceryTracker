using System.Collections.Generic;

public class ArticleDto
{
   public int Id { get; set; }

   public string Name { get; set; }

   public string Details { get; set; }

   public List<string> Tags { get; set; }
}