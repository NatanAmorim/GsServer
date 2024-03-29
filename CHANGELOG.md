# Logging Changes

## English

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## Portuguese

Todas as alterações notáveis ​​neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog (PT-BR)](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere a [Versionamento Semântico (PT-BR)](https://semver.org/lang/pt-BR/).

<!-- TODO
## 1.0.0-RC-1 (Mar XX, 2024)
- Finish implementation of gRPC with the new protobufs.
-->

## 0.11.0-BETA (Mar 22, 2024)

- Rename Package and Project from "gs_server" to "GsServer".
- Update protobufs from `617c788f5` to `a1d46b748`.

## 0.10.0-BETA (Mar 22, 2024)

- Create "RequestTracerId" in every log using `HttpContext.TraceIdentifier`.
- Add Untested `AwsS3Service.cs`.
- Add Untested `AwsCloudWatch` for logging.
- Implement "DecimalValue" conversion to "C# Decimal".
- Update protobufs from `9e568008b` to `617c788f5`.

## 0.9.0-BETA (Mar 12, 2024)

- Rename every model and add the suffix "Model".
- Update minor changes in models that will not be documented because it is not ready for release and consumption yet.
- Update protobufs from `b1e3e6a23` to `9e568008b`.

## 0.8.0-BETA (Mar 11, 2024)

- Create InvoiceBackgroundService (not finished).
- Create Models "BackgroundJob, Notification, Subscription, SubscriptionBilling, SaleBilling, Promotion".
- Implement Cursor pagination (replaced GetAll).
- Update protobufs from `4bd2bcfe3` to `b1e3e6a23`.
- Every model ID now has a prefix in it to make it more readable.
- Update major changes in models that will not be documented because it is not ready for release and consumption yet.
- Rename "Teacher" to "Instructor".
- Remove control flow enums.

## 0.7.0-BETA (Mar 03, 2024)

- Create `CONTRIBUTING.md`.
- Create `CODE_OF_CONDUCT.md`.
- Create `Protos/` as a hack to solve proto import problem for now.
- Continue implementation of gRPC with the new protobufs.
- Update `CHANGELOG.md` move "How to contribute" to `CONTRIBUTING.md`.
- Add suffix "Model" to every model to avoid problems with generated files.
- Change "Address" data type to string and delete AddressModel.
- Picture now can be binary on upload and path from services like Imgur, AWS S3, Azure Blob, and more.
- Update protobufs  from `726541499` to `4bd2bcfe3`.
- Remove AutoMapper package.
- Update Microsoft.EntityFrameworkCore.Design package to "8.0.2".
- gs_test_db dev password.

## 0.6.0-BETA (Feb 29, 2024)

- Start implementation of gRPC with the new protobufs.
- Authentication and Authorization.
- Create `TODO.md`.
- Update protobufs  from `3c0043404` to `726541499`.
- Removed wildcard operator `*` in `<ItemGroup>` on `GsServer.csproj` causing a warning loop on vscode when there's an error.

## 0.5.0-BETA (Feb 27, 2024)

- Rename "Wards" to "Dependents" and use a Person model to make things simpler.
- Update wrong "MobilePhone" to "MobilePhoneNumber" in EntityFramework configuration.

## 0.4.0-BETA (Feb 27, 2024)

- Pictures are now stored as bytes.

## 0.3.0-BETA (Feb 27, 2024)

- change DataTypes of some Models.
- Removed Unnecessary and redundant "SubscribedDisciplines" in Customer Model.

## 0.2.1-BETA (Feb 27, 2024)

- Add new Brazilian ID "Cin" in Person, CIN (Carteira de Identidade Nacional).

## 0.2.0-BETA (Feb 27, 2024)

- Translate and Update Models to english.
- Update EntityFramework DataBase configuration for compatibility with new models.

## 0.1.1-BETA (Feb 09, 2024)

- Added Guidelines for copyright and trademarks in `README.md`.

## 0.1.0-BETA (Feb 08, 2024)

- Initial public release.
