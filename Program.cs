using System;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;

namespace Recipe
{
    class Program
    {
        static void Main(string[] args)
        {

            Recipe r = new Recipe();
            r.AddRecipe();
            r.ListRecipes();
            r.EditRecipeOrCategory(Console.ReadLine(),Convert.ToInt32(Console.ReadLine()));
        }
    }

    public class Recipe
    {
        public int ID { get; }
        public string Title {get;  set; }
        public string Ingredients { get; set; }
        public string Categories { get; set; }
        public string Instructions { get; set; }

        public void AddRecipe() 
        {
            for (int recNo=0; recNo<=ID;recNo++ )
            {
                var recipe = new Recipe
                {
                    Title = Console.ReadLine(),
                    Ingredients = Console.ReadLine(),
                    Instructions = Console.ReadLine(),                
                    Categories = Console.ReadLine()
                };
                string jsonString = JsonSerializer.Serialize(recipe);
                File.AppendAllText(@"D:\\MANAR\\Recipe Project\\Recipe.json", jsonString+ "\n");
                
               // Recipe recipe = JsonSerializer.Deserialize<Recipe>(jsonString);

                Console.WriteLine("Do you want to add another recipe? Yes/No");
                string answer=Console.ReadLine();
                if (answer == "yes")
                {
                    recipe.AddRecipe();
                }
                else
                {
                    break;
                }
                                
            }
        }

        public void ListRecipes()  
        {
            Console.WriteLine(File.ReadAllText(@"D:\\MANAR\\Recipe Project\\Recipe.json"));
        }

        public void EditRecipeOrCategory(string newText, int line_to_edit) 
        {
            string[] arrOfStrLine = File.ReadAllLines(@"D:\\MANAR\\Recipe Project\\Recipe.json");
            arrOfStrLine[line_to_edit] = newText;
            File.WriteAllLines(@"D:\\MANAR\\Recipe Project\\Recipe.json", arrOfStrLine);
            Console.WriteLine("\n"+ arrOfStrLine);
        }

    }
}
