using Lombiq.Tests.UI.Attributes;
using Lombiq.Tests.UI.Constants;
using Lombiq.Tests.UI.Exceptions;
using Lombiq.Tests.UI.Extensions;
using Lombiq.Tests.UI.Services;
using Shouldly;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Lombiq.OSOCE.Tests.UI.Tests;

public class SecurityShortcutsTests : UITestBase
{
    private const string UserUserName = "user";
    private const string UserEmail = "user@example.com";
    private const string AuthorRole = "Author";
    private const string FakeRole = "Fake";
    private const string ViewContentTypesPermission = "ViewContentTypes";
    private const string FakePermission = "Fake";

    public SecurityShortcutsTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [Theory, Chrome]
    public Task AddUserToRoleShouldWork(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await CreateUserAndSignInAsync(context);

                await context.GoToContentTypesListAsync();
                context.GetCurrentUri().AbsolutePath
                    .ShouldBe("/Error/403");
                await context.SignOutDirectlyAsync();

                await context.AddUserToRoleAsync(UserUserName, AuthorRole);
                await context.AddPermissionToRoleAsync(ViewContentTypesPermission, AuthorRole);

                await context.SignInDirectlyAsync(UserUserName);
                await context.GoToContentTypesListAsync();

                context.GetCurrentUri().AbsolutePath
                    .ShouldBe("/Admin/ContentTypes/List");
            },
            browser,
            configuration =>
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    [Theory, Chrome]
    public Task AddUserToFakeRoleShouldThrow(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.AddUserToRoleAsync(UserUserName, FakeRole)
                    .ShouldThrowAsync<UserNotFoundException>();

                await context.CreateUserAsync(UserUserName, DefaultUser.Password, UserEmail);
                await context.AddUserToRoleAsync(UserUserName, FakeRole)
                    .ShouldThrowAsync<RoleNotFoundException>();

                CleanUpLogs(context);
            },
            browser,
            configuration =>
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    [Theory, Chrome]
    public Task AllowFakePermissionToRoleShouldThrow(Browser browser) =>
        ExecuteTestAfterSetupAsync(
            async context =>
            {
                await context.AddPermissionToRoleAsync(FakePermission, AuthorRole)
                    .ShouldThrowAsync<PermissionNotFoundException>();

                CleanUpLogs(context);
            },
            browser,
            configuration =>
                configuration.HtmlValidationConfiguration.RunHtmlValidationAssertionOnAllPageChanges = false);

    private static async Task CreateUserAndSignInAsync(UITestContext context)
    {
        await context.CreateUserAsync(UserUserName, DefaultUser.Password, UserEmail);
        await context.SignInDirectlyAsync(UserUserName);
        (await context.GetCurrentUserNameAsync())
            .ShouldBe(UserUserName);
    }

    private static void CleanUpLogs(UITestContext context)
    {
        // Remove logs to have a clean slate.
        foreach (var log in context.Application.GetLogs()) log.Remove();
        context.ClearHistoricBrowserLog();
    }
}
