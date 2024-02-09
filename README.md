# README

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

Licensed under the Apache License, Version 2.0; you may not use this app except in compliance with the License. You may obtain a copy of the License at <https://opensource.org/licenses/Apache-2.0>.

- [About](#about)
- [Recommended tools](#recommended-tools)
- [Building project](#building-project)
- [Contributing Guidelines](#contributing-guidelines)

> [!IMPORTANT]\
> Potential legal non-compliance (This is not legal advice).\
> This software is currently under development and may not yet comply with all legal requirements and specific laws, like GDPR. There may be potential risks associated with using the software in its current state.

## About

Server used by the company "Gislaine Studio de Dança" in Andradina/Brazil.

> [!Warning]\
> **Guidelines for copyright and trademarks**
> 
> This project may contain trademarks or logos for projects, products, or services.
> The images included in this repository are not part of the open-source license and cannot be freely used or modified without explicit permission. These images are protected by copyright and are provided solely for reference purposes within the context of this project.

Individuals are not authorized to use these images (bitmap and vector) for any purpose without obtaining explicit permission from the copyright holder. Modifying these images is strictly prohibited without prior consent from the copyright holder. Permission is required for any commercial or non-commercial use of the images, including but no limited to advertising, marketing, or product development.

By accessing or using the images in this repository, you agree to abide by these usage guidelines and respect the intellectual property rights associated with the images.

Trademarks are names and designs that tell the world the source of a good or service. Protecting trademarks for an open source project is particularly important. Anyone can change the source code and produce a product from that code, so it’s important that only the original product, or variations that have been approved by the project, use the project’s trademarks. Trademarks cannot be used in ways that appear (to a casual observer) official, affiliated, or endorsed by the original project.

TL;DR: Use of trademarks or logos in modified versions of this project must not cause confusion or imply sponsorship. Any use of third-party trademarks or logos are subject to those third-party's policies.


## gRPC Documentation

- [Official website](https://grpc.io/) - Official documentation, libraries, resources, samples and FAQ
- [Technical documentation](https://github.com/grpc/grpc/tree/master/doc) - Collection of useful technical documentation
- [gRPC status codes](https://github.com/grpc/grpc/blob/master/doc/statuscodes.md) - Status codes and their use in gRPC
- [gRPC status code mapping](https://github.com/grpc/grpc/blob/master/doc/http-grpc-status-mapping.md) - HTTP to gRPC Status Code Mapping
- [grpc-errors](https://github.com/avinassh/grpc-errors) - Code examples in each language on how to return and handle error statuses.
- [API Design Guide](https://cloud.google.com/apis/design/) - Google Cloud API Design Guide useful for gRPC API design insights

## Recommended tools

To get started, I recommend you to have the following tools installed:

### Visual Studio Code

- I recommend installing the latest version of [Visual Studio Code](https://code.visualstudio.com/).
- I use the extension [C# for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp).
- My VS Code setup is available in [NatanAmorim/my-config](https://github.com/NatanAmorim/my-config) at [vscode_config.md](https://github.com/NatanAmorim/my-config/blob/master/vscode_config.md).

### Dotnet SDK

For .NET development, install [.NET Core SDK](https://dotnet.microsoft.com/download) I used the [.NET SDK 8.0](https://get.dot.net/8) for this project.

### Install Docker

- installation on [Mac](https://docs.docker.com/desktop/install/mac-install/)
- installation on [Linux](https://docs.docker.com/engine/install/ubuntu/)
- installation on [Windows](https://docs.docker.com/docker-for-windows/install/) (I recommend to use [Windows Subsystem for Linux 2](https://docs.microsoft.com/en-us/windows/wsl/wsl2-kernel))

#### PostgreSQL on Docker

- `docker pull postgres`
- `docker run --name postgresql -e POSTGRES_PASSWORD=<password> -p 5432:5432 -d postgres`

## Building project

The following instructions are for dotnet-cli that comes with the [.NET Core SDK](#dotnet-sdk):

```sh
dotnet restore
dotnet build
```

## Contributing Guidelines

We prefer all communications to be in English or Portuguese.

This project uses GitHub Issues to track bugs and feature requests. Please search the existing issues before filing new issues to avoid duplicates. For new issues, file your bug or feature request as a new Issue.

For help and questions about using this project, please uses the GitHub discussions.

### Security
<!--
Please do not report security vulnerabilities through public GitHub issues.\
Instead, please report them to {email-address}.\
You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message.
-->

If you believe you have found a security vulnerability in this software, we encourage you to inform us immediately. We will investigate all legitimate reports and do our best to quickly correct the issue.

Please include the requested information listed below (as much as you can provide) to help us better understand the nature and scope of the possible issue:

- Type of issue (e.g. buffer overflow, SQL injection, cross-site scripting, etc.)
- Full paths of source file(s) related to the manifestation of the issue
- The location of the affected source code (tag/branch/commit or direct URL)
- Any special configuration required to reproduce the issue
- Step-by-step instructions to reproduce the issue
- Proof-of-concept or exploit code (if possible)
- Impact of the issue, including how an attacker might exploit the issue

This information will help us triage your report more quickly.

### Contributing Code

1. Fork the repository and clone it to your local machine.
2. Create a new branch for your changes: `git checkout -b my-new-feature`
3. Make your changes and commit them: `git commit -am 'Add some feature'`
    - Include tests that cover your changes.
    - Update the documentation to reflect your changes, where appropriate.
    - Add an entry to the `CHANGELOG.md` file describing your changes if appropriate.
4. Push your changes to your fork: `git push origin my-new-feature`
5. Create a pull request from your fork to the main repository. `gh pr create` (with the GitHub CLI)

### Pull Request

- Clearly describe what you aim to add or fix.
- Try to minimize code changes and use existing style/functions.

### Reporting Issues and Bugs

If you find a bug, please report it by opening a new issue in the issue tracker. Please include as much detail as possible, including steps to reproduce the bug and any relevant error messages.

To better respond to issues please follow these general guidelines when explaining the problem.

1. Clearly describe what you aim to fix, if relevant attach output/logs/screenshots.
2. Describe how developers can reproduce the bug.
