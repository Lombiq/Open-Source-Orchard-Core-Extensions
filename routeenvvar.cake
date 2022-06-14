// Sets environment variables available from Github action process to child processes
var file = System.Environment.GetEnvironmentVariable("GITHUB_ENV");
System.IO.File.AppendAllText(file, $"ACTIONS_RUNTIME_URL={System.Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_URL")}\n");
System.IO.File.AppendAllText(file, $"ACTIONS_RUNTIME_TOKEN={System.Environment.GetEnvironmentVariable("ACTIONS_RUNTIME_TOKEN")}\n");
System.Environment.Exit(0);