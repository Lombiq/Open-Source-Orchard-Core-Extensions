# Repository working rules

These rules apply to work in the OSOCE superproject and its submodules. Use the skills under [skills](skills) when their workflows apply.

## Branches and commits

- Follow the user's latest branch choice in the superproject and affected submodules. If the user corrects the branch name, stop adding work to the previous branch and preserve its unrelated work.
- Do not prefix commit messages with the issue key. Use a concise description of the change; the branch already identifies the issue.
- End every PR description with a line break followed by the work item key on its own final line.
- Inspect staged and unstaged changes before committing. Include only the requested work, preserving unrelated user edits and staged changes.
- When integrating a submodule change, push its commit before pushing the superproject commit that references it.

## Temporary NuGet prereleases

When publishing a temporary issue prerelease is authorized:

1. Check the package's Versions tab on nuget.org or its NuGet version feed. Start from the latest stable production version, excluding prereleases, and increment its patch component.
2. Use the version format `<next-patch>-alpha.<index>.<lowercase-issue-key>`. Start the index at zero if that patch has no alpha prereleases; otherwise increment the highest existing alpha index across all issue suffixes. For a coordinated SDK release, check the packages released together and choose an available index for the whole set.
3. For example, stable `1.2.3` leads to `1.2.4-alpha.0.osoe-93`. If `1.2.4-alpha.0.osoe-89` already exists, use `1.2.4-alpha.1.osoe-93`; a subsequent release uses index `2`.
4. For repositories with tag-triggered publishing, tag the relevant issue-branch commit with `v<version>` and push the tag. Increment the index for subsequent publications instead of reusing a published version or moving its tag.
5. Verify both the publishing workflow and package availability from NuGet. A successful push can precede feed indexing; validate consumer restore against the published package once it is available.

Keep all SDK alpha references in sync with the coordinated prerelease, including the NuGet unit and UI test projects and SDK sample projects. Search project files, imports, and SDK version configuration across the superproject and submodules rather than updating only the first consumer found.

## SDK consumer contracts

UI test projects should need only `Lombiq.MSBuild.OrchardCore.Tests.UI.Sdk` for the UI testing setup. That SDK must supply the executable runner configuration and the `Lombiq.Tests.UI` dependency. Fix missing setup in the UI SDK instead of switching consumers to the unit test SDK or adding compensating package references.

Reuse the common test SDK configuration through the existing SDK inheritance pattern. Ensure the published UI SDK package contains the inherited imports, and verify both local submodule imports and a consumer of the published package.

## Documentation

Avoid fixed package-version wording (such as `xunit.v3` 4.0.0 or xUnit 4) in ongoing documentation. Describe the package or capability instead. Keep specific versions in migration guides when they identify the required upgrade.

## Test harnesses and validation

- Keep regression harnesses simple while preserving their scenarios, failure diagnostics, environment restoration, and exit-code behavior.
- Determine test success from process exit codes, not success-looking log text. Regression coverage should include filtering, theory cases, passing output, reports, diagnostics, empty selections, and intentional failures.
- An empty fixture-level `Directory.Build.props` stops MSBuild from inheriting a containing repository's build settings. Keep an explanatory comment when using this isolation mechanism.
- Run the repository's PowerShell analyzer settings with its custom rules as well as the default rules. A default-only analyzer run can miss repository-specific violations.
- Use existing tests appropriate to the change. A successful build alone does not prove that an MTP test project can launch; verify execution or discovery, including published SDK consumers when SDK packaging changes.
- Report CI results for the current PR revision. Distinguish companion-repository checks from superproject integration checks, and pending or cancelled runs from passing runs. When asked to wait for CI to pass, wait for completion and investigate failures rather than reporting an earlier passing revision.
