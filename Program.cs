using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using Spectre.Console;
using System.Threading.Tasks;
namespace Recipe
{
    class Program
    {
        static async Task Main(String[] args)
        {
            Recipe r = new Recipe();
        startPoint1:
            var ruleUp = new Rule($"[bold deeppink2]Recipe[/]");
            AnsiConsole.Write(ruleUp);
            var chooseAction = AnsiConsole.Prompt
            (new SelectionPrompt<string>()
            .Title("[gold1]Move up and down to reveal more actions\nPress Enter key to Choose an action[/]")
            .PageSize(4)
            .MoreChoicesText("[magenta3](Move up and down to reveal more actions)[/]")
            .AddChoices("Add recipe", "Edit recipe or category", "List recipes", "Exit"));
            if (chooseAction == "Exit")
            {
                var name = AnsiConsole.Ask<string>("[gold1]Are you sure do you want to exit?\n No --> N\n Exit --> E [/]");
                if (name == "n" || name == "N") { Console.Clear(); goto startPoint1; }
            }
            else if (chooseAction == "Add recipe")
            {
                await r.AddRecipe();
                var name = AnsiConsole.Ask<string>("[gold1] Back --> B\n Exit --> E [/]");
                if (name == "B" || name == "b") { Console.Clear(); goto startPoint1; }
            }
            else if (chooseAction == "Edit recipe or category")
            {
                await r.EditRecipeOrCategory(Convert.ToInt32(AnsiConsole.Ask<string>("[chartreuse3_1]Please enter the Id for the recipe: [/]")));
                var name = AnsiConsole.Ask<string>("[gold1] Back --> B\n Exit --> E [/]");
                if (name == "B" || name == "b") { Console.Clear(); goto startPoint1; }
            }
            else if (chooseAction == "List recipes")
            {
                await r.ListRecipes();
                var name = AnsiConsole.Ask<string>("[gold1] Back --> B\n Exit --> E [/]");
                if (name == "B" || name == "b") { Console.Clear(); goto startPoint1; }
            }
        }
    }

    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Categories { get; set; }
        List<Recipe> recipes = new List<Recipe>();
        public Recipe()
        {
            Id = recipes.Count;
        }

        public async Task AddRecipe()
        {
            int id = recipes.Count;
            var title = AnsiConsole.Ask<string>("[steelblue1]Title: [/]");
            var ingredients = AnsiConsole.Ask<string>("[steelblue1]Ingredients: [/]");
            var instructions = AnsiConsole.Ask<string>("[steelblue1]Instructions: [/]");
            var Categories = AnsiConsole.Ask<string>("[steelblue1]Categories: [/]");
            recipes.Add(new Recipe
            {
                Id = id,
                Title = title,
                Ingredients = ingredients,
                Instructions = instructions,
                Categories = Categories
            });
            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);
        }

        public async Task ListRecipes()
        {
            string[] allTitles = new string[recipes.Count];
            if (allTitles.Length == 0)
            {
                AnsiConsole.Markup("[gold1]There is no recipes to list, please go back and add one at least\n[/]");
            }
            else
            {
                string deserializationString = await File.ReadAllTextAsync(@"Recipe.json");
                recipes = JsonSerializer.Deserialize<List<Recipe>>(deserializationString);
                for (int id = 0; id < recipes.Count; id++)
                {
                    allTitles[id] = recipes[id].Title;
                }
                var selectRecipe = AnsiConsole.Prompt
                (new SelectionPrompt<string>()
                .Title("Choose the recipe")
                .PageSize(10)
                .MoreChoicesText("[steelblue1](Move up and down to reveal more recipes)[/]")
                .AddChoices(allTitles));
                string detailsOfRecips = "";
                foreach (var recipe in recipes)
                {
                    if (selectRecipe == recipe.Title)
                    {
                        detailsOfRecips += "[steelblue1]Id:[/]" + $"[lightcoral] {recipe.Id + 1} \n[/]";
                        detailsOfRecips += "[steelblue1]Title:[/]" + $"[lightcoral] {recipe.Title} \n[/]";
                        detailsOfRecips += "[steelblue1]Ingredients:[/]" + $"[lightcoral] {recipe.Ingredients} \n[/]";
                        detailsOfRecips += "[steelblue1]Instructions:[/]" + $"[lightcoral] {recipe.Instructions} \n[/]";
                        detailsOfRecips += "[steelblue1]Categories:[/]" + $"[lightcoral] {recipe.Categories} \n[/]";
                    }
                }
                var panelOfRecipe = new Panel(detailsOfRecips);
                panelOfRecipe.Border = BoxBorder.Double;
                panelOfRecipe.Expand = true;
                AnsiConsole.Write(panelOfRecipe);
            }
        }

        public async Task EditRecipeOrCategory(int lineToEdit)
        {
            string jsonStringToEdit = await File.ReadAllTextAsync(@"Recipe.json");
            recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonStringToEdit);
            recipes[lineToEdit - 1].Title = AnsiConsole.Ask<string>("[steelblue1]Title: [/]");
            recipes[lineToEdit - 1].Ingredients = AnsiConsole.Ask<string>("[steelblue1]Ingredients: [/]");
            recipes[lineToEdit - 1].Instructions = AnsiConsole.Ask<string>("[steelblue1]Instructions: [/]");
            recipes[lineToEdit - 1].Categories = AnsiConsole.Ask<string>("[steelblue1]Categories: [/]");
            string jsonString = JsonSerializer.Serialize(recipes);
            await File.WriteAllTextAsync(@"Recipe.json", jsonString);
        }
    }
}
