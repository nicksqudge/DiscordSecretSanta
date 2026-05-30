# DiscordSecretSanta
A discord bot for organising secret santa on your server

## Libraries:
 - Discord.NET
 - For Testing:
    - Shouldly
    - FakeItEasy

## Glossary:

### Admin:
Someone that can manage the secret santa, this doesn't have to be a server admin. But ALL server admins are going to be able to admin the secret santa. This is for adding other users that you don't want to make server admins.

## Development Notes:

 - Commands are setup in the `CommandsModule.cs` file
 - Commands are registered into the Dependency Injection on the `ServiceCollectionExtensions.cs`