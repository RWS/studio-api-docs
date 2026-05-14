# Copilot Instructions for Trados Studio API Documentation

## Project Overview
This is the **Trados Studio Public API documentation** repository, built with [DocFX](https://dotnet.github.io/docfx/). Content is authored in Markdown and structured via `toc.yml` files.

## Repository Structure
- `articles/` — Getting started guides, hints, and tips for Trados Studio plug-in development
- `apiconcepts/` — API concepts, guidelines, and API references (batch tasks, core, file type support, integration, project automation, terminology, translation memory, verification)
- `api/` — Auto-generated API reference metadata
- `Plugins/TradosStudioDocsPlugin/` — Custom DocFX plugin
- `RWSTemplate/` — DocFX site template
- `docfx.json` — DocFX configuration

## Writing Guidelines
- **Use active voice** over passive voice.
- **Use precise, strong verbs** — avoid weak verbs like "is", "occur", "happen".
- Write clear, concise sentences aimed at developers.
- Follow the detailed conventions in `writing_guidelines.md`.

## Variable Constructs
Use `Var:VariableName` to reference product names and versions. Do **not** hardcode product names. Available variables:
- `Var:ProductName` — e.g., Trados Studio
- `Var:ProductNameWithEdition` — e.g., Trados Studio 2021
- `Var:ProductVersion` — e.g., Studio16
- `Var:VersionNumber` — e.g., 16
- `Var:VisualStudioEdition` — e.g., the compatible Visual Studio edition

## Content Conventions
- New Markdown files must have a corresponding entry in the relevant `toc.yml`.
- Follow existing file and folder naming patterns (lowercase, descriptive names).
- Code samples should target the Trados Studio public API and use C#.
- Keep documentation consistent with existing articles in tone and structure.

## DocFX
- Build locally with `docfx docfx.json` to verify changes.
- The `docfx.json` file defines metadata sources (DLLs) and build configuration — avoid modifying unless necessary.
