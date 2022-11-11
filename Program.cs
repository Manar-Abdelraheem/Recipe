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
            await r.ListRecipes();
            //  var panel = new Panel("Hello World\nklerfmkmsfslkwmawklafmwlek\nkenakfonkf");

            //panel.Header = new PanelHeader("title");
            // await r.EditRecipeOrCategory(Console.ReadLine(), Convert.ToInt32(Console.ReadLine()));
            //// await r.ListRecipes();
            //  var panel1 = new Panel(await r.ListRecipes());
            // AnsiConsole.Write(panel1);
        }
    }

    public class Recipe
    {
        private static int SharedId { get; set; } 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Categories { get; set; }

        List<Recipe> recipes = new List<Recipe>();

        public async Task AddRecipe()
        {
            string answer;

                recipes.Add(new Recipe
                {
                    Id = SharedId,
                    Title = Console.ReadLine(),
                    Ingredients = Console.ReadLine(),
                    Instructions = Console.ReadLine(),
                    Categories = Console.ReadLine()
                });
                SharedId++;
                Console.WriteLine("Do you want to add another recipe? Yes/No");
                answer = Console.ReadLine();
                if (answer == "yes")
                {
                    await AddRecipe();
                }

            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);

        }

        public async Task<string[]> ListRecipes()
        {
            string[] allTitles = new string[recipes.Count] ;
            string deserializationString = await File.ReadAllTextAsync(@"Recipe.json");
            recipes = JsonSerializer.Deserialize<List<Recipe>>(deserializationString);

            for (int i = 0; i < recipes.Count; i++)
            {
                allTitles[i] = recipes[i].Title;
            }
            var selectRecipe = AnsiConsole.Prompt
            (new SelectionPrompt<string>()
            .Title("Choose the recipe")
            .PageSize(10)
            .MoreChoicesText("[blue](Move up and down to reveal more fruits)[/]")
            .AddChoices(allTitles));
            

            //  AnsiConsole.Write(selectRecipe);

            //string s = "";
            //foreach (var Row in recipes)
            //{
            //    Console.WriteLine(Row.ID);
            //    s += "Title: " + Row.Title + "\n";
            //    s += "Ingredients: " + Row.Ingredients + "\n";
            //    s += "Instructions: " + Row.Instructions + "\n";
            //    s += "Categories: " + Row.Categories + "\n";
            //}

            return allTitles;
        }

        public async Task EditRecipeOrCategory(string newText, int line_to_edit)
        {
            string jsonStringToEdit = await File.ReadAllTextAsync(@"Recipe.json");
            recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonStringToEdit);

            recipes[line_to_edit].Title = newText;

            recipes[line_to_edit].Ingredients = newText;
            recipes[line_to_edit].Instructions = newText;
            recipes[line_to_edit].Categories = newText;
            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);

            Console.WriteLine(recipes[line_to_edit].Title);
        }

    }
}


