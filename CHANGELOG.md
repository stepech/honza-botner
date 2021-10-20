﻿# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Calendar Versioning](https://calver.org).

## [Unreleased]

## [2021.2.0] - 2021-10-15

### Changed

- Version is now taken from `Directory.Build.props` config file. ([#218](https://github.com/fit-ctu-discord/honza-botner/pull/218))
- Using CalVer from now on.

### Fixed

- Getting users from DiscordGuild object sometimes failed, used async API instead. ([#217](https://github.com/fit-ctu-discord/honza-botner/pull/217))
- `::bot info` not responding in DMs. ([#209](https://github.com/fit-ctu-discord/honza-botner/pull/209))

## [1.0.2] - 2021-09-30

### Fixed

- Use async getter to fix potential issue with verification. ([#205](https://github.com/fit-ctu-discord/honza-botner/pull/205), [#206](https://github.com/fit-ctu-discord/honza-botner/pull/206))
- Fix Hangfire too-much-expired logs issue. ([#203](https://github.com/fit-ctu-discord/honza-botner/pull/203))
- Fix weird behavior of emotes command by adding a proper failed check handler. ([#202](https://github.com/fit-ctu-discord/honza-botner/pull/202))

## [1.0.1] - 2021-09-28

### Fixed

- Hangfire no longer breaks the app due to Heroku db connection limit. Number of workers have been limited to 3, db connections stays at 10-12/20. ([#196](https://github.com/fit-ctu-discord/honza-botner/pull/196))
- Path to FIT Discord logo in auth page. ([#196](https://github.com/fit-ctu-discord/honza-botner/pull/196))

## [1.0.0] - 2021-09-28

### Added

- Logging to Discord channel. ([#158](https://github.com/fit-ctu-discord/honza-botner/issues/158))
- Verification using buttons. ([#170](https://github.com/fit-ctu-discord/honza-botner/pull/170))
- Reminder command. ([#7](https://github.com/fit-ctu-discord/honza-botner/issues/7))
- Voice command and event handler, containing auto creating voice channel by joining special voice channel or using command to create and edit it. ([#68](https://github.com/fit-ctu-discord/honza-botner/pull/68))
- Granting roles event handler. ([#87](https://github.com/fit-ctu-discord/honza-botner/pull/87))
- Welcome message handler. ([#128](https://github.com/fit-ctu-discord/honza-botner/pull/128))
- Warning command, containing adding and listing warning to guild members. ([#113](https://github.com/fit-ctu-discord/honza-botner/pull/113))
- Bot command, containing activity changing and bot info.
- Channel command, containing channel cloning.
- Member command, containing guild member counting, getting info, and removing.
- Message command, containing message sending, editing, removing and binding handler to some reaction.
- Fun command, containing random picker of provided options. ([#122](https://github.com/fit-ctu-discord/honza-botner/pull/122))
- User pinning, containing event handler, soft pin pruning etc. ([#181](https://github.com/fit-ctu-discord/honza-botner/pull/181))
- Emotes counting command and event handler.
- Authorization using CTU OAuth2.
- Core functionality and design of the bot.
- Poll command, containing ABC or yes/no polls. ([#58](https://github.com/fit-ctu-discord/honza-botner/pull/58), [#78](https://github.com/fit-ctu-discord/honza-botner/pull/78))

[Unreleased]: https://github.com/https://github.com/fit-ctu-discord/honza-botner/compare/v2021.2.0...HEAD
[2021.2.0]: https://github.com/fit-ctu-discord/honza-botner/compare/v2021.2.0...v1.0.2
[1.0.2]: https://github.com/fit-ctu-discord/honza-botner/compare/v1.0.2...v1.0.1
[1.0.1]: https://github.com/fit-ctu-discord/honza-botner/compare/v1.0.1...v1.0.0
[1.0.0]: https://github.com/fit-ctu-discord/honza-botner/releases/tag/v1.0.0