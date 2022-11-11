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
            //panel.Header = new PanelHeader("title");
            await r.EditRecipeOrCategory(Console.ReadLine(), Convert.ToInt32(Console.ReadLine()));
            await r.ListRecipes();
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
            .MoreChoicesText("[blue](Move up and down to reveal more recipes)[/]")
            .AddChoices(allTitles));


            string sDcetailsOfRecips = "";
            foreach (var Row in recipes)
            {
                if(selectRecipe==Row.Title)
                {
                    sDcetailsOfRecips += "Id: " + Row.Id;
                    sDcetailsOfRecips += "Title: " + Row.Title + "\n";
                    sDcetailsOfRecips += "Ingredients: " + Row.Ingredients + "\n";
                    sDcetailsOfRecips += "Instructions: " + Row.Instructions + "\n";
                    sDcetailsOfRecips += "Categories: " + Row.Categories + "\n";
                }
                var panelOfrecipe = new Panel(sDcetailsOfRecips);
                 AnsiConsole.Render(panelOfrecipe);
            }

            return allTitles;
        }

        public async Task EditRecipeOrCategory(string newText, int line_to_edit)
        {

            string jsonStringToEdit = await File.ReadAllTextAsync(@"Recipe.json");
            recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonStringToEdit);
            //string[] editRecip = new string[recipes.Count];
            recipes[line_to_edit].Title = Console.ReadLine();
            recipes[line_to_edit].Ingredients = Console.ReadLine();
            recipes[line_to_edit].Instructions = Console.ReadLine();
            recipes[line_to_edit].Categories = Console.ReadLine();
            //var selectRecipe = AnsiConsole.Prompt
            //(new SelectionPrompt<string[]>()
            //.Title("Edit recipe")
            //.PageSize(4)
            //.AddChoices( recipes
            //recipes[line_to_edit].Title,
            //));
            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);

            Console.WriteLine(recipes[line_to_edit].Title);
        }

    }
}
