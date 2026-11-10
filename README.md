# components

## NuGet publishing

The GitHub Actions workflow publishes both packages when a version tag such as
`v1.2.3` is pushed. Configure these repository secrets before publishing:

- `NUGET_API_KEY`: an API key with permission to publish the packages.
- `STRONG_NAME_KEY_BASE64`: the base64-encoded private strong-name `.snk` file.

The publishing workflow signs both assemblies with the strong-name key and uses
the tag version as the NuGet package version.
