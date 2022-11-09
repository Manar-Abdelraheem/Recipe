using System;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Collections.Generic;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Threading.Tasks;

namespace Recipe
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Recipe r = new Recipe();
            await r.AddRecipe();
            await r.EditRecipeOrCategory(Console.ReadLine(), Convert.ToInt32(Console.ReadLine()));
            await r.ListRecipes();
            //r.EditRecipeOrCategory(Console.ReadLine(),Convert.ToInt32(Console.ReadLine()));
            // var panel1 = new Panel(r.ListRecipes());
            //AnsiConsole.Write(panel1);
        }
    }

    public class Recipe
    {
        public int ID { get; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Categories { get; set; }

        List<Recipe> recipes = new List<Recipe>();
        public async Task AddRecipe()
        {
            
            for (int i=0; i<2; i++) 
            {

               recipes.Add ( new Recipe
                {
                    Title = Console.ReadLine(),
                    Ingredients = Console.ReadLine(),
                    Instructions = Console.ReadLine(),
                    Categories = Console.ReadLine()
                });
            }
               string jsonString = JsonSerializer.Serialize(recipes);
               await File.WriteAllTextAsync(@"Recipe.json", jsonString);

                //Console.WriteLine("Do you want to add another recipe? Yes/No");
                //string answer = Console.ReadLine();
        
        }


        public async Task  ListRecipes()
        {

            string deserializationString = await File.ReadAllTextAsync(@"Recipe.json");
             recipes = JsonSerializer.Deserialize<List<Recipe>>(deserializationString);

            foreach (var Row in recipes)
            {
                Console.WriteLine(Row.ID);
                Console.WriteLine(Row.Title);
                Console.WriteLine(Row.Ingredients);
                Console.WriteLine(Row.Instructions);
                Console.WriteLine(Row.Categories);
            }            

        }

        public async Task EditRecipeOrCategory(string newText,int line_to_edit)
        {
            string jsonStringToEdit= await File.ReadAllTextAsync(@"Recipe.json");
            recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonStringToEdit);

            recipes[line_to_edit].Title = newText;
            
            recipes[line_to_edit].Ingredients = newText;
            recipes[line_to_edit].Instructions = newText;
            recipes[line_to_edit].Categories = newText;
            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);

            Console.WriteLine(recipes[line_to_edit].Title );
        }

    }
}

