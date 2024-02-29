# Change Log

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
### Added
### Changed
### Fixed
-->

## 0.6.0 (Feb 29, 2024)

- added: Start implementation of gRPC with the new protobufs.
- added: Authentication and Authorization.
- added: Create `TODO.md`.
- changed: Update protobufs to the most recent version.
- changed: Removed wildcard operator `*` in `<ItemGroup>` on `gs_server.csproj` causing a warning loop on vscode when there's an error.

## 0.5.0 (Feb 27, 2024)

- changed: Rename "Wards" to "Dependents" and use a Person model to make things simpler.
- changed: Update wrong "MobilePhone" to "MobilePhoneNumber" in EntityFramework configuration.

## 0.4.0 (Feb 27, 2024)

- Pictures are now stored as bytes.

## 0.3.0 (Feb 27, 2024)

- changed: change DataTypes of some Models.
- changed: Removed Unnecessary and redundant "SubscribedDisciplines" in Customer Model.

## 0.2.1 (Feb 27, 2024)

- added: Add new Brazilian ID "Cin" in Person, CIN (Carteira de Identidade Nacional).

## 0.2.0 (Feb 27, 2024)

- changed: Translate and Update Models to english.
- changed: Update EntityFramework DataBase configuration for compatibility with new models.

## 0.1.1 (Feb 09, 2024)

- changed: Added Guidelines for copyright and trademarks in `README.md`.

## 0.1.0 (Feb 08, 2024)

- Initial public release.
