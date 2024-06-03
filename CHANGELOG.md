# Logging Changes

## English

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## Portuguese

Todas as alterações notáveis ​​neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog (PT-BR)](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere a [Versionamento Semântico (PT-BR)](https://semver.org/lang/pt-BR/).

## 1.0.0-RC-7 (Jun 03, 2024)

- Update protobufs from `d5bc3684d` to `a5d081123`, with breaking changes.

## 1.0.3-RC-6 (May 27, 2024)

- Fix all "GetPaginated" not working with a cursor that was not null (some GET functions are still missing "include" arguments on the select).
- Update All AWS packages to the more recent version.

## 1.0.2-RC-6 (May 22, 2024)

- Add "Include Error Detail=True" to Postgres connection string in debug mode.
- Add missing Authorization in services.
- Update Person & Customer model definition.
- Update all "GetPaginated" can have null as a cursor value in case is the first fetch.
- Update protobufs from `3d4788411` to `d5bc3684d`.

## 1.0.1-RC-6 (May 08, 2024)

- Better "ErrorMessage" in some validations.

## 1.0.0-RC-6 (May 05, 2024)

- Remove "AutoMapper".
- Remove "BackgroundJobStatus" from DB (because logs are enough to keep track).
- Update all Models and add "FromProtoRequest" and "ToGetById" on most of them.
- Update all Services To use "FromProtoRequest" and "ToGetById" instead of AutoMapper.
- Update protobufs from `15b4f0054` to `3d4788411`.

## 1.0.2-RC-5 (May 01, 2024)

- "SubscriptionInvoiceBackgroundJob" now is tracked in the DB.
- User now has "IsActive" to know if email is confirmed valid and allow it to login.
- Some User properties changes for the customer App.
- Cleaned some unused test code.

## 1.0.1-RC-5 (Apr 30, 2024)

- Remove unnecessary error messages when they won't reach the end user.
- Add a variable character limit for RefreshToken.

## 1.0.0-RC-5 (Apr 24, 2024)

- Change "CreatedBy" data type from `Int` to `ULID`.
- Rename All "Comments" properties to "Observations".
- Update AutoMapper to support Ulid.
- Update protobufs from `0adaf3a56` to `15b4f0054`.

## 1.0.0-RC-4 (Apr 24, 2024)

- Add `Ulid` package from Cysharp, also added value converter.
- Change every Model Id to ULID.
- Change Default Id on Postgres from `Serial` to `Identity`.
- Rename Folder `CustomTypes` to `ValuesConverter`.
- Rename Table "Persons" to "People".

## 1.0.0-RC-3 (Apr 23, 2024)

- Change some data validation.
- Improve mapping of SQL relationships in EF Core.

## 1.0.0-RC-2 (Apr 21, 2024)

- Change some data validation.
- Fix instances where a composite index was created, instead of multiple indexes.

## 1.0.0-RC-1 (Apr 18, 2024)

- Finish basic implementation of gRPC services with the new protobufs, some stuff will be implemented later, but it's not needed for critical functionality.
- Add Middleware `GlobalExceptionHandler` to be able to log and return even when unknown errors occur, but at the moment does not work with gRPC.
- Update protobufs from `a1d46b748` to `0adaf3a56`.

## 0.11.0-BETA (Mar 22, 2024)

- Rename Package and Project from "gs_server" to "GsServer".
- Update protobufs from `617c788f5` to `a1d46b748`.

## 0.10.0-BETA (Mar 22, 2024)

- Create "RequestTracerId" in every log using `HttpContext.TraceIdentifier`.
- Add **Untested** `AwsS3Service.cs`.
- Add **Untested** `AwsCloudWatch` for logging.
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
