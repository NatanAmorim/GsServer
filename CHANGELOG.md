# Logging Changes

## English

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## Portuguese

Todas as alterações notáveis ​​neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog (PT-BR)](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere a [Versionamento Semântico (PT-BR)](https://semver.org/lang/pt-BR/).

<!--
for copy and paste
added:
changed:
fixed:
-->

<!-- TODO
## 1.0.0-RC-1 (Mar 12, 2024)
- changed:
-->

## 0.9.0-BETA (Mar 12, 2024)

- changed: Rename every model and add the suffix "Model".
- changed: Update minor changes in models that will not be documented because it is not ready for release and consumption yet.
- changed: Update protobufs from `b1e3e6a23` to `9e568008b`.

## 0.8.0-BETA (Mar 11, 2024)

- added: Create InvoiceBackgroundService (not finished).
- added: Create Models "BackgroundJob, Notification, Subscription, SubscriptionBilling, SaleBilling, Promotion".
- changed: Implement Cursor pagination (replaced GetAll).
- changed: Update protobufs from `4bd2bcfe3` to `b1e3e6a23`.
- changed: Every model ID now has a prefix in it to make it more readable.
- changed: Update major changes in models that will not be documented because it is not ready for release and consumption yet.
- changed: Rename "Teacher" to "Instructor".
- changed: Remove control flow enums.

## 0.7.0-BETA (Mar 03, 2024)

- added: Create `CONTRIBUTING.md`.
- added: Create `CODE_OF_CONDUCT.md`.
- added: Create `Protos/` as a hack to solve proto import problem for now.
- changed: Continue implementation of gRPC with the new protobufs.
- changed: Update `CHANGELOG.md` move "How to contribute" to `CONTRIBUTING.md`.
- changed: Add suffix "Model" to every model to avoid problems with generated files.
- changed: Change "Address" data type to string and delete AddressModel.
- changed: Picture now can be binary on upload and path from services like Imgur, AWS S3, Azure Blob, and more.
- changed: Update protobufs  from `726541499` to `4bd2bcfe3`.
- changed: Remove AutoMapper package.
- changed: Update Microsoft.EntityFrameworkCore.Design package to "8.0.2".
- changed: gs_test_db dev password.

## 0.6.0-BETA (Feb 29, 2024)

- added: Start implementation of gRPC with the new protobufs.
- added: Authentication and Authorization.
- added: Create `TODO.md`.
- changed: Update protobufs  from `3c0043404` to `726541499`.
- changed: Removed wildcard operator `*` in `<ItemGroup>` on `gs_server.csproj` causing a warning loop on vscode when there's an error.

## 0.5.0-BETA (Feb 27, 2024)

- changed: Rename "Wards" to "Dependents" and use a Person model to make things simpler.
- changed: Update wrong "MobilePhone" to "MobilePhoneNumber" in EntityFramework configuration.

## 0.4.0-BETA (Feb 27, 2024)

- Pictures are now stored as bytes.

## 0.3.0-BETA (Feb 27, 2024)

- changed: change DataTypes of some Models.
- changed: Removed Unnecessary and redundant "SubscribedDisciplines" in Customer Model.

## 0.2.1-BETA (Feb 27, 2024)

- added: Add new Brazilian ID "Cin" in Person, CIN (Carteira de Identidade Nacional).

## 0.2.0-BETA (Feb 27, 2024)

- changed: Translate and Update Models to english.
- changed: Update EntityFramework DataBase configuration for compatibility with new models.

## 0.1.1-BETA (Feb 09, 2024)

- changed: Added Guidelines for copyright and trademarks in `README.md`.

## 0.1.0-BETA (Feb 08, 2024)

- Initial public release.
