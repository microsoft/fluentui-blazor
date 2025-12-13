# DocApiGen - Generation Modes

## Overview

Le projet DocApiGen supporte maintenant deux modes de génération:

### 1. Mode **Summary** (Par défaut)
- Génère une documentation résumée contenant uniquement les propriétés marquées avec l'attribut `[Parameter]`
- Compatible avec les formats `csharp` et `json`
- Utilisé par l'ApiClassGenerator pour la génération traditionnelle

### 2. Mode **All**
- Génère une documentation complète incluant toutes les propriétés, méthodes et événements
- Compatible avec les formats `csharp`, `json` et `mcp`
- Le format `mcp` utilise toujours ce mode par défaut
- Utilisé par le McpDocumentationGenerator

## Utilisation

### Ligne de commande

```bash
DocApiGen --xml <xml_file> --dll <dll_file> --output <generated_file> --format <csharp|json|mcp> --mode <summary|all>
```

### Paramètres

- `--xml` : Fichier XML de documentation
- `--dll` : Fichier DLL à analyser
- `--output` : Fichier de sortie (optionnel)
- `--format` : Format de sortie
  - `csharp` : Code C# avec dictionnaire de données
  - `json` : JSON avec données résumées
  - `mcp` : JSON complet pour McpServer
- `--mode` : Mode de génération (optionnel, par défaut: `summary`)
  - `summary` : Documentation avec uniquement les propriétés `[Parameter]`
  - `all` : Documentation complète avec toutes les propriétés, méthodes et événements

### Exemples

#### Génération Summary (mode par défaut)
```bash
DocApiGen --xml MyLib.xml --dll MyLib.dll --output docs.json --format json
```

```bash
DocApiGen --xml MyLib.xml --dll MyLib.dll --output docs.json --format json --mode summary
```

#### Génération All (complète)
```bash
DocApiGen --xml MyLib.xml --dll MyLib.dll --output docs.json --format json --mode all
```

#### Génération MCP (toujours en mode All)
```bash
DocApiGen --xml MyLib.xml --dll MyLib.dll --output mcp-docs.json --format mcp
```

**Note**: Le format `mcp` utilise toujours le mode `all` indépendamment du paramètre `--mode`.

## Architecture

### Classes principales

- **GenerationMode** (enum) : Définit les deux modes de génération
  - `Summary` : Documentation résumée
  - `All` : Documentation complète

- **ApiClassOptions** : Options de configuration
  - Propriété `Mode` : Permet de basculer entre Summary et All
  - Propriété `PropertyParameterOnly` : Contrôle interne lié au mode

- **ApiClassGenerator** : Générateur de documentation traditionnel
  - Méthodes `GenerateCSharp(mode)` et `GenerateJson(mode)`
  - Support des deux modes

- **McpDocumentationGenerator** : Générateur MCP
  - Utilise toujours `GenerationMode.All`
  - Génère une documentation JSON complète

## Modifications apportées

1. Ajout de l'enum `GenerationMode` dans `Models/GenerationMode.cs`
2. Ajout de la propriété `Mode` dans `ApiClassOptions`
3. Mise à jour de `ApiClassGenerator` pour supporter le paramètre `mode`
4. Mise à jour de `McpDocumentationGenerator` pour utiliser explicitement `GenerationMode.All`
5. Mise à jour de `Program.cs` pour gérer le paramètre `--mode`

## Compatibilité

- Le comportement par défaut reste inchangé (mode Summary)
- Les scripts existants continueront de fonctionner sans modification
- L'ajout du paramètre `--mode all` permet d'obtenir une documentation plus complète
